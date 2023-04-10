using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    private Board _board;
    private ParticleManager _particleManager;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _particleManager = FindObjectOfType<ParticleManager>().GetComponent<ParticleManager>();
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

                if (_particleManager != null)
                {
                    if (bombedPieces.Contains(piece))
                    {
                        _particleManager.BombFXAt(piece.xIndex, piece.yIndex);
                    }
                    else
                    {
                        _particleManager.ClearPieceFXAt(piece.xIndex, piece.yIndex);
                    }
                }
            }
        }
    }
}
