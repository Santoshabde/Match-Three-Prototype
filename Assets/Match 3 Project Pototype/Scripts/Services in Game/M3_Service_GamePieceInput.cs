using DG.Tweening;
using SNGames.CommonModule;
using UnityEngine;

namespace SNGames.M3
{
    public class M3_Service_GamePieceInput : BaseService
    {
        private bool currentTileMoved = true;
        private bool hoveredTileMoved = true;

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
            if (currentTileMoved && hoveredTileMoved)
            {
                clickedTile = tile;
            }
        }

        public void OnTileHovered(M3_Tile tile)
        {
            if (currentTileMoved && hoveredTileMoved)
            {
                if (clickedTile != null)
                {
                    hoveredTile = tile;

                    if (hoveredTile != clickedTile
                     && clickedTile.NeighbourTiles.Contains(hoveredTile))
                    {
                        int movedCount = 0;
                        hoveredTileMoved = false;
                        currentTileMoved = false;

                        clickedTile.TileGamePiece?.MovePieceToTile(hoveredTile, OnMoveComplete);
                        hoveredTile.TileGamePiece?.MovePieceToTile(clickedTile, OnMoveComplete);

                        void OnMoveComplete()
                        {
                            movedCount++;
                            // Ensure both moves are done before swapping
                            if (movedCount >= 2)
                            {
                                SwapTiles();
                            }
                        }
                    }
                }
            }
        }

        private void SwapTiles()
        {
            if (clickedTile == null || hoveredTile == null) return;

            ServiceRegistry.Get<M3_Service_BoardData>().SwapInBoardTilesData(clickedTile, hoveredTile);
            ServiceRegistry.Get<M3_Service_BoardData>().SwapInBoardGamePiecesData(clickedTile.TileGamePiece, hoveredTile.TileGamePiece);

            var tempGamePiece = clickedTile.TileGamePiece;

            clickedTile.SetTileGamePiece(hoveredTile.TileGamePiece);
            hoveredTile.SetTileGamePiece(tempGamePiece);

            clickedTile.TileGamePiece?.SetTile(clickedTile);
            hoveredTile.TileGamePiece?.SetTile(hoveredTile);

            currentTileMoved = true;
            hoveredTileMoved = true;

            clickedTile = null;
            hoveredTile = null;
        }
    }
}
