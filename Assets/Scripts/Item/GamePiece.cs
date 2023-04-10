using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {
	
	public int xIndex;
	public int yIndex;
	public MatchValue matchValue;

	private bool _isMoving;
	private readonly int _fallOffset = 10;
	
	public int scoreValue = 20;
	public AudioClip clearSound;

    private Board _board;

    public void Init(Board board, int x, int y)
	{
		_board = board;
		xIndex = x;
		yIndex = y;
	}
    
    public void Fall()
    {
	    transform.position = new Vector3(xIndex, yIndex + _fallOffset, 0);
	    Move(xIndex, yIndex, _board.swapTime);
    }


    void Set(int x, int y)
    {
	    transform.position = new Vector3(x, y, 0);
	    if (_board.IsInBorder(x, y))
	    {
		    _board.AllGamePieces[x, y] = this;
	    }
	    xIndex = x;
	    yIndex = y;
    }

    public void Move (int destX, int destY, float timeToMove)
	{
		if (!_isMoving)
		{
			StartCoroutine(MoveRoutine(new Vector3(destX, destY,0), timeToMove));	
		}
	}
	
	IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
	{
		Vector3 startPosition = transform.position;
		
		bool isReached = false;

        // how much time has passed since we started moving
		float elapsedTime = 0f;

		_isMoving = true;

		while (!isReached)
		{
			if (Vector3.Distance(transform.position, destination) < 0.01f)
			{
				isReached = true;
				Set((int) destination.x, (int) destination.y);
			}

			else
			{
				elapsedTime += Time.deltaTime;

				// calculate the Lerp value
				float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
				t =  t*t*t*(t*(t*6 - 15) + 10);
				
				transform.position = Vector3.Lerp(startPosition, destination, t);

				// wait until next frame
				yield return null;
			}
		}

		_isMoving = false;
	}

	public bool IsDisco()
	{
		Item ıtem = GetComponent<Item>();

		if (ıtem != null)
		{
			return (ıtem.ıtemType == ItemType.Disco);
		}
		return false;
	}

}
