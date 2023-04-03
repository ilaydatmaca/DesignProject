using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeadlockManager : MonoBehaviour
{
    private Board _board;
    private Directions _directions;

    private int _matchLength = 3;
    private void Awake()
    {
        _board = GetComponent<Board>();
        _directions = GetComponent<Directions>();

    }
    
    /*List<GamePiece> GetRowOrColumnList(int x, int y, bool checkRow = true)
    {
        List<GamePiece> piecesList = new List<GamePiece>();

        for (int i = 0; i < _matchLength; i++)
        {
            if (checkRow)
            {
                if (_board.IsInBorder(x + i, y))
                {
                    if (_board.AllGamePieces[x + i, y] != null)
                    {
                        piecesList.Add(_board.AllGamePieces[x + i, y]);

                    }
                }
            }
            else
            {
                if (_board.IsInBorder(x, y + i))
                {
                    if (_board.AllGamePieces[x, y + i] != null)
                    {
                        piecesList.Add(_board.AllGamePieces[x, y + i]);
                    }
                }
            }
        }
        return piecesList;
    }

    List<GamePiece> GetMinimumMatches(List<GamePiece> gamePieces)
    {
        List<GamePiece> matches = new List<GamePiece>();
        
        var groups = gamePieces.GroupBy(n => n.matchValue);

        foreach (var grp in groups)
        {
            if (grp.Count() >= _matchLength - 1  && grp.Key != MatchValue.None)
            {
                matches = grp.ToList();
            }
        }
        return matches;
    }

    List<GamePiece> GetNeighbors(int x, int y)
    {
        HashSet<GamePiece> neighbors = new HashSet<GamePiece>();

        List<Vector2> searchDirections = _directions.GetVectorsFromList(
            new List<Direction>(){ Direction.Up, Direction.Down, Direction.Left, Direction.Right });

        foreach (Vector2 dir in searchDirections)
        {
            if (_board.IsInBorder(x + (int)dir.x, y + (int)dir.y))
            {
                if (_board.AllGamePieces[x + (int)dir.x, y + (int)dir.y] != null)
                {
                    neighbors.Add(_board.AllGamePieces[x + (int)dir.x, y + (int)dir.y]);
                }
            }
        }
        return neighbors.ToList();
    }


    bool HasMoveAt(int x, int y, bool checkRow = true)
    {
        List<GamePiece> pieces = GetRowOrColumnList(x, y, checkRow);

        List<GamePiece> matches = GetMinimumMatches(pieces);

        GamePiece unmatchedPiece = null;
        if (pieces != null && matches != null)
        {
            if (pieces.Count == _matchLength && matches.Count == _matchLength - 1)
            {
                unmatchedPiece = pieces.Except(matches).FirstOrDefault();
            }

            if (unmatchedPiece != null)
            {
                List<GamePiece> neighbors = GetNeighbors(unmatchedPiece.xIndex, unmatchedPiece.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.matchValue == matches[0].matchValue);
                matches = matches.Union(neighbors).ToList();
            }

            if (matches.Count >= _matchLength)
            {
                return true;
            }
        }

        // otherwise, return false
        return false;
    }


    public bool IsDeadlocked()
    {
        int width = _board.AllGamePieces.GetLength(0);
        int height = _board.AllGamePieces.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (HasMoveAt(i, j, true) || 
                    HasMoveAt(i, j, false))
                {
                    return false;

                }
            }
        }
        return true;
    }*/
    
     List<GamePiece> GetRowOrColumnList(GamePiece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true)
    {

        List<GamePiece> piecesList = new List<GamePiece>();

        for (int i = 0; i < listLength; i++)
        {
            if (checkRow)
            {
                if (_board.IsInBorder(x + i, y) && allPieces[x + i, y] != null)
                {
                    piecesList.Add(allPieces[x + i, y]);
                }
            }
            else
            {
                if (_board.IsInBorder(x, y + i) && allPieces[x, y + i] != null)
                {
                    piecesList.Add(allPieces[x, y + i]);
                }
            }
        }
        return piecesList;
    }

    List<GamePiece> GetMinimumMatches(List<GamePiece> gamePieces, int minForMatch = 2)
    {
        List<GamePiece> matches = new List<GamePiece>();

        var groups = gamePieces.GroupBy(n => n.matchValue);

        foreach (var grp in groups)
        {
            if (grp.Count() >= minForMatch && grp.Key != MatchValue.None)
            {
                matches = grp.ToList();
            }
        }
        return matches;
    }

    List<GamePiece> GetNeighbors(GamePiece[,] allPieces, int x, int y)
    {

        List<GamePiece> neighbors = new List<GamePiece>();

        Vector2[] searchDirections = new Vector2[4]
        {
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1)
        };

        foreach (Vector2 dir in searchDirections)
        {
            if (_board.IsInBorder(x + (int)dir.x, y + (int)dir.y))
            {
                if (allPieces[x + (int)dir.x, y + (int)dir.y] != null)
                {
                    if (!neighbors.Contains(allPieces[x + (int)dir.x, y + (int)dir.y]))
                    {
                        neighbors.Add(allPieces[x + (int)dir.x, y + (int)dir.y]);
                    }
                }
            }
        }
        return neighbors;
    }


    bool HasMoveAt(GamePiece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true)
    {
        List<GamePiece> pieces = GetRowOrColumnList(allPieces, x, y, listLength, checkRow);

        List<GamePiece> matches = GetMinimumMatches(pieces, listLength - 1);

        GamePiece unmatchedPiece = null;
        if (pieces != null && matches != null)
        {
            if (pieces.Count == listLength && matches.Count == listLength - 1)
            {
                unmatchedPiece = pieces.Except(matches).FirstOrDefault();
            }

            if (unmatchedPiece != null)
            {
                List<GamePiece> neighbors = GetNeighbors(allPieces, unmatchedPiece.xIndex, unmatchedPiece.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.matchValue == matches[0].matchValue);
                matches = matches.Union(neighbors).ToList();
            }
            
            if (matches.Count >= listLength)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsDeadlocked()
    {
        int listLength = 3;
        int width = _board.AllGamePieces.GetLength(0);
        int height = _board.AllGamePieces.GetLength(1);

        bool isDeadlocked = true;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (HasMoveAt(_board.AllGamePieces, i, j, listLength, true) || HasMoveAt(_board.AllGamePieces, i, j, listLength, false))
                {
                    isDeadlocked = false;

                }
            }
        }
        return isDeadlocked;
    }
}
