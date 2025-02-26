using UnityEngine;

namespace SNGames.M3
{
    public class M3_Tile : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardPosition;
        [SerializeField] private M3_GamePiece tileGamePiece;

        public void Init(int xPos, int yPos, M3_GamePiece gamePiece)
        {
            this.name = $"Tile ({xPos}, {yPos})";

            this.boardPosition = new Vector2Int(xPos, yPos);
            this.tileGamePiece = gamePiece;
        }
    }
}