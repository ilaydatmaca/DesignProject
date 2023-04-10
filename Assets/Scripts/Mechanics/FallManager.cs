using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FallManager : MonoBehaviour
{
    private Board _board;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    public List<GamePiece> FallPieces(float collapseTime = 0.1f)
    {
        HashSet<GamePiece> movingPieces = new HashSet<GamePiece>();

        int nullCount = 0;

        for (int x = 0; x < _board.width; x++)
        {
            for (int y = 0; y < _board.height; y++)
            {
                if (_board.AllGamePieces[x, y] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    _board.AllGamePieces[x, y].Move(x, y - nullCount, collapseTime * (nullCount));
                    _board.AllGamePieces[x, y - nullCount] = _board.AllGamePieces[x, y];
                    movingPieces.Add(_board.AllGamePieces[x, y - nullCount]);
                    _board.AllGamePieces[x, y] = null;


                }
            }

            nullCount = 0;
        }

        return movingPieces.ToList();
    }
    
    
    public bool AreAllPiecesIsSet(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                return IsSetup(piece);
            }
        }
        return true;
    }

    
    bool IsSetup(GamePiece piece)
    {
        Vector2 destination = new Vector2(piece.xIndex, piece.yIndex);
        return Vector2.Distance(transform.position, destination) < 0.01f;
    }

}
