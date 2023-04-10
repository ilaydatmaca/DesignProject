using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    private int _count;
    private bool _isRunning = true;

    void Update()
    {
        if (_isRunning)
        {
            StartCoroutine(ChangeText());
            if (_count < 3)
            {
                loadingText.text += ".";
                _count++;
            }
            else
            {
                loadingText.text = "Loading";
                _count = 0;
            }
            _isRunning = false;
        }
    }

    
    IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(1.1f);
        _isRunning = true;
    }
    
   



   
}
