using UnityEngine;


public class BoardFiller : MonoBehaviour
{
    public Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }


    public void MakeColorBombBooster(int x, int y)
    {
        if (board == null)
            return;

        if (board.IsInBorder(x, y))
        {
            GamePiece pieceToReplace = board.AllGamePieces[x, y];

            if (pieceToReplace != null)
            {
                board.clearManager.DestroyAt(x, y);
                GameObject bombObject = board.itemFactory.MakeItem(ItemType.Disco, x, y);
                board.ıtemManager.InitBomb(bombObject);
            }
        }
    }
}