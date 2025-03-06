using UnityEngine;

namespace SNGames.M3
{
    public class M3_BlockedTile : M3_Tile
    {
        public override bool CanHoldNormalGamePiece()
        {
            return false;
        }

        public override void SetTileVisuals()
        {
            tileSpriteRenderer.color = new Color(tileSpriteRenderer.color.r, tileSpriteRenderer.color.g, tileSpriteRenderer.color.b, 0f);
        }
    }
}
