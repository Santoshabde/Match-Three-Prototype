using System;
using UnityEngine;

namespace SNGames.M3
{
    public class M3_Tile : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardPosition;
        [SerializeField] private M3_GamePiece tileGamePiece;

        public M3_GamePiece TileGamePiece => tileGamePiece;

        public Action<M3_Tile> OnTileClicked;
        public Action<M3_Tile> OnTileHovered;

        public void Init(int xPos, int yPos, M3_GamePiece gamePiece)
        {
            this.name = $"Tile ({xPos}, {yPos})";

            this.boardPosition = new Vector2Int(xPos, yPos);
            this.tileGamePiece = gamePiece;
        }

        public void SetTileGamePiece(M3_GamePiece gamePiece)
        {
            tileGamePiece = gamePiece;
        }

        #region  Input Actions
        
        void OnMouseDown()
        {
            OnTileClicked?.Invoke(this);
        }

        void OnMouseEnter()
        {
            OnTileHovered?.Invoke(this);
        }

        #endregion
    }
}