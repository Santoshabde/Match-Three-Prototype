using UnityEngine;
using SNGames.CommonModule;
using System.Collections.Generic;

namespace SNGames.M3
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "InGameConfigs/LevelData")]
    public class LevelDataConfig : ScriptableObject
    {
        public LevelData levelsData;
    }

    [System.Serializable]
    public class LevelData
    {
        public int levelID;
        public string levelName;
        public int width;
        public int height;
        public List<LevelTileData> levelTilesData;
    }

    [System.Serializable]
    public class LevelTileData
    {
        public string tileType;
        public float[] boardPosition; // Use an array to match JSON structure
        public GamePiceType gamePiceType;

        public Vector2 GetBoardPosition()
        {
            return new Vector2(boardPosition[0], boardPosition[1]); // Convert array to Vector2
        }
    }
}
