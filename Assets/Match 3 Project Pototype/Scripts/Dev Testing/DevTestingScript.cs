using System.Collections.Generic;
using System.Linq;
using SNGames.CommonModule;
using SNGames.M3;
using UnityEngine;

public class DevTestingScript : SerializeSingleton<DevTestingScript>
{
    [SerializeField] private M3_BoardController boardController;
    [SerializeField] private string levelJson;
    [SerializeField] private int xToStartMatchFinding;
    [SerializeField] private int yToStartMatchFinding;

    void Start()
    {
        //SpawnBoard();
        SpawnBoardWithJsonLevel();
    }

    [ContextMenu("SpawnRandomBoard")]
    public void SpawnBoard()
    {
        boardController.SpawnBoard_Randomly(8,8);     
    }

   [ContextMenu("SpawnBoardFromLevelJson")]
    public void SpawnBoardWithJsonLevel()
    {
        boardController.SpawnBoard_FromLevelJson(levelJson);
    }

    [ContextMenu("Find Match")]
    public void FindMatch()
    {
        ResetTilesColor();
        var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleHorizontalMatches(xToStartMatchFinding, yToStartMatchFinding);
        foreach (var item in result)
        {
            item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    [ContextMenu("Full Board HorizontalMatch")]
    public void FullBoardHorizontalMatch()
    {
        ResetTilesColor();
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleHorizontalMatches(x, y);
                foreach (var item in result)
                {
                    item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
                }
            }
        }
    }

    [ContextMenu("Full Board Vertical Match")]
    public void FullBoardVerticalMatch()
    {
        ResetTilesColor();
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleVerticalMatches(x, y);
                foreach (var item in result)
                {
                    item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f); //0.09411765
                }
            }
        }
    }

    [ContextMenu("All Matches")]
    public void AllMatches()
    {
        ResetTilesColor();
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleMatches(x, y);
                foreach (var item in result)
                {
                    item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
                }
            }
        }
    }

    private void ResetTilesColor()
    {
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard()[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.09411765f);
            }
        }
    }

    public void HighlightCurrentMatches(List<M3_GamePiece> matches1, List<M3_GamePiece> matches2)
    {
        ResetTilesColor();

        foreach (var item in matches1)
        {
            item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        }

        foreach (var item in matches2)
        {
            item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        }
    }
}
