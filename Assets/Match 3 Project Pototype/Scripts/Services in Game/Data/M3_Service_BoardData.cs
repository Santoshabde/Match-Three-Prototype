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

        public void SwapInBoardTilesData(M3_Tile tile1, M3_Tile tile2)
        {
            Vector2Int tile1BoardPosition = tile1.BoardPosition;
            Vector2Int tile2BoardPosition = tile2.BoardPosition;

            tilesOnBoard[tile1BoardPosition.x, tile1BoardPosition.y] = tile2;
            tilesOnBoard[tile2BoardPosition.x, tile2BoardPosition.y] = tile1;

            tile1.Init(tile2BoardPosition.x, tile2BoardPosition.y);
            tile2.Init(tile1BoardPosition.x, tile1BoardPosition.y);
        }


        public void SwapInBoardGamePiecesData(M3_GamePiece gamePiece1, M3_GamePiece gamePiece2)
        {
            Vector2Int gamePiece1BoardPosition = gamePiece1.BoardPosition;
            Vector2Int gamePiece2BoardPosition = gamePiece2.BoardPosition;

            gamePiecesOnBoard[gamePiece1BoardPosition.x, gamePiece1BoardPosition.y] = gamePiece2;
            gamePiecesOnBoard[gamePiece2BoardPosition.x, gamePiece2BoardPosition.y] = gamePiece1;

            gamePiece1.Init(gamePiece2BoardPosition.x, gamePiece2BoardPosition.y);
            gamePiece2.Init(gamePiece1BoardPosition.x, gamePiece1BoardPosition.y);
        }
    }
}