using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    private int count;
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            StartCoroutine(changeText());
            if (count < 3)
            {
                loadingText.text += ".";
                count++;
            }
            else
            {
                loadingText.text = "Loading";
                count = 0;
            }
            isRunning = false;
        }
    }

    
    IEnumerator changeText()
    {
        yield return new WaitForSeconds(1.1f);
        isRunning = true;
    }
    
   



   
}
