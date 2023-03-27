
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }
    void BoardChecking(List<GamePiece> gamePieces)
    {
        StartCoroutine(BoardRoutine(gamePieces));
    }

    public IEnumerator BoardRoutine(List<GamePiece> gamePieces)
    {

        board.playerInputEnabled = false;
        board.isRefilling = true;

        List<GamePiece> matches = gamePieces;

        // store a score multiplier for chain reactions
        board.scoreMultiplier = 0;

        while(matches.Count != 0)
        {
            board.scoreMultiplier++;
            
            yield return StartCoroutine(ClearAndFallRoutine(matches));

            yield return null;
            
            yield return StartCoroutine(RefillRoutine());
            
            matches = board.matchFinder.FindAllMatches();
            yield return new WaitForSeconds(board.delay);

        }

        StartCoroutine(DeadlockCheck());


        board.playerInputEnabled = true;
        board.isRefilling = false;
    }


    IEnumerator ClearAndFallRoutine(List<GamePiece> gamePieces)
    {

        yield return new WaitForSeconds(board.delay);

        bool isFinished = false;

        while (!isFinished)
        {

            List<GamePiece> bombedPieces = board.GetAffectedPiecesByItems(gamePieces);
            gamePieces = gamePieces.Union(bombedPieces).ToList();

            bombedPieces = board.GetAffectedPiecesByItems(gamePieces);
            gamePieces = gamePieces.Union(bombedPieces).ToList();

            board.clearManager.DestroyAt(gamePieces, bombedPieces);

            board.ıtemManager.InitAllBombs();

            
            yield return new WaitForSeconds(board.delay);

            List<GamePiece> movingPieces =board.fallManager.FallPieces();
            while (!board.fallManager.AreAllPiecesIsSet(movingPieces))
            {
                yield return null;
            }

            yield return new WaitForSeconds(board.delay);
            
            List<GamePiece> allMatchesInBoard = board.matchFinder.FindAllMatches();

            if (allMatchesInBoard.Count == 0)
            {
                isFinished = true;
            }
            
            else
            {
                board.scoreMultiplier++;

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
        if (board.boardDeadlock.IsDeadlocked())
        {
            yield return new WaitForSeconds(board.delay * 5f);

            yield return StartCoroutine(board.boardShuffler.ShuffleBoardRoutine());

            yield return new WaitForSeconds(board.delay * 5f);

            yield return StartCoroutine(RefillRoutine());
        }
    }
    
    
    
    // clear and refill one position of the Board (used by Booster)
    public void ClearAndRefillBoard(int x, int y)
    {
        if (board.IsInBorder(x, y))
        {
            GamePiece pieceToClear = board.AllGamePieces[x, y];
            List<GamePiece> listOfOne = new List<GamePiece>();
            listOfOne.Add(pieceToClear);
            BoardChecking(listOfOne);
        }
    }
    
    
    // coroutine to refill the Board
    IEnumerator RefillRoutine()
    {
        board.FillBoard();

        yield return null;
    }
}
