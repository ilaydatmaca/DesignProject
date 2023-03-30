using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Deneme : MonoBehaviour
{
    public GameObject[] cubePrefabs;
    
    private void Start()
    {
        Ins();

    }
    
    void Ins()
    {
        int randomIdx = Random.Range(0, cubePrefabs.Length);
        Instantiate(cubePrefabs[randomIdx], Vector3.zero, Quaternion.identity);
    }
}
