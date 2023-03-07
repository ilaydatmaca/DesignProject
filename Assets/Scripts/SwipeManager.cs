using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public Cell clickedCell;
    public Cell targetCell;

    private Board _board;
    private MatchFinder _matchFinder;
    private FallAndFillManager _fallAndFillManager;
    
    public GameObject clickedCellItem;
    public GameObject targetCellItem;

    public bool playerInputEnabled = true;

    public float swapTime = 0.5f;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _matchFinder = GetComponent<MatchFinder>();
        _fallAndFillManager = GetComponent<FallAndFillManager>();
    }

    public void ClickCell(Cell cell)
    {
        if (clickedCell == null)
        {
            clickedCell = cell;
        }
    }

    public void DragToCell(Cell cell)
    {
        if (clickedCell != null && _board.IsNextTo(cell, clickedCell))
        {
            targetCell = cell;
        }
    }

    public void ReleaseCell()
    {
        if (clickedCell != null && targetCell != null)
        {
            SwitchCells(clickedCell, targetCell);
        }

        clickedCell = null;
        targetCell = null;
    }
    
    void SwitchCells(Cell clickedCell1, Cell targetCell1)
    {
        StartCoroutine(SwitchCellsRoutine(clickedCell1, targetCell1));
    }
    
    IEnumerator SwitchCellsRoutine(Cell clickedCell1, Cell targetCell1)
    {
        if (playerInputEnabled)
        {
            GamePiece clickedPiece = _board.m_allGamePieces[clickedCell1.xIndex, clickedCell1.yIndex];
            GamePiece targetPiece = _board.m_allGamePieces[targetCell1.xIndex, targetCell1.yIndex];

            if (targetPiece != null && clickedPiece != null)
            {
                clickedPiece.Move(targetCell.xIndex, targetCell.yIndex, swapTime);
                targetPiece.Move(clickedCell.xIndex, clickedCell.yIndex, swapTime);

                yield return new WaitForSeconds(swapTime);

                List<GamePiece> clickedPieceMatches = _matchFinder.FindMatchesAt(clickedCell.xIndex, clickedCell.yIndex);
                List<GamePiece> targetPieceMatches = _matchFinder.FindMatchesAt(targetCell.xIndex, targetCell.yIndex);
                var colorMatches = CheckDisco(clickedPiece, targetPiece);

                if (targetPieceMatches.Count == 0 && clickedPieceMatches.Count == 0 && colorMatches.Count == 0)
                {
                    clickedPiece.Move(clickedCell.xIndex, clickedCell.yIndex, swapTime);
                    targetPiece.Move(targetCell.xIndex, targetCell.yIndex, swapTime);
                }
                else
                {
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.movesLeft--;
                        GameManager.Instance.UpdateMoves();
                    }
                    yield return new WaitForSeconds(swapTime);
                    Vector2 swipeDirection = new Vector2(targetCell.xIndex - clickedCell.xIndex, targetCell.yIndex - clickedCell.yIndex);
                    clickedCellItem = _board.CheckItem(clickedCell.xIndex, clickedCell.yIndex, swipeDirection, clickedPieceMatches);
                    targetCellItem = _board.CheckItem(targetCell.xIndex, targetCell.yIndex, swipeDirection, targetPieceMatches);

                    if (clickedCellItem != null && targetPiece != null)
                    {
                        GamePiece clickedBombPiece = clickedCellItem.GetComponent<GamePiece>();
                        if (!IsDisco(clickedBombPiece))
                        {
                            clickedBombPiece.ChangeColor(targetPiece);
                        }
                    }

                    if (targetCellItem != null && clickedPiece != null)
                    {
                        GamePiece targetBombPiece = targetCellItem.GetComponent<GamePiece>();

                        if (!IsDisco(targetBombPiece))
                        {
                            targetBombPiece.ChangeColor(clickedPiece);
                        }
                    }


                    _fallAndFillManager.ClearAndRefillBoard(clickedPieceMatches.Union(targetPieceMatches).ToList().Union(colorMatches).ToList());

                }
            }
        }

    }
    private List<GamePiece> CheckDisco(GamePiece clickedPiece, GamePiece targetPiece)
    {
        List<GamePiece> colorMatches = new List<GamePiece>();

        if (IsDisco(clickedPiece) && !IsDisco(targetPiece))
        {
            clickedPiece.matchValue = targetPiece.matchValue;
            colorMatches = _matchFinder.FindAllMatchValue(clickedPiece.matchValue);
        }
        else if (!IsDisco(clickedPiece) && IsDisco(targetPiece))
        {
            targetPiece.matchValue = clickedPiece.matchValue;
            colorMatches = _matchFinder.FindAllMatchValue(targetPiece.matchValue);
        }
        else if (IsDisco(clickedPiece) && IsDisco(targetPiece))
        {
            foreach (GamePiece piece in _board.m_allGamePieces)
            {
                if (!colorMatches.Contains(piece))
                {
                    colorMatches.Add(piece);
                }
            }
        }

        return colorMatches;
    }
    
    bool IsDisco(GamePiece gamePiece)
    {
        Item item = gamePiece.GetComponent<Item>();

        if (item != null)
        {
            return (item._itemType == ItemType.Disco);
        }
        return false;
    }
    

}
