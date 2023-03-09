using UnityEngine;


public class Cell : MonoBehaviour {

	[HideInInspector] public int xIndex;
	[HideInInspector] public int yIndex;

	Board _board;

	public void Init(int x, int y, Board board)
	{
		xIndex = x;
		yIndex = y;
		_board = board;
		
		transform.position = new Vector3(x, y, 0);
		UpdateLabel();

	}
	
	void UpdateLabel()
	{
		var cellName = "Cell " + xIndex + "," + yIndex;
		gameObject.name = cellName;

	}
	void OnMouseDown()
	{
		if (_board !=null)
		{
			_board.swapManager.ClickCell(this);
		}

	}
	void OnMouseEnter()
	{
		if (_board !=null)
		{
			_board.swapManager.DragCell(this);
		}
	}
	void OnMouseUp()
	{
		if (_board !=null)
		{
			_board.swapManager.ReleaseCell();
		}
	}

}
