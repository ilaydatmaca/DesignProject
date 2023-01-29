using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public int xIndex;
    public int yIndex;

    Board m_board;

    // Use this for initialization
    void Start () 
    {
	
    }

    public void Init(int x, int y, Board board)
    {
        xIndex = x;
        yIndex = y;
        m_board = board;

    }

    void OnMouseDown()
    {
        if (m_board !=null)
        {
            m_board.ClickTile(this);
        }

    }

    void OnMouseEnter()
    {
        if (m_board !=null)
        {
            m_board.DragToTile(this);
        }

    }

    void OnMouseUp()
    {
        if (m_board !=null)
        {
            m_board.ReleaseTile();
        }

    }
}