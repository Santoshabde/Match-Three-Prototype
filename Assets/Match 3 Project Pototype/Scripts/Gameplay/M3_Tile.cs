using System;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.M3
{
    public class M3_Tile : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardPosition;
        [SerializeField] private M3_GamePiece tileGamePiece;
        [SerializeField] private List<M3_Tile> neighbourTiles;

        public M3_GamePiece TileGamePiece => tileGamePiece;
        public List<M3_Tile> NeighbourTiles => neighbourTiles;
        public Vector2Int BoardPosition => boardPosition;

        public Action<M3_Tile> OnTileClicked;
        public Action<M3_Tile> OnTileHovered;

        public void Init(int xPos, int yPos, M3_GamePiece gamePiece)
        {
            this.name = $"Tile ({xPos}, {yPos})";

            this.boardPosition = new Vector2Int(xPos, yPos);
            this.tileGamePiece = gamePiece;
        }

        public void Init(int xPos, int yPos)
        {
            this.name = $"Tile ({xPos}, {yPos})";
            this.boardPosition = new Vector2Int(xPos, yPos);
        }

        public void SetTileGamePiece(M3_GamePiece gamePiece)
        {
            tileGamePiece = gamePiece;
        }

        public void SetNeighbourTiles(List<M3_Tile> neighbourTiles)
        {
            this.neighbourTiles = neighbourTiles;
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