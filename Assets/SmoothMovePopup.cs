using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothMovePopup : MonoBehaviour
{
    private bool reachedDestination = false;
    public GameObject box;

    private void Start()
    {
        StartCoroutine(SmoothTranslation(5f));
    }


    IEnumerator SmoothTranslation( float speed)
    {
        Vector3 target = new Vector3(box.transform.position.x, 250, box.transform.position.z);
        
        while (box.transform.position != target) {
            box.transform.position = Vector3.Lerp (box.transform.position, target, Time.deltaTime * speed);
            yield return null;
        }        
    }


    
    
   
}
