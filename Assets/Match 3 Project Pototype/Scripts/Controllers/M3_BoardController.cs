using System.Collections.Generic;
using SNGames.CommonModule;
using UnityEngine;

namespace SNGames.M3
{
    public enum GamePiceType
    {
        None,
        Blue,
        Green,
        Red,
        Yellow,
        Purple,
        Orange
    }

    public class M3_BoardController : MonoBehaviour
    {
        [Header("Required Componenets")]
        [SerializeField] private M3_Tile tilePrefab;
        [SerializeField] private List<M3_GamePiece> allGamePieces;
        [SerializeField] private M3_CameraController cameraController;

        [Space]
        [Header("Board Settings")]
        [SerializeField] private int width;
        [SerializeField] private int height;

        private M3_Tile[,] tilesOnBoard;
        private M3_GamePiece[,] gamePiecesOnBoard;

        #region  Public Region
        public void InitialSpawnBoardTiles()
        {
            ClearExistingTilesInGame();

            tilesOnBoard = new M3_Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    M3_Tile tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    tile.Init(x, y, null);
                    tilesOnBoard[x, y] = tile;
                }
            }

            SetCameraPositionAndFOVBasedOnBoardSize();

            ServiceRegistry.Get<M3_Service_BoardData>().SetTilesOnBoard(tilesOnBoard);
        }

        public void InitialSpawnRandomGamePicesOnTheBoard()
        {
            gamePiecesOnBoard = new M3_GamePiece[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    M3_GamePiece gamePiece = Instantiate(allGamePieces[Random.Range(0, allGamePieces.Count)], new Vector3(x, y, 0), Quaternion.identity, transform);
                    gamePiece.Init(x, y, tilesOnBoard[x, y]);
                    tilesOnBoard[x, y].Init(x, y, gamePiece);

                    gamePiecesOnBoard[x, y] = gamePiece;
                }
            }

            ServiceRegistry.Get<M3_Service_BoardData>().SetGamePiecesOnBoard(gamePiecesOnBoard);
        }

        #endregion

        #region  Private Region

        private void SetCameraPositionAndFOVBasedOnBoardSize()
        {
            cameraController.SetCameraPositionAndFOV(width, height);
        }

        private void ClearExistingTilesInGame()
        {
            if (tilesOnBoard == null)
            {
                return;
            }

            for (int x = 0; x < tilesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < tilesOnBoard.GetLength(1); y++)
                {
                    if (tilesOnBoard[x, y] != null)
                    {
                        Destroy(tilesOnBoard[x, y].gameObject);
                    }
                }
            }
        }

        #endregion
    }
}