using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {

	public int xIndex;
	public int yIndex;
	public MatchType matchValue;

	private bool _isMoving = false;

	private Board _board;
	
	private InterpType interpolation = InterpType.SmootherStep;
	
	public AudioClip clearSound;

	private int fillYOffset = 10;
	private float fillMoveTime = 0.5f;
	private int scoreValue = 20;

	public void Init(Board board, int x, int y)
	{
		_board = board;
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

	public void Fall()
	{
		transform.position = new Vector3(xIndex, yIndex + fillYOffset, 0);
		Move(xIndex, yIndex, fillMoveTime);
	}


	void Set(int x, int y)
	{
		transform.position = new Vector3(x, y, 0);
		transform.rotation = Quaternion.identity;
		if (_board.IsWithinBounds(x, y))
		{
			_board.m_allGamePieces[x, y] = this;
		}
		xIndex = x;
		yIndex = y;
	}


	IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
	{
		Vector3 startPosition = transform.position;

		bool reachedDestination = false;

		float elapsedTime = 0f;

		_isMoving = true;

		while (!reachedDestination)
		{
			// if we are close enough to destination
			if (Vector3.Distance(transform.position, destination) < 0.01f)
			{
				reachedDestination = true;
				Set((int) destination.x, (int) destination.y);
			}
			else
			{
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
		}
		_isMoving = false;
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

	public void ScorePoints(int multiplier = 1, int bonus = 0)
	{
		if (ScoreManager.Instance != null)
		{
			ScoreManager.Instance.AddScore(scoreValue * multiplier + bonus);
		}

		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.PlayClipAtPoint(clearSound, Vector3.zero, SoundManager.Instance.fxVolume);

		}
	}
}
