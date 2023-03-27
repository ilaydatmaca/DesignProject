using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Board))]
public class MatchFinder : MonoBehaviour
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }


    // general method to find matches, defaulting to a minimum of three-in-a-row, passing in an (x,y) position and direction
    List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        HashSet<GamePiece> matches = new HashSet<GamePiece>();

        GamePiece startPiece = null;

        if (board.IsInBorder(startX, startY))
        {
            startPiece = board.AllGamePieces[startX, startY];
        }

        if (startPiece != null)
        {
            matches.Add(startPiece);
        }
        else
        {
            return new List<GamePiece>();
        }


        int maxValue = (board.width > board.height) ? board.width : board.height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            int nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            int nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!board.IsInBorder(nextX, nextY))
            {
                break;
            }
            // find the adjacent GamePiece and check its MatchValue...
            GamePiece nextPiece = board.AllGamePieces[nextX, nextY];

            if (nextPiece == null)
            {
                break;
            }

            // ... if it matches then add it our running list of GamePieces
            if (nextPiece.matchValue == startPiece.matchValue && nextPiece.matchValue != MatchValue.None)
            {
                matches.Add(nextPiece);
            }
            else
            {
                break;
            }
        }
        
        // if our list is greater than our minimum (usually 3), then return the list...
        if (matches.Count >= minLength)
        {
            return matches.ToList();
        }
        return new List<GamePiece>();

    }

    List<GamePiece> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<GamePiece> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : new List<GamePiece>();

    }

    List<GamePiece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<GamePiece> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);

        var combinedMatches = rightMatches.Union(leftMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : new List<GamePiece>();

    }
    public List<GamePiece> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<GamePiece> horizMatches = FindHorizontalMatches(x, y, minLength);
        List<GamePiece> vertMatches = FindVerticalMatches(x, y, minLength);
        
        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }
    
    // find all matches in the game Board
    public List<GamePiece> FindAllMatches()
    {
        List<GamePiece> combinedMatches = new List<GamePiece>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                var matches = FindMatchesAt(i, j);
                combinedMatches = combinedMatches.Union(matches).ToList();
            }
        }
        return combinedMatches;
    }




}
