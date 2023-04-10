using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    private Board _board;
    private ParticleManager _particleManager;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();
    }
    

    public void DestroyAt(int x, int y)
    {
        GamePiece pieceToClear = _board.AllGamePieces[x, y];
        if (pieceToClear != null)
        {
            _board.AllGamePieces[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }

    }

    // clear a list of GamePieces (plus a potential sublist of GamePieces destroyed by items)
    public void DestroyAt(List<GamePiece> gamePieces, List<GamePiece> bombedPieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                DestroyAt(piece.xIndex, piece.yIndex);

                int bonus = 0;
                if (gamePieces.Count >= 4)
                {
                    bonus = 20;
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ScorePoints(piece, _board.scoreMultiplier, bonus);
                }

                // play particle effects for pieces...
                if (_particleManager != null)
                {
                    // ... cleared by bombs 
                    if (bombedPieces.Contains(piece))
                    {
                        _particleManager.BombFXAt(piece.xIndex, piece.yIndex);
                    }
                    // ... cleared normally
                    else
                    {
                        _particleManager.ClearPieceFXAt(piece.xIndex, piece.yIndex);
                    }
                }
            }
        }
    }
}
