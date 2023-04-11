using System.Collections;
using UnityEngine;

public class SmoothMovePopup : MonoBehaviour
{
    RectTransform _rectTransform;
    private float _speed = 2f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }


    private void Start()
    {
        StartCoroutine(SmoothTranslation());
    }


    IEnumerator SmoothTranslation()
    {
        Vector3 target = new Vector3(_rectTransform.anchoredPosition.x, 0);

        while (Vector3.Distance(target, _rectTransform.anchoredPosition) > 0.1f)
        {
            _rectTransform.anchoredPosition = Vector3.Lerp (_rectTransform.anchoredPosition, target, Time.deltaTime * _speed);

        }

        yield return null;
    }


    
    
   
}
