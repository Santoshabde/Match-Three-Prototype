using System.Linq.Expressions;
using SNGames.M3;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class M3_BreakableTransparentTile : M3_Tile
    {
        [SerializeField] private GameObject visualElement;
        public override bool CanHoldNormalGamePiece()
        {
            return true;
        }

        public override void SetTileVisuals()
        {
            visualElement.SetActive(true);
        }
    }
}
