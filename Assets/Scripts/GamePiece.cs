using UnityEngine;
using System.Collections;

public enum MatchValue
{
	Yellow,
	Blue,
	Magenta,
	Indigo,
	Green,
	Teal,
	Red,
	Cyan,
	Wild,
	None
}


public class GamePiece : MonoBehaviour {

	public int xIndex;
	public int yIndex;

	Board m_board;

	bool m_isMoving = false;

	public InterpType interpolation = InterpType.SmootherStep;

	public enum InterpType
	{
		Linear,
		EaseOut,
		EaseIn,
		SmoothStep,
		SmootherStep
	};

	public MatchValue matchValue;



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Move((int)transform.position.x + 2, (int) transform.position.y, 0.5f);

		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Move((int)transform.position.x - 2, (int) transform.position.y, 0.5f);

		}
		*/

	}

	public void Init(Board board)
	{
		m_board = board;
	}

	public void SetCoord(int x, int y)
	{
		xIndex = x;
		yIndex = y;
	}

	public void Move (int destX, int destY, float timeToMove)
	{

		if (!m_isMoving)
		{

			StartCoroutine(MoveRoutine(new Vector3(destX, destY,0), timeToMove));	
		}
	}


	IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
	{
		Vector3 startPosition = transform.position;

		bool reachedDestination = false;

		float elapsedTime = 0f;

		m_isMoving = true;

		while (!reachedDestination)
		{
			// if we are close enough to destination
			if (Vector3.Distance(transform.position, destination) < 0.01f)
			{

				reachedDestination = true;

				if (m_board !=null)
				{
					m_board.PlaceGamePiece(this, (int) destination.x, (int) destination.y);

				}

				break;
			}

			// track the total running time
			elapsedTime += Time.deltaTime;

			// calculate the Lerp value
			float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

			switch (interpolation)
			{
				case InterpType.Linear:
					break;
				case InterpType.EaseOut:
					t = Mathf.Sin(t * Mathf.PI * 0.5f);
					break;
				case InterpType.EaseIn:
					t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
					break;
				case InterpType.SmoothStep:
					t = t*t*(3 - 2*t);
					break;
				case InterpType.SmootherStep:
					t =  t*t*t*(t*(t*6 - 15) + 10);
					break;
			}

			// move the game piece
			transform.position = Vector3.Lerp(startPosition, destination, t);

			// wait until next frame
			yield return null;
		}

		m_isMoving = false;


	}

	public void ChangeColor(GamePiece pieceToMatch)
	{
		SpriteRenderer rendererToChange = GetComponent<SpriteRenderer>();

		if (pieceToMatch !=null)
		{
			SpriteRenderer rendererToMatch = pieceToMatch.GetComponent<SpriteRenderer>();

			if (rendererToMatch !=null && rendererToChange !=null)
			{
				rendererToChange.color = rendererToMatch.color;
			}

			matchValue = pieceToMatch.matchValue;
		}

	}
}
