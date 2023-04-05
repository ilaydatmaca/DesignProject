using System.Collections;
using UnityEngine;

public class SmoothMovePopup : MonoBehaviour
{
    RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }


    private void Start()
    {
        StartCoroutine(SmoothTranslation(5f));
    }


    IEnumerator SmoothTranslation( float speed)
    {
        Vector3 target = new Vector3(_rectTransform.position.x, 250, _rectTransform.position.z);

        while (_rectTransform.position.y - target.y >= 0.2f) {
            _rectTransform.position = Vector3.Lerp (_rectTransform.position, target, Time.deltaTime * speed);
            yield return null;
        }        
    }


    
    
   
}
