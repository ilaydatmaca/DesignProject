using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    public void CheckItems(Cell cellA, Cell cellB, GamePiece clickedPiece, GamePiece targetPiece, List<GamePiece> tileAPieces, List<GamePiece> tileBPieces)
    {
        Vector2 swipeDirection = new Vector2(cellB.xIndex - cellA.xIndex, cellB.yIndex - cellA.yIndex);

        board.clickedCellItem = CreateItem(cellA.xIndex, cellA.yIndex, swipeDirection, tileAPieces);
        board.targetCellItem = CreateItem(cellB.xIndex, cellB.yIndex, swipeDirection, tileBPieces);
        
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
        MatchValue matchValue = MatchValue.None;

        if (gamePieces != null)
        {
            matchValue = FindMatchValue(gamePieces);
        }

        if (gamePieces.Count == 4 && matchValue != MatchValue.None)
        {
            if(Mathf.Abs(swapDirection.x) > Math.Abs(swapDirection.y))
            {
                item = board.itemFactory.MakeItem(ItemType.RocketRow, x, y, matchValue);
            }
            else
            {
                item = board.itemFactory.MakeItem(ItemType.RocketColumn, x, y, matchValue);
            }
        }
        else if (gamePieces.Count == 5 && matchValue != MatchValue.None)
        {
            item = board.itemFactory.MakeItem(ItemType.Bomb,x, y,  matchValue);
        }
        
        
        if (gamePieces.Count >= 6)
        {
            item = board.itemFactory.MakeItem(ItemType.Disco, x, y);
        }
        return item;
    }


    public List<GamePiece> ProcessDisco(GamePiece clickedPiece, GamePiece targetPiece)
    {
        List<GamePiece> colorMatches = new List<GamePiece>();

        if (clickedPiece.IsDisco() && targetPiece.IsDisco())
        {
            foreach (GamePiece piece in board.AllGamePieces)
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
    public List<GamePiece> FindAllMatchValue(MatchValue mValue)
    {
        if (board == null)
            return null;

        List<GamePiece> foundPieces = new List<GamePiece>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.AllGamePieces[i, j] != null)
                {
                    if (board.AllGamePieces[i, j].matchValue == mValue)
                    {
                        foundPieces.Add(board.AllGamePieces[i, j]);
                    }
                }
            }
        }
        return foundPieces;
    }
    
    
    
    
    

    // puts the bomb into the game Board and treats it as a normal GamePiece
    public void InitBomb(GameObject bomb)
    {
        int x = (int)bomb.transform.position.x;
        int y = (int)bomb.transform.position.y;


        if (board.IsInBorder(x, y))
        {
            board.AllGamePieces[x, y] = bomb.GetComponent<GamePiece>();
        }
    }

    // initializes any bombs created on the clicked or target tile
    public void InitAllBombs()
    {
        if (board.clickedCellItem != null)
        {
            InitBomb(board.clickedCellItem);
            board.clickedCellItem = null;
        }

        if (board.targetCellItem != null)
        {
            InitBomb(board.targetCellItem);
            board.targetCellItem = null;
        }
    }

}
