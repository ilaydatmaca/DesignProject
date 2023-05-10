using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class SwapManager : MonoBehaviour
{
    private Board _board;
    private BoardManager _boardManager;
    private MatchFinder _matchFinder;
    private ItemManager _itemManager;
    private RoundManager _roundManager;
    
    private void Awake()
    {
        _board = GetComponent<Board>();
        _boardManager = GetComponent<BoardManager>();
        _matchFinder = GetComponent<MatchFinder>();
        _itemManager = GetComponent<ItemManager>();
        _roundManager = FindObjectOfType<GameManager>().GetComponent<RoundManager>();
    }

    public void ClickCell(Cell cell)
    {
        if(!_roundManager.turnView.IsMine || MovesManager.Instance.noMoreMoves || GameManager.Instance.paused)
            return;

        if (BoosterCell.Instance.isHammerActive || BoosterCell.Instance.isDiscoActive ||
            BoosterCell.Instance.isRocketActive)
        {
            BoosterCell.Instance.targetCell = cell;
            BoosterCell.Instance.CallEvent();
        }
        
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
            _board.photonView.RPC("RPC_SwapCells", RpcTarget.All,
                _board.clickedCell.xIndex,
                _board.clickedCell.yIndex,
                _board.targetCell.xIndex,
                _board.targetCell.yIndex);
        }

        _board.clickedCell = null;
        _board.targetCell = null;
    }

    public void SwapCells(int cell1X, int cell1Y, int cell2X, int cell2Y)
    {
        Cell cell1 = _board._allTiles[cell1X, cell1Y];
        Cell cell2 = _board._allTiles[cell2X, cell2Y];

        StartCoroutine(SwapCellsRoutine(cell1, cell2));
    }

    IEnumerator SwapCellsRoutine(Cell cell1, Cell cell2)
    { 

        if (_board.playerInputEnabled && !GameManager.Instance.IsGameOver)
        {
            GamePiece clickedItem = _board.AllGamePieces[cell1.xIndex, cell1.yIndex];
            GamePiece targetItem = _board.AllGamePieces[cell2.xIndex, cell2.yIndex];

            if (targetItem != null && clickedItem != null)
            {
                clickedItem.Move(cell2.xIndex, cell2.yIndex, _board.swapTime);
                targetItem.Move(cell1.xIndex, cell1.yIndex, _board.swapTime);

                // wait for the swap time
                yield return new WaitForSeconds(_board.swapTime);

                // find all matches for each GamePiece after the swap
                List<GamePiece> cellAMatches = _matchFinder.FindMatchesAt(cell1.xIndex, cell1.yIndex);
                List<GamePiece> cellBMatches = _matchFinder.FindMatchesAt(cell2.xIndex, cell2.yIndex);
                List<GamePiece> colorMatches = _itemManager.ProcessDisco(clickedItem, targetItem);
                

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
                    _itemManager.CheckItems(cell1, cell2, cellAMatches, cellBMatches);

                    List<GamePiece> piecesToClear = cellAMatches.Union(cellBMatches).ToList().Union(colorMatches).ToList();
                    
                    _boardManager.BoardChecking(piecesToClear);
                    
                    
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.UpdateMoves();
                    }
                }
            }
        }
    }
}
