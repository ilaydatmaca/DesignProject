using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Board _board;
    private ItemFactory _itemFactory;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _itemFactory = GetComponent<ItemFactory>();
    }

    public void CheckItems(Cell cellA, Cell cellB, List<GamePiece> tileAPieces, List<GamePiece> tileBPieces)
    {
        Vector2 swipeDirection = new Vector2(cellB.xIndex - cellA.xIndex, cellB.yIndex - cellA.yIndex);

        _board.clickedCellItem = CreateItem(cellA.xIndex, cellA.yIndex, swipeDirection, tileAPieces);
        _board.targetCellItem = CreateItem(cellB.xIndex, cellB.yIndex, swipeDirection, tileBPieces);
        
    }

    MatchValue FindMatchValue(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                return piece.matchValue;
            }
        }
        return MatchValue.None;
    }
    
    GameObject CreateItem(int x, int y, Vector2 swapDirection, List<GamePiece> gamePieces)
    {
        GameObject item = null;

        if (gamePieces != null)
        {
            MatchValue matchValue = FindMatchValue(gamePieces);
            
            if (gamePieces.Count == 4 && matchValue != MatchValue.None)
            {
                if (Mathf.Abs(swapDirection.x) > Math.Abs(swapDirection.y))
                {
                    item = _itemFactory.MakeItem(ItemType.RocketRow, x, y, matchValue);
                }
                else
                {
                    item = _itemFactory.MakeItem(ItemType.RocketColumn, x, y, matchValue);
                }
            }
            else if (gamePieces.Count == 5 && matchValue != MatchValue.None)
            {
                item = _itemFactory.MakeItem(ItemType.Bomb,x, y,  matchValue);
            }
        
            else if (gamePieces.Count >= 6)
            {
                item = _itemFactory.MakeItem(ItemType.Disco, x, y);
            }
        }

        return item;
    }


    public List<GamePiece> ProcessDisco(GamePiece clickedPiece, GamePiece targetPiece)
    {
        List<GamePiece> colorMatches = new List<GamePiece>();

        if (clickedPiece.IsDisco() && targetPiece.IsDisco())
        {
            foreach (GamePiece piece in _board.AllGamePieces)
            {
                if (!colorMatches.Contains(piece))
                {
                    colorMatches.Add(piece);
                }
            }
        }
        
        else if (clickedPiece.IsDisco() || targetPiece.IsDisco())
        {
            if (clickedPiece.IsDisco())
            {
                clickedPiece.matchValue = targetPiece.matchValue;
                colorMatches = FindAllMatchValue(targetPiece.matchValue);
            }
            else if (targetPiece.IsDisco())
            {
                targetPiece.matchValue = clickedPiece.matchValue;
                colorMatches = FindAllMatchValue(clickedPiece.matchValue);
            }
        }
        
        return colorMatches;
    }
    
    // find all GamePieces on the Board with a certain MatchValue
    List<GamePiece> FindAllMatchValue(MatchValue mValue)
    {
        if (_board == null)
            return null;

        List<GamePiece> foundPieces = new List<GamePiece>();

        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                if (_board.AllGamePieces[i, j] != null)
                {
                    if (_board.AllGamePieces[i, j].matchValue == mValue)
                    {
                        foundPieces.Add(_board.AllGamePieces[i, j]);
                    }
                }
            }
        }
        return foundPieces;
    }
    

    // puts the bomb into the game Board and treats it as a normal GamePiece
    public void InitItem(GameObject item)
    {
        int x = item.GetComponent<GamePiece>().xIndex;
        int y = item.GetComponent<GamePiece>().yIndex;
        
        if (_board.IsInBorder(x, y))
        {
            _board.AllGamePieces[x, y] = item.GetComponent<GamePiece>();
        }
    }

    // initializes any items created on the clicked or target tile
    public void InitAllItems()
    {
        if (_board.clickedCellItem != null)
        {
            InitItem(_board.clickedCellItem);
            _board.clickedCellItem = null;
        }

        if (_board.targetCellItem != null)
        {
            InitItem(_board.targetCellItem);
            _board.targetCellItem = null;
        }
    }

}
