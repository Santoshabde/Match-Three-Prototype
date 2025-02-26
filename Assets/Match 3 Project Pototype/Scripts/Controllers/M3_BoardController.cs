using System.Collections.Generic;
using SNGames.CommonModule;
using UnityEngine;

namespace SNGames.M3
{
    public enum GamePiceType
    {
        None,
        Blue,
        Green,
        Red,
        Yellow,
        Purple,
        Orange
    }

    public class M3_BoardController : MonoBehaviour
    {
        [Header("Required Componenets")]
        [SerializeField] private M3_Tile tilePrefab;
        [SerializeField] private List<M3_GamePiece> allGamePieces;
        [SerializeField] private M3_CameraController cameraController;

        [Space]
        [Header("Board Settings")]
        [SerializeField] private int width;
        [SerializeField] private int height;

        void OnDestroy()
        {
            ClearExistingTilesInGame();
            ClearExistingGamePiecesInGame();
        }

        #region  Public Region
        public void InitialSpawnBoardTiles()
        {
            ClearExistingTilesInGame();

            var tilesOnBoard = new M3_Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    M3_Tile tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    tile.Init(x, y, null);
                    tilesOnBoard[x, y] = tile;

                    tile.OnTileClicked += ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileClicked;
                    tile.OnTileHovered += ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileHovered;
                }
            }

            SetCameraPositionAndFOVBasedOnBoardSize();

            ServiceRegistry.Get<M3_Service_BoardData>().SetTilesOnBoard(tilesOnBoard);
        }

        public void InitialSpawnRandomGamePicesOnTheBoard(M3_Tile[,] tilesOnBoard)
        {
            ClearExistingGamePiecesInGame();

            var gamePiecesOnBoard = new M3_GamePiece[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    M3_GamePiece gamePiece = Instantiate(allGamePieces[Random.Range(0, allGamePieces.Count)], new Vector3(x, y, 0), Quaternion.identity, transform);
                    gamePiece.Init(x, y, tilesOnBoard[x, y]);
                    tilesOnBoard[x, y].Init(x, y, gamePiece);

                    gamePiecesOnBoard[x, y] = gamePiece;
                }
            }

            ServiceRegistry.Get<M3_Service_BoardData>().SetGamePiecesOnBoard(gamePiecesOnBoard);
        }

        #endregion

        #region  Private Region

        private void SetCameraPositionAndFOVBasedOnBoardSize()
        {
            cameraController.SetCameraPositionAndFOV(width, height);
        }

        private void ClearExistingTilesInGame()
        {
            var tilesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard();
            if (tilesOnBoard == null)
            {
                return;
            }

            for (int x = 0; x < tilesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < tilesOnBoard.GetLength(1); y++)
                {
                    if (tilesOnBoard[x, y] != null)
                    {
                        tilesOnBoard[x, y].OnTileClicked -= ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileClicked;
                        tilesOnBoard[x, y].OnTileHovered -= ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileHovered;

                        DestroyImmediate(tilesOnBoard[x, y].gameObject);
                    }
                }
            }
        }

        private void ClearExistingGamePiecesInGame()
        {
            var gamePiecesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetGamePiecesOnBoard();
            if (gamePiecesOnBoard == null)
            {
                return;
            }

            for (int x = 0; x < gamePiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < gamePiecesOnBoard.GetLength(1); y++)
                {
                    if (gamePiecesOnBoard[x, y] != null)
                    {
                        DestroyImmediate(gamePiecesOnBoard[x, y].gameObject);
                    }
                }
            }
        }

        #endregion
    }
}