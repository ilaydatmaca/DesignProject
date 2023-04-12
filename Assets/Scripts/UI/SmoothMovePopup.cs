using System.Collections;
using UnityEngine;

public class SmoothMovePopup : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _timeToMove = 1f;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }


    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        Vector3 endPos = new Vector3(_rectTransform.anchoredPosition.x, 0);
        Vector3 startPos = _rectTransform.anchoredPosition;

        bool reachedDestination = false;
        float elapsedTime = 0f;

        while (!reachedDestination) 
        {
            if (Vector3.Distance (_rectTransform.anchoredPosition, endPos) < 0.01f)
            {
                reachedDestination = true;

            }
            else
            {
                elapsedTime += Time.deltaTime;
            
                float t = Mathf.Clamp (elapsedTime / _timeToMove, 0f, 1f);
                t = t * t * t * (t * (t * 6 - 15) + 10);
            
                if (_rectTransform != null)
                {
                    _rectTransform.anchoredPosition = Vector3.Lerp (startPos, endPos, t);
              
                }

                yield return null;

            }
        }
    }
}
