using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ShuffleManager : MonoBehaviour
{
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
    
    public IEnumerator ShuffleBoardRoutine()
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

        List<GamePiece> nonItems = GetNonItems();

        ShuffleWithFisherYates(nonItems);

        FillBoardFromList(nonItems);

        MovePieces();

        // in the event some matches form, clear and refill the Board
        List<GamePiece> matches = _matchFinder.FindAllMatches();
        StartCoroutine(_boardManager.BoardRoutine(matches));


    }


    List<GamePiece> GetNonItems()
    {

        List<GamePiece> nonItems = new List<GamePiece>();

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
                        nonItems.Add(_board.AllGamePieces[i, j]);
                        _board.AllGamePieces[i, j] = null;
                    }
                }
            }
        }
        return nonItems;
    }

    void ShuffleWithFisherYates(List<GamePiece> piecesToShuffle)
    {
        int maxCount = piecesToShuffle.Count;

        for (int i = 0; i < maxCount - 1; i++)
        {
            int r = Random.Range(i, maxCount);

            if (r == i)
            {
                continue;
            }
            (piecesToShuffle[r], piecesToShuffle[i]) = (piecesToShuffle[i], piecesToShuffle[r]);
        }
    }
    
    void FillBoardFromList(List<GamePiece> gamePieces)
    {
        Queue<GamePiece> unusedPieces = new Queue<GamePiece>(gamePieces);

        int maxIterations = 100;

        for (int i = 0; i < _board.width; i++)
        {
            for (int j = 0; j < _board.height; j++)
            {
                if (_board.AllGamePieces[i, j] == null)
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


    void MovePieces()
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
