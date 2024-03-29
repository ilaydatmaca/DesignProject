﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShuffleManager : MonoBehaviour
{
    private List<GamePiece> _nonItems;

    private Board _board;
    private BoardManager _boardManager;
    private MatchFinder _matchFinder;
    private FallManager _fallManager;
    private void Awake()
    {
        _board = GetComponent<Board>();
        _boardManager = GetComponent<BoardManager>();
        _matchFinder = GetComponent<MatchFinder>();
        _fallManager = GetComponent<FallManager>();
    }

    public void ShuffleBoard()
    {
        StartCoroutine(ShuffleBoardRoutine());
    }
    
    IEnumerator ShuffleBoardRoutine()
    {
        
        List<GamePiece> allPieces = new List<GamePiece>();
        foreach (GamePiece piece in _board.AllGamePieces)
        {
            allPieces.Add(piece);
        }

        while (!_fallManager.AreAllPiecesIsSet(allPieces))
        {
            yield return null;
        }

        _nonItems = GetNonItems();

        if (PhotonNetwork.IsMasterClient)
        {
            ShuffleWithFisherYates();

            _board.photonView.RPC("RPC_FillBoardFromList", RpcTarget.All);
            _board.photonView.RPC("RPC_MovePieces", RpcTarget.All);
        }
        
        yield return new WaitForSeconds(_board.delay);
        
        List<GamePiece> matches = _matchFinder.FindAllMatches();
        _boardManager.BoardChecking(matches);
    }


    List<GamePiece> GetNonItems()
    {
        _nonItems = new List<GamePiece>();

        int width = _board.AllGamePieces.GetLength(0);
        int height = _board.AllGamePieces.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_board.AllGamePieces[i, j] != null)
                {
                    Item item = _board.AllGamePieces[i, j].GetComponent<Item>();
                    
                    if (item == null)
                    {
                        _nonItems.Add(_board.AllGamePieces[i, j]);
                        _board.AllGamePieces[i, j] = null;
                    }
                }
            }
        }
        return _nonItems;
    }

    void ShuffleWithFisherYates()
    {
        int maxCount = _nonItems.Count;

        for (int i = 0; i < maxCount - 1; i++)
        {
            int r = Random.Range(i, maxCount);

            if (r == i)
            {
                continue;
            }
            _board.photonView.RPC("RPC_SwapArrayItems", RpcTarget.All, r, i);
        }
    }

    public void SwapArrayItems(int index1, int index2)
    {
        (_nonItems[index1], _nonItems[index2]) = (_nonItems[index2], _nonItems[index1]);
    }
    
    public void FillBoardFromList()
    {
        Queue<GamePiece> unusedPieces = new Queue<GamePiece>(_nonItems);

        int maxIterations = 100;

        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                if (_board.AllGamePieces[i, j] == null && unusedPieces.Count != 0)
                {
                    _board.AllGamePieces[i, j] = unusedPieces.Dequeue();
                    
                    int iterationNum = 0;

                    while (_board.HasMatchOnFill(i, j) && iterationNum < maxIterations)
                    {
                        unusedPieces.Enqueue(_board.AllGamePieces[i, j]);
                        
                        _board.AllGamePieces[i, j] = unusedPieces.Dequeue();
                        iterationNum++;
                    }
                }
            }
        }
    }


    public void MovePieces()
    {
        int width = _board.AllGamePieces.GetLength(0);
        int height = _board.AllGamePieces.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_board.AllGamePieces[i, j] != null)
                {
                    _board.AllGamePieces[i, j].Move(i, j, _board.swapTime);
                }
            }
        }

    }


}
