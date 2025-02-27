using UnityEngine;
using SNGames.CommonModule;
using UnityEditor.U2D.Aseprite;
using System.Collections.Generic;
using System.Linq;

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
            GamePiceType currentGamePieceType = currentGamePiece.GamePieceType;
            int width = gamePiecesOnBoard.GetLength(0);

            // -- Possible horizontal Matches
            List<M3_GamePiece> possibleHorizontalMatches = new List<M3_GamePiece>();
            //-- Current piece is always part of match - so add this
            possibleHorizontalMatches.Add(currentGamePiece);

            int currentXIndex = xPos;
            int matchCount = 0;
            // -- Check Right --
            while (currentXIndex < width)
            {
                int nextXIndex = currentXIndex + 1;
                if (nextXIndex >= width)
                    break;

                M3_GamePiece nextGamePiece = gamePiecesOnBoard[nextXIndex, yPos];
                GamePiceType nextGamePieceType = nextGamePiece.GamePieceType;

                if (nextGamePieceType == currentGamePieceType)
                {
                    matchCount += 1;
                    currentXIndex += 1;

                    possibleHorizontalMatches.Add(nextGamePiece);
                }
                else
                    break;
            }

            // -- Check Left --
            currentXIndex = xPos;
            while (currentXIndex >= 0)
            {
                int nextXIndex = currentXIndex - 1;
                if (nextXIndex < 0)
                    break;

                M3_GamePiece nextGamePiece = gamePiecesOnBoard[nextXIndex, yPos];
                GamePiceType nextGamePieceType = nextGamePiece.GamePieceType;

                if (nextGamePieceType == currentGamePieceType)
                {
                    currentXIndex -= 1;
                    matchCount += 1;

                    possibleHorizontalMatches.Add(nextGamePiece);
                }
                else
                    break;
            }

            return (possibleHorizontalMatches.Count >= 3) ? possibleHorizontalMatches : new List<M3_GamePiece>();
        }

        public List<M3_GamePiece> IdentifyPossibleVerticalMatches(int xPos, int yPos)
        {
            M3_GamePiece[,] gamePiecesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetGamePiecesOnBoard();
            M3_GamePiece currentGamePiece = gamePiecesOnBoard[xPos, yPos];
            GamePiceType currentGamePieceType = currentGamePiece.GamePieceType;
            int height = gamePiecesOnBoard.GetLength(1); // Get board height

            // -- Possible Vertical Matches
            List<M3_GamePiece> possibleVerticalMatches = new List<M3_GamePiece>();
            possibleVerticalMatches.Add(currentGamePiece); // Current piece is always part of the match

            int currentYIndex = yPos;
            int matchCount = 0;

            // -- Check Down --
            while (currentYIndex < height)
            {
                int nextYIndex = currentYIndex + 1;
                if (nextYIndex >= height)
                    break;

                M3_GamePiece nextGamePiece = gamePiecesOnBoard[xPos, nextYIndex];
                GamePiceType nextGamePieceType = nextGamePiece.GamePieceType;

                if (nextGamePieceType == currentGamePieceType)
                {
                    matchCount += 1;
                    currentYIndex += 1;
                    possibleVerticalMatches.Add(nextGamePiece);
                }
                else
                    break;
            }

            // -- Check Up --
            currentYIndex = yPos;
            while (currentYIndex >= 0)
            {
                int nextYIndex = currentYIndex - 1;
                if (nextYIndex < 0)
                    break;

                M3_GamePiece nextGamePiece = gamePiecesOnBoard[xPos, nextYIndex];
                GamePiceType nextGamePieceType = nextGamePiece.GamePieceType;

                if (nextGamePieceType == currentGamePieceType)
                {
                    currentYIndex -= 1;
                    matchCount += 1;
                    possibleVerticalMatches.Add(nextGamePiece);
                }
                else
                    break;
            }

            return (possibleVerticalMatches.Count >= 3) ? possibleVerticalMatches : new List<M3_GamePiece>();
        }
    }
}