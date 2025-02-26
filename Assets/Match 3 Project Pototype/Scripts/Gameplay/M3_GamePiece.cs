using System;
using DG.Tweening;
using UnityEngine;

namespace SNGames.M3
{
    public class M3_GamePiece : MonoBehaviour
    {
        [SerializeField] private GamePiceType gamePieceType;
        [SerializeField] private Vector2Int boardPosition;

        private M3_Tile currentGamePieceTile;

        public GamePiceType GamePieceType => gamePieceType;
        public M3_Tile CurrentGamePieceTile => currentGamePieceTile;

        public void Init(int xPos, int yPos, M3_Tile tile)
        {
            this.name = $"Game Piece ({xPos}, {yPos})";
            this.boardPosition = new Vector2Int(xPos, yPos);
            this.currentGamePieceTile = tile;
        }

        public void SetTile(M3_Tile tile)
        {
            this.currentGamePieceTile = tile;
        }

        public void MovePieceToTile(M3_Tile newTile, Action OnCompleted = null)
        {
            // Move the piece to the new tile
            transform.DOMove(newTile.transform.position, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                OnCompleted?.Invoke();
            });
        }
    }
}
