using UnityEngine;

namespace SNGames.M3
{
    public class M3_NormalTile : M3_Tile
    {
        public override bool CanHoldNormalGamePiece()
        {
            return true;
        }

        public override void DirectTileMatchImpact()
        {
            
        }

        public override void SetTileVisuals()
        {
            //Nothing as of now
        }
    }
}
