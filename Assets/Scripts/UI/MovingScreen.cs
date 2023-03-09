using UnityEngine;
using System.Collections;

public class MovingScreen : MonoBehaviour 
{
	public Vector3 startPosition;
	public Vector3 onscreenPosition;
	public Vector3 offScreenPosition;

	public float timeToMove = 1f;
	bool _isMoving;

	RectTransform _rectTransform;
	
	void Awake() 
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	public void MoveOn()
	{
		Move (startPosition, onscreenPosition);
	}

	public void MoveOff()
	{
		Move (onscreenPosition, offScreenPosition);
	}

	void Move(Vector3 startPos, Vector3 endPos)
	{
		if (!_isMoving) 
		{
			StartCoroutine (MoveRoutine (startPos, endPos));
		}
	}

	IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos)
	{
		_rectTransform.anchoredPosition = startPos;

		bool reachedDestination = false;
		float elapsedTime = 0f;
		_isMoving = true;

		while (!reachedDestination) 
		{
			if (Vector3.Distance (_rectTransform.anchoredPosition, endPos) < 0.01f)
			{
				reachedDestination = true;
				_isMoving = false;
			}
			else
			{
				
				elapsedTime += Time.deltaTime;

				float t = Mathf.Clamp (elapsedTime / timeToMove, 0f, 1f);
				t = t * t * t * (t * (t * 6 - 15) + 10);

				_rectTransform.anchoredPosition = Vector3.Lerp (startPos, endPos, t);
				
				yield return null;
			}
		}
	}

}
