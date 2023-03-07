using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board _board;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }
    
    List<GamePiece> FindMatches(int startX, int startY, Direction direction, int minLength = 3)
    {
        List<GamePiece> matches = new List<GamePiece>();

        GamePiece startPiece = null;

        if (_board.IsWithinBounds(startX, startY))
        {
            startPiece = _board.m_allGamePieces[startX, startY];
        }

        if (startPiece != null)
        {
            matches.Add(startPiece);
        }
        else
        {
            return null;
        }
        
        int maxValue = (_board.width > _board.height) ? _board.width : _board.height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            int nextX = startX + (int)GetVectorFromDirection(direction).x * i;
            int nextY = startY + (int)GetVectorFromDirection(direction).y * i;

            if (!_board.IsWithinBounds(nextX, nextY))
            {
                break;
            }

            GamePiece nextPiece = _board.m_allGamePieces[nextX, nextY];

            if (nextPiece == null)
            {
                break;
            }

            if (nextPiece.matchValue == startPiece.matchValue && nextPiece.matchValue != MatchType.None &&
                !matches.Contains(nextPiece))
            {
                matches.Add(nextPiece);
            }
            else
            {
                break;
            }
        }

        if (matches.Count >= minLength)
        {
            return matches.ToList();
        }

        return new List<GamePiece>();
    }

    public Vector2 GetVectorFromDirection(Direction direction)
    {
        Vector2 directionVector = Vector2.zero;
        switch (direction)
        {
            case Direction.Up:
                directionVector = new Vector2(0, 1);
                break;
            case Direction.Right:
                directionVector = new Vector2(1, 0);
                break;
            case Direction.Down:
                directionVector = new Vector2(0, -1);
                break;
            case Direction.Left:
                directionVector = new Vector2(-1, 0);
                break;
        }

        return directionVector;
    }

    List<GamePiece> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> upwardMatches = FindMatches(startX, startY, Direction.Up, 2);
        List<GamePiece> downwardMatches = FindMatches(startX, startY, Direction.Down, 2);

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    List<GamePiece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePiece> rightMatches = FindMatches(startX, startY, Direction.Right, 2);
        List<GamePiece> leftMatches = FindMatches(startX, startY, Direction.Left, 2);

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

    public List<GamePiece> FindMatchesAt(List<GamePiece> gamePieces, int minLength = 3)
    {
        List<GamePiece> matches = new List<GamePiece>();

        foreach (GamePiece piece in gamePieces)
        {
            matches = matches.Union(FindMatchesAt(piece.xIndex, piece.yIndex, minLength)).ToList();
        }

        return matches;
    }

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

        return combinedMatches;
    }

    public List<GamePiece> FindAllMatchValue(MatchType mValue)
    {
        List<GamePiece> foundPieces = new List<GamePiece>();

        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                if (_board.m_allGamePieces[i, j] != null)
                {
                    if (_board.m_allGamePieces[i, j].matchValue == mValue)
                    {
                        foundPieces.Add(_board.m_allGamePieces[i, j]);
                    }
                }
            }
        }

        return foundPieces;
    }
}