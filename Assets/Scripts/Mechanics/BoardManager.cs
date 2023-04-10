using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Board _board;
    private MatchFinder _matchFinder;
    private ItemManager _itemManager;
    private ShuffleManager _shuffleManager;
    private FallManager _fallManager;
    private DeadlockManager _deadlockManager;
    private ClearManager _clearManager;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _matchFinder = GetComponent<MatchFinder>();
        _itemManager = GetComponent<ItemManager>();
        _shuffleManager = GetComponent<ShuffleManager>();
        _fallManager = GetComponent<FallManager>();
        _deadlockManager = GetComponent<DeadlockManager>();
        _clearManager = GetComponent<ClearManager>();
    }
    public void BoardChecking(List<GamePiece> gamePieces)
    {
        StartCoroutine(BoardRoutine(gamePieces));
    }

    IEnumerator BoardRoutine(List<GamePiece> gamePieces)
    {

        _board.playerInputEnabled = false;
        _board.isRefilling = true;

        List<GamePiece> matches = gamePieces;

        // store a score multiplier for chain reactions
        _board.scoreMultiplier = 0;

        while(matches.Count != 0)
        {
            _board.scoreMultiplier++;
            
            yield return StartCoroutine(ClearAndFallRoutine(matches));

            yield return null;
            
            matches = _matchFinder.FindAllMatches();
            
            yield return new WaitForSeconds(_board.delay);

        }
        
        yield return StartCoroutine(RefillRoutine());

        if (_matchFinder.FindAllMatches().Count > 0)
        {
            Debug.Log(_matchFinder.FindAllMatches().Count);
            BoardChecking(_matchFinder.FindAllMatches());
        }
        else
        {
            StartCoroutine(DeadlockCheck());
            _board.playerInputEnabled = true;
            _board.isRefilling = false;
        }
        
    }


    IEnumerator ClearAndFallRoutine(List<GamePiece> gamePieces)
    {

        yield return new WaitForSeconds(_board.delay);

        bool isFinished = false;

        while (!isFinished)
        {

            List<GamePiece> bombedPieces = _board.GetAffectedPiecesByItems(gamePieces);
            gamePieces = gamePieces.Union(bombedPieces).ToList();

            bombedPieces = _board.GetAffectedPiecesByItems(gamePieces);
            gamePieces = gamePieces.Union(bombedPieces).ToList();

            _clearManager.DestroyAt(gamePieces, bombedPieces);

            _itemManager.InitAllItems();

            yield return new WaitForSeconds(_board.delay);

            List<GamePiece> movingPieces = _fallManager.FallPieces();
            while (!_fallManager.AreAllPiecesIsSet(movingPieces))
            {
                yield return null;
            }

            yield return new WaitForSeconds(_board.delay);
            
            List<GamePiece> allMatchesInBoard = _matchFinder.FindAllMatches();

            if (allMatchesInBoard.Count == 0)
            {
                isFinished = true;
            }
            
            else
            {
                _board.scoreMultiplier++;

                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayBonusSound();
                }
                yield return StartCoroutine(ClearAndFallRoutine(allMatchesInBoard));
            }
        }
        yield return null;
    }
    
    

    private IEnumerator DeadlockCheck()
    {
        // deadlock check
        
        if (_deadlockManager.IsDeadlocked())
        {
            yield return new WaitForSeconds(_board.delay * 5f);

            _shuffleManager.ShuffleBoard();

            yield return new WaitForSeconds(_board.delay * 5f);
            
            BoardChecking(_matchFinder.FindAllMatches());
        }
    }
    
    
    // clear and refill one position of the Board (used by Booster)
    public void ClearAndRefillBoard(int x, int y)
    {
        if (_board.IsInBorder(x, y))
        {
            GamePiece pieceToClear = _board.AllGamePieces[x, y];
            List<GamePiece> listOfOne = new List<GamePiece> {pieceToClear};
            BoardChecking(listOfOne);
        }
    }
    
    
    // coroutine to refill the Board
    IEnumerator RefillRoutine()
    {
        _board.FillBoard();

        yield return new WaitForSeconds(_board.delay);
        
    }
}
