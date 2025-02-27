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
                                SwapTiles(clickedTile, hoveredTile);

                                var possibleClickedTileMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(clickedTile);
                                var possibleHoveredTileMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(hoveredTile);

                                DevTestingScript.Instance.HighlightCurrentMatches(possibleClickedTileMatches, possibleHoveredTileMatches);

                                if (possibleClickedTileMatches.Count > 0
                                || possibleHoveredTileMatches.Count > 0)
                                {
                                    currentTileMoved = true;
                                    hoveredTileMoved = true;

                                    clickedTile = null;
                                    hoveredTile = null;
                                }
                                else
                                {
                                    clickedTile.TileGamePiece?.MovePieceToTile(hoveredTile);
                                    hoveredTile.TileGamePiece?.MovePieceToTile(clickedTile);

                                    SwapTiles(hoveredTile, clickedTile);

                                    currentTileMoved = true;
                                    hoveredTileMoved = true;

                                    clickedTile = null;
                                    hoveredTile = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SwapTiles(M3_Tile tile1, M3_Tile tile2)
        {
            if (tile1 == null || tile2 == null) return;

            ServiceRegistry.Get<M3_Service_BoardData>().SwapInBoardGamePiecesData(tile1.TileGamePiece, tile2.TileGamePiece);

            var tempGamePiece = tile1.TileGamePiece;

            tile1.SetTileGamePiece(tile2.TileGamePiece);
            tile2.SetTileGamePiece(tempGamePiece);

            tile1.TileGamePiece.SetTile(tile1);
            tile2.TileGamePiece.SetTile(tile2);
        }

        private bool IsClickedAndHoveredGamePicesInMovingState()
        {
            return currentTileMoved && hoveredTileMoved;
        }
    }
}
