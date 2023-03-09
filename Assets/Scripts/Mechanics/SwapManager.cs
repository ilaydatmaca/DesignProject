
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwapManager : MonoBehaviour
{

    private Board _board;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    public void ClickCell(Cell cell)
    {
        if (_board.clickedCell == null)
        {
            _board.clickedCell = cell;
        }
    }

    public void DragCell(Cell cell)
    {
        if (_board.clickedCell != null && _board.IsNextTo(cell, _board.clickedCell))
        {
            _board.targetCell = cell;
        }
    }
    public void ReleaseCell()
    {
        if (_board.clickedCell != null && _board.targetCell != null)
        {
            SwapCells(_board.clickedCell, _board.targetCell);
        }

        _board.clickedCell = null;
        _board.targetCell = null;
    }

    public void SwapCells(Cell clickedCell, Cell targetCell)
    {
        StartCoroutine(SwapCellsRoutine(clickedCell, targetCell));
    }

    IEnumerator SwapCellsRoutine(Cell cell1, Cell cell2)
    { 

        if (_board.playerInputEnabled && !GameManager.Instance.IsGameOver)
        {
            GamePiece clickedItem = _board.allGamePieces[cell1.xIndex, cell1.yIndex];
            GamePiece targetItem = _board.allGamePieces[cell2.xIndex, cell2.yIndex];

            if (targetItem != null && clickedItem != null)
            {
                clickedItem.Move(cell2.xIndex, cell2.yIndex, _board.swapTime);
                targetItem.Move(cell1.xIndex, cell1.yIndex, _board.swapTime);

                // wait for the swap time
                yield return new WaitForSeconds(_board.swapTime);

                // find all matches for each GamePiece after the swap
                List<GamePiece> cellAMatches = _board.matchFinder.FindMatchesAt(cell1.xIndex, cell1.yIndex);
                List<GamePiece> cellBMatches = _board.matchFinder.FindMatchesAt(cell2.xIndex, cell2.yIndex);
                List<GamePiece> colorMatches = _board.ıtemManager.ProcessDisco(clickedItem, targetItem);
                

                // if we don't make any matches, then swap the pieces back
                if (cellBMatches.Count == 0 && cellAMatches.Count == 0 && colorMatches.Count == 0)
                {
                    clickedItem.Move(cell1.xIndex, cell1.yIndex, _board.swapTime);
                    targetItem.Move(cell2.xIndex, cell2.yIndex, _board.swapTime);
                }
                else
                {
                    // wait for our swap time
                    yield return new WaitForSeconds(_board.swapTime);

                    // drop items on either tile if necessary
                    _board.ıtemManager.CheckItems(cell1, cell2, clickedItem, targetItem, cellAMatches, cellBMatches);

                    List<GamePiece> piecesToClear = cellAMatches.Union(cellBMatches).ToList().Union(colorMatches).ToList();
                    
                    yield return StartCoroutine(_board.boardManager.BoardRoutine(piecesToClear));


                    // otherwise, we decrement our moves left
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.UpdateMoves();
                    }
                }
            }
        }
    }
}
