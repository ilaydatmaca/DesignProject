using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private Board _board;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    
    List<GamePiece> FindMatches(int startX, int startY, Vector2 searchDirection)
    {
        HashSet<GamePiece> matches = new HashSet<GamePiece>();

        GamePiece startPiece = null;
        
        if (_board.IsInBorder(startX, startY))
        {
            startPiece = _board.AllGamePieces[startX, startY];
            
            if (startPiece != null)
            {
                matches.Add(startPiece);
            }
            else
            {
                return new List<GamePiece>();
            }
        }


        int maxValue = (_board.width > _board.height) ? _board.width : _board.height;

        for (int i = 1; i < maxValue; i++)
        {
            int nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            int nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!_board.IsInBorder(nextX, nextY))
            {
                break;
            }

            GamePiece nextPiece = _board.AllGamePieces[nextX, nextY];

            if (nextPiece == null)
            {
                break;
            }

            if (nextPiece.matchValue == startPiece.matchValue)
            {
                matches.Add(nextPiece);
            }
            else
            {
                break;
            }
        }
        return matches.ToList();

    }

    List<GamePiece> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1));
        List<GamePiece> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1));

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();
        
        Debug.Log("vertical " + combinedMatches.Count);

        return (combinedMatches.Count >= minLength) ? combinedMatches : new List<GamePiece>();

    }

    List<GamePiece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> rightMatches = FindMatches(startX, startY, new Vector2(1, 0));
        List<GamePiece> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0));

        var combinedMatches = rightMatches.Union(leftMatches).ToList();

        Debug.Log("horizontaş " + combinedMatches.Count);

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

        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                var matches = FindMatchesAt(i, j);
                combinedMatches = combinedMatches.Union(matches).ToList();
            }
        }
        
        Debug.Log(combinedMatches.Count());
        return combinedMatches;
    }




}
