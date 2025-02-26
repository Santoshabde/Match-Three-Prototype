using System.Collections.Generic;
using System.Linq;
using SNGames.CommonModule;
using SNGames.M3;
using UnityEngine;

public class DevTestingScript : MonoBehaviour
{
    [SerializeField] private M3_BoardController boardController;
    [SerializeField] private int xToStartMatchFinding;
    [SerializeField] private int yToStartMatchFinding;

    [ContextMenu("SpawnBoard")]
    public void SpawnBoard()
    {
        boardController.InitialSpawnBoardTiles();
        boardController.InitialSpawnRandomGamePicesOnTheBoard();
    }

    [ContextMenu("Find Match")]
    public void FindMatch()
    {
        var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleHorizontalMatches(xToStartMatchFinding, yToStartMatchFinding);
        foreach (var item in result)
        {
            item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    [ContextMenu("Full Board HorizontalMatch")]
    public void FullBoardHorizontalMatch()
    {
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleHorizontalMatches(x, y);
                foreach (var item in result)
                {
                    item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.5f);
                }
            }
        }
    }

    [ContextMenu("Full Board Vertical Match")]
    public void FullBoardVerticalMatch()
    {
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                var result = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleVerticalMatches(x, y);
                foreach (var item in result)
                {
                    item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.5f);
                }
            }
        }
    }

    [ContextMenu("All Matches")]
    public void AllMatches()
    {
        for (int x = 0; x < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(0); x++)
        {
            for (int y = 0; y < ServiceRegistry.Get<M3_Service_BoardData>().GetTilesOnBoard().GetLength(1); y++)
            {
                var horizontalMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleHorizontalMatches(x, y);
                var verticalMatches = ServiceRegistry.Get<M3_Service_BoardMatcher>().IdentifyPossibleVerticalMatches(x, y);

                // Use HashSet to remove duplicates and get the union of both lists
                HashSet<M3_GamePiece> allMatches = new HashSet<M3_GamePiece>(horizontalMatches);
                allMatches.UnionWith(verticalMatches);

                // Convert back to a List if needed
                List<M3_GamePiece> finalMatches = allMatches.ToList();
                foreach (var item in finalMatches)
                {
                    item.CurrentGamePieceTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.5f);
                }
            }
        }
    }
}
