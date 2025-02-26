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

                    if (hoveredTile != clickedTile)
                    {
                        hoveredTileMoved = false;
                        currentTileMoved = false;

                        //Swapping
                        Sequence tileSwapSeq = DOTween.Sequence();
                        tileSwapSeq.AppendInterval(0.35f);
                        tileSwapSeq.OnComplete(() =>
                        {
                            var tempClickedTileGamePiece = clickedTile.TileGamePiece;

                            clickedTile.SetTileGamePiece(hoveredTile.TileGamePiece);
                            hoveredTile.SetTileGamePiece(tempClickedTileGamePiece);

                            clickedTile.TileGamePiece.SetTile(clickedTile);
                            hoveredTile.TileGamePiece.SetTile(hoveredTile);

                            currentTileMoved = true;
                            hoveredTileMoved = true;

                            clickedTile = null;
                            hoveredTile = null;
                        });

                        clickedTile.TileGamePiece.MovePieceToTile(hoveredTile, () =>
                        {

                        });
                        hoveredTile.TileGamePiece.MovePieceToTile(clickedTile, () =>
                        {

                        });
                    }
                }

                //clickedTile = null;
                //hoveredTile = null;
            }
        }
    }
}
