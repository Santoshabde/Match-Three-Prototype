using SNGames.CommonModule;
using UnityEngine;

namespace SNGames.M3
{
    public class M3_Service_BoardData : BaseService
    {
        private M3_Tile[,] tilesOnBoard;
        private M3_GamePiece[,] gamePiecesOnBoard;

        public override void Init()
        {

        }

        public override void Deinit()
        {

        }

        // --- Tiles Data on Board ---
        public void SetTilesOnBoard(M3_Tile[,] tilesOnBoard)
        {
            this.tilesOnBoard = tilesOnBoard;
        }

        public M3_Tile[,] GetTilesOnBoard() => tilesOnBoard;

        
        // --- GamePieces Data on Board ---
        public void SetGamePiecesOnBoard(M3_GamePiece[,] gamePiecesOnBoard)
        {
            this.gamePiecesOnBoard = gamePiecesOnBoard;
        }

        public M3_GamePiece[,] GetGamePiecesOnBoard() => gamePiecesOnBoard;
    }
}