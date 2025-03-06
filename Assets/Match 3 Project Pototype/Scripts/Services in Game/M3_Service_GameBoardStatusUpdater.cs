using UnityEngine;
using SNGames.CommonModule;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.Tilemaps;

namespace SNGames.M3
{
    public class M3_Service_GameBoardStatusUpdater : BaseService
    {
        public override void Init()
        {
            SNEventsController<M3_InGameEvents>.RegisterEvent<(M3_Tile, M3_Tile)>(M3_InGameEvents.SWAP_COMPLETED, OnSwapCompleted);
            SNEventsController<M3_InGameEvents>.RegisterEvent<List<M3_Tile>>(M3_InGameEvents.EMPTY_BOARD_SPORTS_FILLED, OnEmptyBoardSpotsFilled);
        }

        public override void Deinit()
        {
            SNEventsController<M3_InGameEvents>.DeregisterEvent<(M3_Tile, M3_Tile)>(M3_InGameEvents.SWAP_COMPLETED, OnSwapCompleted);
            SNEventsController<M3_InGameEvents>.DeregisterEvent<List<M3_Tile>>(M3_InGameEvents.EMPTY_BOARD_SPORTS_FILLED, OnEmptyBoardSpotsFilled);
        }

        private void OnSwapCompleted((M3_Tile, M3_Tile) clickedAndHoveredTile)
        {
            M3_Tile clickedTile = clickedAndHoveredTile.Item1;
            M3_Tile hoveredTile = clickedAndHoveredTile.Item2;

            if (clickedTile == null || hoveredTile == null)
            {
                Debug.LogError("Clicked or Hovered Tile is null");
                return;
            }

            var possibleClickedTileMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(clickedTile);
            var possibleHoveredTileMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(hoveredTile);

            var mergedMatches = possibleClickedTileMatches
                    .Concat(possibleHoveredTileMatches)
                    .Distinct()
                    .ToList();

            CleanBoardMatches(mergedMatches);
        }

        public void CleanBoardMatches(List<M3_GamePiece> matchesFound)
        {
            if (matchesFound == null || matchesFound.Count == 0)
            {
                ServiceRegistry.Get<M3_GameInputService>().ConsumeInput = true;
                return;
            }

            List<int> uniqueColumnNumbersToRearrange = new List<int>();
            foreach (var gamePiece in matchesFound)
            {
                if (gamePiece != null)
                {
                    if (!uniqueColumnNumbersToRearrange.Contains(gamePiece.BoardPosition.x))
                        uniqueColumnNumbersToRearrange.Add(gamePiece.BoardPosition.x);

                    //1. Remove the GamePiece from the Tile
                    gamePiece.CurrentGamePieceTile.SetTileGamePiece(null);

                    //2. Remove the GamePiece from the BoardData
                    ServiceRegistry.Get<M3_Service_BoardData>().NullTheGamePiceOnBoardData(gamePiece);

                    //3. Destroy the GamePiece
                    M3_VFXController.Instance.SpawnVFX("Match", gamePiece.transform.position, Quaternion.identity, Vector3.one);

                    GameObject.Destroy(gamePiece.gameObject);
                }
            }

            M3_ServiceMonobehaviourHelper.Instance.StartCoroutine(Delay_DropTheBoardAfterMatchesClearing(uniqueColumnNumbersToRearrange));
        }

        IEnumerator Delay_DropTheBoardAfterMatchesClearing(List<int> uniqueColumnNumbersToRearrange)
        {
            yield return new WaitForSeconds(0.25f);
            DropTheBoardAfterMatchesClearing(uniqueColumnNumbersToRearrange);
        }

        private void DropTheBoardAfterMatchesClearing(List<int> uniqueColumnNumbersToRearrange)
        {
            if (uniqueColumnNumbersToRearrange.Count >= 0)
            {
                //Stop input
                ServiceRegistry.Get<M3_GameInputService>().ConsumeInput = false;
            }

            var gameBoardService = ServiceRegistry.Get<M3_Service_BoardData>();
            var gamePiecesOnBoard = gameBoardService.GetGamePiecesOnBoard();
            var tilesOnBoard = gameBoardService.GetTilesOnBoard();
            var boardHeight = gameBoardService.GetBoardWidthAndHeight().Item2;

            foreach (var columnNumber in uniqueColumnNumbersToRearrange)
            {
                int currentX = columnNumber;
                // --Track the lowest empty space
                int emptyY = -1;

                for (int y = 0; y < boardHeight; y++)
                {
                    if (tilesOnBoard[currentX, y].TileType == TileType.InvisibleBlocked)
                        continue;

                    if (gamePiecesOnBoard[currentX, y] == null)
                    {
                        // -- If this is the first empty space found, track it
                        if (emptyY == -1)
                            emptyY = y;
                    }
                    // -- Found a piece above an empty space, shift it down
                    else if (emptyY != -1)
                    {
                        M3_GamePiece gamePieceToMove = gamePiecesOnBoard[currentX, y];
                        M3_Tile tileToMoveTo = gameBoardService.GetTilesOnBoard()[currentX, emptyY];
                        M3_Tile oldTile = gameBoardService.GetTilesOnBoard()[currentX, y];

                        Debug.Log(gamePieceToMove.name + " is moving to " + tileToMoveTo.name + " from " + oldTile.name);

                        gameBoardService.UpdateNewGamePieceDataOnBoardFromOneTileToAnother(
                            gamePieceToMove, tileToMoveTo, oldTile
                        );

                        gamePieceToMove.MovePieceToTile(tileToMoveTo);

                        // -- Go to next empty space!!
                        emptyY++;
                    }
                }
            }

            M3_ServiceMonobehaviourHelper.Instance.StartCoroutine(LoopCheckForMatchesAfterBoardDrop(gameBoardService));
        }

        private IEnumerator LoopCheckForMatchesAfterBoardDrop(M3_Service_BoardData gameBoardService)
        {
            yield return new WaitForSeconds(0.3f); // Wait for board updates

            bool matchesFound = false;

            foreach (var piece in gameBoardService.GetTilesOnBoard())
            {
                var matches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(piece);
                if (matches != null && matches.Count > 0)
                {
                    matchesFound = true;
                    CleanBoardMatches(matches); //Recursion function
                    yield break; // Exit the coroutine so it starts again
                }
            }

            // If no new matches were found, recursion is complete
            if (!matchesFound)
            {
                //Spawn new board in these places
                SNEventsController<M3_InGameEvents>.TriggerEvent<object>(M3_InGameEvents.FILL_EMPTY_BOARD_SPOTS, null);
            }
        }

        private void OnEmptyBoardSpotsFilled(List<M3_Tile> list)
        {
            M3_ServiceMonobehaviourHelper.Instance.StartCoroutine(RecheckForMatchesAfterFillingEmptyBoard(list));

            IEnumerator RecheckForMatchesAfterFillingEmptyBoard(List<M3_Tile> newTilesWhereWeSpawnedRandoms)
            {
                yield return new WaitForSeconds(0.32f);
                var matches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(newTilesWhereWeSpawnedRandoms);
                CleanBoardMatches(matches);
            }
        }
    }
}
