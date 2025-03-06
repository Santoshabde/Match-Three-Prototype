using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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

    [System.Serializable]
    public class TilesTypesData
    {
        public string tileType;
        public M3_Tile tile;
    }

    public class M3_BoardController : MonoBehaviour
    {
        [Header("Required Componenets")]
        [SerializeField] private List<M3_GamePiece> allGamePieces;
        [SerializeField] private List<TilesTypesData> allTilesInGame;
        [SerializeField] private M3_CameraController cameraController;

        void Start()
        {
            SNEventsController<M3_InGameEvents>.RegisterEvent<object>(M3_InGameEvents.FILL_EMPTY_BOARD_SPOTS, FillEmptyBoardSpots);
        }

        void OnDestroy()
        {
            ClearExistingTilesInGame();
            ClearExistingGamePiecesInGame();

            SNEventsController<M3_InGameEvents>.DeregisterEvent<object>(M3_InGameEvents.FILL_EMPTY_BOARD_SPOTS, FillEmptyBoardSpots);
        }

        #region  Public Region
        public void SpawnBoard_FromLevelJson(string json)
        {
            // ----- Spawning Tiles -----
            ClearExistingTilesInGame();

            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(json);
            var tilesOnBoard = new M3_Tile[levelData.width, levelData.height];
            foreach (LevelTileData tileData in levelData.levelTilesData)
            {
                Vector2 pos = tileData.GetBoardPosition();
                int xPos = (int)pos.x;
                int yPos = (int)pos.y;

                // Spawn tile
                M3_Tile toSpawn = allTilesInGame.Find(t => t.tileType == tileData.tileType).tile;
                if (toSpawn != null)
                {
                    M3_Tile tile = Instantiate(toSpawn, new Vector3(xPos, yPos, 0), Quaternion.identity, transform);
                    tile.Init(xPos, yPos, null);
                    tile.SetTileVisuals();
                    tilesOnBoard[xPos, yPos] = tile;

                    tile.OnTileClicked += ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileClicked;
                    tile.OnTileHovered += ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileHovered;
                }
            }

            AssignTilesNeighbour(tilesOnBoard, levelData.width, levelData.height);
            SetCameraPositionAndFOVBasedOnBoardSize(levelData.width, levelData.height);

            ServiceRegistry.Get<M3_Service_BoardData>().SetTilesOnBoard(tilesOnBoard);

            // ----- Spawning GamePieces -----
            SpawnRandomGamePicesOnTheBoard_FromLevelJson(tilesOnBoard, levelData.levelTilesData);
        }

        public void SpawnBoard_Randomly(int width, int height)
        {
            // ----- Spawning Tiles -----
            ClearExistingTilesInGame();

            var tilesOnBoard = new M3_Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    M3_Tile toSpawn = allTilesInGame.Find(t => t.tileType == "Normal").tile;
                    M3_Tile tile = Instantiate(toSpawn, new Vector3(x, y, 0), Quaternion.identity, transform);
                    tile.Init(x, y, null);
                    tile.SetTileVisuals();
                    tilesOnBoard[x, y] = tile;

                    tile.OnTileClicked += ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileClicked;
                    tile.OnTileHovered += ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileHovered;
                }
            }

            AssignTilesNeighbour(tilesOnBoard, width, height);
            SetCameraPositionAndFOVBasedOnBoardSize(width, height);

            ServiceRegistry.Get<M3_Service_BoardData>().SetTilesOnBoard(tilesOnBoard);

            // ----- Spawning GamePieces -----
            SpawnRandomGamePicesOnTheBoard_Randomly(tilesOnBoard);
        }

        #endregion

        #region  Private Region

        private void SpawnRandomGamePicesOnTheBoard_FromLevelJson(M3_Tile[,] tilesOnBoard, List<LevelTileData> levelTilesData)
        {
            int width = tilesOnBoard.GetLength(0);
            int height = tilesOnBoard.GetLength(1);

            var gamePiecesOnBoard = new M3_GamePiece[width, height];
            foreach (LevelTileData tileData in levelTilesData)
            {
                Vector2 pos = tileData.GetBoardPosition();
                int xPos = (int)pos.x;
                int yPos = (int)pos.y;
                M3_GamePiece gamePiece = null;

                if (tilesOnBoard[xPos, yPos].CanHoldNormalGamePiece())
                {
                    M3_GamePiece gamePieceToSpawn = allGamePieces.Find(t => t.GamePieceType == tileData.gamePiceType);

                    gamePiece = Instantiate(gamePieceToSpawn, new Vector3(xPos, yPos, 0), Quaternion.identity, transform);
                    gamePiece.Init(xPos, yPos, tilesOnBoard[xPos, yPos]);

                }

                tilesOnBoard[xPos, yPos].Init(xPos, yPos, gamePiece);
                gamePiecesOnBoard[xPos, yPos] = gamePiece;
            }

            ServiceRegistry.Get<M3_Service_BoardData>().SetGamePiecesOnBoard(gamePiecesOnBoard);
        }

        private void SpawnRandomGamePicesOnTheBoard_Randomly(M3_Tile[,] tilesOnBoard)
        {
            ClearExistingGamePiecesInGame();

            int width = tilesOnBoard.GetLength(0);
            int height = tilesOnBoard.GetLength(1);

            var gamePiecesOnBoard = new M3_GamePiece[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    M3_GamePiece randomGamePiece = GetARandomNonMatchFormiongPiece(x, y, gamePiecesOnBoard);

                    M3_GamePiece gamePiece = Instantiate(randomGamePiece, new Vector3(x, y, 0), Quaternion.identity, transform);
                    gamePiece.Init(x, y, tilesOnBoard[x, y]);
                    tilesOnBoard[x, y].Init(x, y, gamePiece);

                    gamePiecesOnBoard[x, y] = gamePiece;
                }
            }

            ServiceRegistry.Get<M3_Service_BoardData>().SetGamePiecesOnBoard(gamePiecesOnBoard);
        }

        private void FillEmptyBoardSpots(object obj)
        {
            var gamePiecesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetGamePiecesOnBoard();
            var tilesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard();

            List<M3_Tile> newTilesWhereWeSpawnedRandoms = new List<M3_Tile>();

            for (int x = 0; x < gamePiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < gamePiecesOnBoard.GetLength(1); y++)
                {
                    if (gamePiecesOnBoard[x, y] == null
                    && tilesOnBoard[x, y].CanHoldNormalGamePiece())
                    {
                        Vector3 spawnOffset = new Vector3(0, 10, 0);

                        var randomTilePicked = allGamePieces[Random.Range(0, allGamePieces.Count)];

                        M3_GamePiece gamePiece = Instantiate(randomTilePicked, new Vector3(x, y, 0) + spawnOffset, Quaternion.identity, transform);
                        gamePiece.Init(x, y, tilesOnBoard[x, y]);
                        tilesOnBoard[x, y].Init(x, y, gamePiece);

                        gamePiecesOnBoard[x, y] = gamePiece;
                        newTilesWhereWeSpawnedRandoms.Add(tilesOnBoard[x, y]);

                        gamePiece.MovePieceToCurrentTile(0.25f);
                    }
                }
            }

            ServiceRegistry.Get<M3_Service_BoardData>().SetGamePiecesOnBoard(gamePiecesOnBoard);

            SNEventsController<M3_InGameEvents>.TriggerEvent<List<M3_Tile>>(M3_InGameEvents.EMPTY_BOARD_SPORTS_FILLED, newTilesWhereWeSpawnedRandoms);
        }

        private M3_GamePiece GetARandomNonMatchFormiongPiece(int x, int y, M3_GamePiece[,] gamePiecesOnBoard)
        {
            M3_GamePiece randomGamePiece;

            int maxTriesToFindNonMatchFormingPiece = 100;
            int currentTry = 0;
            while (true)
            {
                randomGamePiece = allGamePieces[Random.Range(0, allGamePieces.Count)];
                var randomGamePieceType = randomGamePiece.GamePieceType;

                var verticalMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().CheckVerticalMatches(randomGamePieceType, x, y, -1, gamePiecesOnBoard);
                var horizontalMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().CheckHorizontalMatches(randomGamePieceType, x, y, -1, gamePiecesOnBoard);

                if (verticalMatches.Count >= 2 || horizontalMatches.Count >= 2)
                {
                    currentTry += 1;
                    if (currentTry >= maxTriesToFindNonMatchFormingPiece)
                    {
                        Debug.LogError("Cannot find a non-match forming piece after " + maxTriesToFindNonMatchFormingPiece + " tries.");
                        break;
                    }

                    continue;
                }
                else
                {
                    break;
                }
            }

            return randomGamePiece;
        }

        private void AssignTilesNeighbour(M3_Tile[,] tilesOnBoard, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //-- Calculate neighbour for each tilesOnBoard[i,j] (4 directions)-- 
                    List<M3_Tile> neighbourTiles = new List<M3_Tile>();

                    if (i > 0)
                    {
                        neighbourTiles.Add(tilesOnBoard[i - 1, j]);
                    }

                    if (i < width - 1)
                    {
                        neighbourTiles.Add(tilesOnBoard[i + 1, j]);
                    }

                    if (j > 0)
                    {
                        neighbourTiles.Add(tilesOnBoard[i, j - 1]);
                    }

                    if (j < height - 1)
                    {
                        neighbourTiles.Add(tilesOnBoard[i, j + 1]);
                    }

                    tilesOnBoard[i, j].SetNeighbourTiles(neighbourTiles);
                }
            }
        }

        private void SetCameraPositionAndFOVBasedOnBoardSize(int width, int height)
        {
            cameraController.SetCameraPositionAndFOV(width, height);
        }

        private void ClearExistingTilesInGame()
        {
            var tilesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard();
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
                        tilesOnBoard[x, y].OnTileClicked -= ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileClicked;
                        tilesOnBoard[x, y].OnTileHovered -= ServiceRegistry.Get<M3_Service_GamePieceInput>().OnTileHovered;

                        DestroyImmediate(tilesOnBoard[x, y].gameObject);
                    }
                }
            }
        }

        private void ClearExistingGamePiecesInGame()
        {
            var gamePiecesOnBoard = ServiceRegistry.Get<M3_Service_BoardData>().GetGamePiecesOnBoard();
            if (gamePiecesOnBoard == null)
            {
                return;
            }

            for (int x = 0; x < gamePiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < gamePiecesOnBoard.GetLength(1); y++)
                {
                    if (gamePiecesOnBoard[x, y] != null)
                    {
                        DestroyImmediate(gamePiecesOnBoard[x, y].gameObject);
                    }
                }
            }
        }

        #endregion
    }
}