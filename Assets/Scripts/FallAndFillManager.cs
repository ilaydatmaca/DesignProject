using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallAndFillManager : MonoBehaviour
{
    private Board _board;
    private SwipeManager _swipeManager;
    private MatchFinder _matchFinder;
    private void Awake()
    {
        _board = GetComponent<Board>();
        _swipeManager = GetComponent<SwipeManager>();
        _matchFinder = GetComponent<MatchFinder>();

    }

    public void ClearAndRefillBoard(List<GamePiece> gamePieces)
    {
        StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
    }
    
    IEnumerator ClearAndRefillBoardRoutine(List<GamePiece> gamePieces)
    {
        _swipeManager.playerInputEnabled = false;

        List<GamePiece> matches = gamePieces;
        _board.m_scoreMultiplier = 0;

        do
        {
            _board.m_scoreMultiplier++;
            
            yield return StartCoroutine(ClearAndCollapseRoutine(matches));

            yield return null;

            yield return StartCoroutine(RefillRoutine());

            matches = _matchFinder.FindAllMatches();

            yield return new WaitForSeconds(0.2f);
        }
        while (matches.Count != 0);

        _swipeManager.playerInputEnabled = true;

    }

    IEnumerator ClearAndCollapseRoutine(List<GamePiece> gamePieces)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();
        List<GamePiece> matches = new List<GamePiece>();

        yield return new WaitForSeconds(0.2f);

        bool isFinished = false;

        while (!isFinished)
        {
            List<GamePiece> bombedPieces = _board.GetBombedPieces(gamePieces);
            gamePieces = gamePieces.Union(bombedPieces).ToList();

            bombedPieces = _board.GetBombedPieces(gamePieces);
            gamePieces = gamePieces.Union(bombedPieces).ToList();

            _board.ClearPieceAt(gamePieces, bombedPieces);

            if (_swipeManager.clickedCellItem != null)
            {
                ActivateBomb(_swipeManager.clickedCellItem);
                _swipeManager.clickedCellItem = null;
            }

            if (_swipeManager.targetCellItem != null)
            {
                ActivateBomb(_swipeManager.targetCellItem);
                _swipeManager.targetCellItem = null;
            }

            yield return new WaitForSeconds(0.25f);

            movingPieces = CollapseColumn(gamePieces);
            while (!IsCollapsed(movingPieces))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);

            matches = _matchFinder.FindMatchesAt(movingPieces);

            if (matches.Count == 0)
            {
                isFinished = true;
            }
            else
            {
                _board.m_scoreMultiplier++;
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayBonusSound();
                }
                yield return StartCoroutine(ClearAndCollapseRoutine(matches));
            }
        }
        yield return null;
    }
    
    void ActivateBomb(GameObject bomb)
    {
        int x = (int)bomb.transform.position.x;
        int y = (int)bomb.transform.position.y;


        if(_board.IsWithinBounds(x, y))
        {
            _board.m_allGamePieces[x, y] = bomb.GetComponent<GamePiece>();
        }
    }

    
    bool IsCollapsed(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                if (piece.transform.position.y - (float)piece.yIndex > 0.001f)
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator RefillRoutine()
    {
        _board.FillBoard();
        yield return null;
    }

    List<GamePiece> CollapseColumn(int column, float collapseTime = 0.1f)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();

        for (int i = 0; i < _board.height - 1; i++)
        {
            if (_board.m_allGamePieces[column, i] == null)
            {
                for (int j = i + 1; j < _board.height; j++)
                {
                    if (_board.m_allGamePieces[column, j] != null)
                    {
                        _board.m_allGamePieces[column, j].Move(column, i, collapseTime * (j - i));

                        _board.m_allGamePieces[column, i] = _board.m_allGamePieces[column, j];

                        if (!movingPieces.Contains(_board.m_allGamePieces[column, i]))
                        {
                            movingPieces.Add(_board.m_allGamePieces[column, i]);
                        }

                        _board.m_allGamePieces[column, j] = null;

                        break;
                    }
                }
            }
        }
        return movingPieces;
    }

    List<GamePiece> CollapseColumn(List<GamePiece> gamePieces)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();

        List<int> columnsToCollapse = GetColumns(gamePieces);

        foreach (int column in columnsToCollapse)
        {
            movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
        }

        return movingPieces;
    }
    
    List<int> GetColumns(List<GamePiece> gamePieces)
    {
        List<int> columns = new List<int>();

        foreach (GamePiece piece in gamePieces)
        {
            if (!columns.Contains(piece.xIndex))
            {
                columns.Add(piece.xIndex);
            }
        }

        return columns;
    }
}
