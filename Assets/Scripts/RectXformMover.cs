using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectXformMover : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 onScreenPosition;
    public Vector3 endPosition;

    public float timeToMove = 1f;

    private RectTransform rectXform;
    private bool isMoving = false;

    void Awake()
    {
        rectXform = GetComponent<RectTransform>();
    }

    void Move(Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRoutine(startPos, endPos, timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        if (rectXform != null)
        {
            rectXform.anchoredPosition = startPos;
        }

        bool reachedDestination = false;
        float elapsedTime = 0f;
        isMoving = true;

        while (!reachedDestination)
        {
            if (Vector3.Distance(rectXform.anchoredPosition, endPos) < 0.01f)
            {
                reachedDestination = true;
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
            t = t * t * t * (t * (t * 6 - 15) + 10);

            if (rectXform != null)
            {
                rectXform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            }

            yield return null;
        }

        isMoving = false;

    }

    public void MoveOn()
    {
        Move(startPosition, onScreenPosition, timeToMove);
    }
    
    public void MoveOff()
    {
        Move(onScreenPosition, endPosition, timeToMove);
    }
}
