using UnityEngine;
using SNGames.CommonModule;
using UnityEditor.U2D.Aseprite;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SNGames.M3
{
    public enum MatchDirection
    {
        Horizontal,
        Vertical
    }

    public class M3_Service_BoardMatcher : BaseService
    {
        public override void Init()
        {

        }

        public override void Deinit()
        {

        }

        private void OnSwapCompleted(object u)
        {

        }

        public List<M3_GamePiece> IdentifyPossibleMatches(M3_Tile tile)
        {
            return IdentifyPossibleMatches(tile.BoardPosition.x, tile.BoardPosition.y);
        }

        public bool HavePossibleMatches(M3_Tile tile)
        {
            return IdentifyPossibleMatches(tile).Count > 0;
        }

        public bool HavePossibleMatches(int xPos, int yPos)
        {
            return IdentifyPossibleMatches(xPos, yPos).Count > 0;
        }

        public List<M3_GamePiece> IdentifyPossibleMatches(int xPos, int yPos)
        {
            var possibleHorizontalMatches = IdentifyPossibleHorizontalMatches(xPos, yPos);
            var possibleVerticalMatches = IdentifyPossibleVerticalMatches(xPos, yPos);

            // Use a HashSet to avoid duplicate entries
            HashSet<M3_GamePiece> possibleMatches = new HashSet<M3_GamePiece>(possibleHorizontalMatches);
            possibleMatches.UnionWith(possibleVerticalMatches);

            // Convert back to List if needed
            return possibleMatches.ToList();
        }

        public List<M3_GamePiece> IdentifyPossibleHorizontalMatches(int xPos, int yPos)
        {
            M3_GamePiece[,] gamePiecesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetGamePiecesOnBoard();
            M3_GamePiece currentGamePiece = gamePiecesOnBoard[xPos, yPos];
            if (currentGamePiece == null)
                return new List<M3_GamePiece>();

            List<M3_GamePiece> possibleHorizontalMatches = new List<M3_GamePiece> { currentGamePiece };

            var currentPieceType = currentGamePiece.GamePieceType;

            int width = gamePiecesOnBoard.GetLength(0);

            possibleHorizontalMatches.AddRange(CheckHorizontalMatches(currentPieceType, xPos, yPos, 1, gamePiecesOnBoard));  // Right
            possibleHorizontalMatches.AddRange(CheckHorizontalMatches(currentPieceType, xPos, yPos, -1, gamePiecesOnBoard)); // Left

            return (possibleHorizontalMatches.Count >= 3) ? possibleHorizontalMatches : new List<M3_GamePiece>();
        }

        public List<M3_GamePiece> CheckHorizontalMatches(GamePiceType currentPieceType, int startX, int yPos, int direction, M3_GamePiece[,] gamePiecesOnBoard)
        {
            int width = gamePiecesOnBoard.GetLength(0);

            List<M3_GamePiece> matches = new List<M3_GamePiece>();
            int currentX = startX + direction;

            while (currentX >= 0 && currentX < width)
            {
                M3_GamePiece nextGamePiece = gamePiecesOnBoard[currentX, yPos];
                if (nextGamePiece == null || nextGamePiece.GamePieceType != currentPieceType)
                    break;

                matches.Add(nextGamePiece);
                currentX += direction;
            }

            return matches;
        }

        public List<M3_GamePiece> IdentifyPossibleVerticalMatches(int xPos, int yPos)
        {
            M3_GamePiece[,] gamePiecesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetGamePiecesOnBoard();
            M3_GamePiece currentGamePiece = gamePiecesOnBoard[xPos, yPos];
            if (currentGamePiece == null)
                return new List<M3_GamePiece>();

            List<M3_GamePiece> possibleVerticalMatches = new List<M3_GamePiece> { currentGamePiece };
            var currentPieceType = currentGamePiece.GamePieceType;

            possibleVerticalMatches.AddRange(CheckVerticalMatches(currentPieceType, xPos, yPos, 1, gamePiecesOnBoard)); // Downward
            possibleVerticalMatches.AddRange(CheckVerticalMatches(currentPieceType, xPos, yPos, -1, gamePiecesOnBoard)); // Upward

            return (possibleVerticalMatches.Count >= 3) ? possibleVerticalMatches : new List<M3_GamePiece>();
        }

        public List<M3_GamePiece> CheckVerticalMatches(GamePiceType currentPieceType, int xPos, int startY, int direction, M3_GamePiece[,] gamePiecesOnBoard)
        {
            int height = gamePiecesOnBoard.GetLength(1);

            List<M3_GamePiece> matches = new List<M3_GamePiece>();
            int currentY = startY + direction;

            while (currentY >= 0 && currentY < height)
            {
                M3_GamePiece nextGamePiece = gamePiecesOnBoard[xPos, currentY];
                if (nextGamePiece == null || nextGamePiece.GamePieceType != currentPieceType)
                    break;

                matches.Add(nextGamePiece);
                currentY += direction;
            }

            return matches;
        }
    }
}