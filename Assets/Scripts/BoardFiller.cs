using System.Collections.Generic;
using UnityEngine;


public class BoardFiller : MonoBehaviour
{
    public Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }
    

    // fill the Board using a known list of GamePieces instead of Instantiating new prefabs
    public void FillBoardFromList(List<GamePiece> gamePieces)
    {
        // create a first in-first out Queue to store the GamePieces in a pre-set order
        Queue<GamePiece> unusedPieces = new Queue<GamePiece>(gamePieces);

        // iterations to prevent infinite loop
        int maxIterations = 100;
        int iterations = 0;

        // loop through each position on the Board
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // only fill in a GamePiece if 
                if (board.allGamePieces[i, j] == null)
                {
                    // grab a new GamePiece from the Queue
                    board.allGamePieces[i, j] = unusedPieces.Dequeue();

                    // reset iteration count
                    iterations = 0;

                    // while a match forms when filling in a GamePiece...
                    while (board.HasMatchOnFill(i, j))
                    {
                        // put the GamePiece back into the Queue at the end of the line
                        unusedPieces.Enqueue(board.allGamePieces[i, j]);

                        // grab a new GamePiece from the Queue
                        board.allGamePieces[i, j] = unusedPieces.Dequeue();

                        // increment iterations each time we try to place a piece
                        iterations++;

                        // if our iterations exceeds limit, break out of the loop and move to next position
                        if (iterations >= maxIterations)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
    


    public void MakeColorBombBooster(int x, int y)
    {
        if (board == null)
            return;

        if (board.IsInBorder(x, y))
        {
            GamePiece pieceToReplace = board.allGamePieces[x, y];

            if (pieceToReplace != null)
            {
                board.clearManager.DestroyAt(x, y);
                GameObject bombObject = board.itemFactory.MakeItem(ItemType.Disco, x, y);
                board.ıtemManager.InitBomb(bombObject);
            }
        }
    }
}