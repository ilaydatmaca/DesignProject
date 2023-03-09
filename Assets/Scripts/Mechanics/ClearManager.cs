using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }
    

    public void DestroyAt(int x, int y)
    {
        GamePiece pieceToClear = board.AllGamePieces[x, y];

        if (pieceToClear != null)
        {
            board.AllGamePieces[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }

    }

    // clear a list of GamePieces (plus a potential sublist of GamePieces destroyed by bombs)
    public void DestroyAt(List<GamePiece> gamePieces, List<GamePiece> bombedPieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                DestroyAt(piece.xIndex, piece.yIndex);

                // add a score bonus if we clear four or more pieces
                int bonus = 0;
                if (gamePieces.Count >= 4)
                {
                    bonus = 20;
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ScorePoints(piece, board.scoreMultiplier, bonus);
                }

                // play particle effects for pieces...
                if (board.particleManager != null)
                {
                    // ... cleared by bombs
                    if (bombedPieces.Contains(piece))
                    {
                        board.particleManager.BombFXAt(piece.xIndex, piece.yIndex);
                    }
                    // ... cleared normally
                    else
                    {
                        board.particleManager.ClearPieceFXAt(piece.xIndex, piece.yIndex);
                    }
                }
            }
        }
    }
}
