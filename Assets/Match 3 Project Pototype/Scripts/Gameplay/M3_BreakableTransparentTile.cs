using System.Linq.Expressions;
using SNGames.M3;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class M3_BreakableTransparentTile : M3_Tile
    {
        [SerializeField] private GameObject visualElement;

        [SerializeField] private int tileHealth;

        public override bool CanHoldNormalGamePiece()
        {
            return true;
        }

        public override void SetTileVisuals()
        {
            visualElement.SetActive(true);
        }

        public void SetTileHealth(int tileHealth)
        {
            this.tileHealth = tileHealth;
        }

        public override void DirectTileMatchImpact()
        {
            tileHealth -= 1;
            if(tileHealth <= 0)
            {
                visualElement.SetActive(false);
            }
        }
    }
}
