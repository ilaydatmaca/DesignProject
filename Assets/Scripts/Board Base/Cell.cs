using UnityEngine;


public class Cell : MonoBehaviour {

	[HideInInspector] public int xIndex;
	[HideInInspector] public int yIndex;

	private SwapManager _swapManager;

	private void Awake()
	{
		_swapManager = FindObjectOfType<Board>().GetComponent<SwapManager>();
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
		var cellName = "Cell " + xIndex + "," + yIndex;
		gameObject.name = cellName;

	}
	void OnMouseDown()
	{
		if (_swapManager !=null)
		{
			_swapManager.ClickCell(this);
		}

	}
	void OnMouseEnter()
	{
		if (_swapManager !=null)
		{
			_swapManager.DragCell(this);
		}
	}
	void OnMouseUp()
	{
		if (_swapManager !=null)
		{
			_swapManager.ReleaseCell();
		}
	}

}
