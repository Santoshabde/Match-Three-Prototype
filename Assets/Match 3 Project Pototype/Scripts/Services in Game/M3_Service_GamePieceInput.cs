using DG.Tweening;
using SNGames.CommonModule;
using UnityEngine;

namespace SNGames.M3
{
    public class M3_Service_GamePieceInput : BaseService
    {
        private bool currentTileIsStatic = true;
        private bool hoveredTileIsStatic = true;

        private M3_Tile clickedTile;
        private M3_Tile hoveredTile;

        public override void Deinit()
        {

        }

        public override void Init()
        {

        }

        public void OnTileClicked(M3_Tile tile)
        {
            if (IsClickedAndHoveredGamePicesNOTInMovingState()
            && tile.CanHoldNormalGamePiece())
            {
                clickedTile = tile;
            }
        }

        public void OnTileHovered(M3_Tile tile)
        {
            //Already some tiles are in moving state? or is clicked tile null?
            if (!IsClickedAndHoveredGamePicesNOTInMovingState() || clickedTile == null) return;

            //Hovered Tile should Not be same as clicked tile AND Hovered Tile should be a neighbour of clicked tile
            hoveredTile = tile;
            if (!CanInitiateSwapBtwClickedAndHoveredTile()) return;

            //If code reaches here - that we are qualified to TRY swapping tiles
            hoveredTileIsStatic = false;
            currentTileIsStatic = false;

            int movedCount = 0;
            clickedTile.TileGamePiece?.MovePieceToTile(hoveredTile, OnMoveComplete);
            hoveredTile.TileGamePiece?.MovePieceToTile(clickedTile, OnMoveComplete);

            void OnMoveComplete()
            {
                movedCount++;
                if (movedCount >= 2)
                {
                    SwapTilesLogicalData(clickedTile, hoveredTile);

                    //After swapping tiles logical data - check if there are any possible matches which came up after swapping
                    if (AreThereAnyPossibleMatchesWithClickedAndHoveredTilesSwapped())
                    {
                        SNEventsController<M3_InGameEvents>.TriggerEvent<(M3_Tile, M3_Tile)>(M3_InGameEvents.SWAP_COMPLETED, (clickedTile, hoveredTile));

                        ResetClickedAndHoveredTilesOnSettledState();
                    }
                    //If no? Then reset what you have swapped - swap back - both physically and logically
                    else
                    {
                        clickedTile.TileGamePiece?.MovePieceToTile(hoveredTile);
                        hoveredTile.TileGamePiece?.MovePieceToTile(clickedTile);

                        SwapTilesLogicalData(hoveredTile, clickedTile);

                        //TODO: San
                        ResetClickedAndHoveredTilesOnSettledState();
                    }
                }
            }
        }

        #region Private Section

        private void SwapTilesLogicalData(M3_Tile tile1, M3_Tile tile2)
        {
            //3 Things to keep in mind while swapping tiles
            // 1. Need to swap positions of the game pieces - visual thing
            // 2. Need to swap of the game pieces in the Board data - logical thing
            // 3. Need to update the tile reference in the game piece & vice versa - logical thing
            // 4. IMP: Tiles are FIXED!! they dont move - so you only have to update gamepice a tile is holding in tiles data nothing else!!

            if (tile1 == null || tile2 == null) return;

            // 2. Need to swap of the game pieces in the Board data - logical thing
            ServiceRegistry.Get<M3_Service_BoardData>().SwapInBoardGamePiecesData(tile1.TileGamePiece, tile2.TileGamePiece);

            //3. Need to update the tile reference in the game piece & vice versa - logical thing
            var tempGamePiece = tile1.TileGamePiece;

            tile1.SetTileGamePiece(tile2.TileGamePiece);
            tile2.SetTileGamePiece(tempGamePiece);

            tile1.TileGamePiece.SetTile(tile1);
            tile2.TileGamePiece.SetTile(tile2);
        }

        private bool IsClickedAndHoveredGamePicesNOTInMovingState()
        {
            return currentTileIsStatic && hoveredTileIsStatic && ServiceRegistry.Get<M3_GameInputService>().ConsumeInput;
        }

        private bool CanInitiateSwapBtwClickedAndHoveredTile()
        {
            return hoveredTile != clickedTile
                && clickedTile.NeighbourTiles.Contains(hoveredTile)
                && hoveredTile.TileGamePiece != null;
        }

        private bool AreThereAnyPossibleMatchesWithClickedAndHoveredTilesSwapped()
        {
            var possibleClickedTileMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(clickedTile);
            var possibleHoveredTileMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(hoveredTile);

            //DevTestingScript.Instance.HighlightCurrentMatches(possibleClickedTileMatches, possibleHoveredTileMatches);

            return possibleClickedTileMatches.Count > 0
                || possibleHoveredTileMatches.Count > 0;
        }

        private void ResetClickedAndHoveredTilesOnSettledState()
        {
            currentTileIsStatic = true;
            hoveredTileIsStatic = true;

            clickedTile = null;
            hoveredTile = null;
        }

        #endregion
    }
}
