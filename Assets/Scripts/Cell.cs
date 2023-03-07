using UnityEngine;

public class Cell : MonoBehaviour {

    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;

    private SwipeManager _swipeManager;
    
    void Awake () 
    {
        _swipeManager = FindObjectOfType<Board>().GetComponent<SwipeManager>();
    }

    public void Init(int x, int y)
    {
        xIndex = x;
        yIndex = y;

        transform.position = new Vector3(x, y, 0);
        UpdateLabel();

    }

    void UpdateLabel()
    {
        var cellName = "Tile (" + xIndex + "," + yIndex + ")";
        gameObject.name = cellName;

    }
    void OnMouseDown()
    {
        if (_swipeManager !=null)
        {
            _swipeManager.ClickCell(this);
        }

    }

    void OnMouseEnter()
    {
        if (_swipeManager !=null)
        {
            _swipeManager.DragToCell(this);
        }

    }

    void OnMouseUp()
    {
        if (_swipeManager !=null)
        {
            _swipeManager.ReleaseCell();
        }

    }

}