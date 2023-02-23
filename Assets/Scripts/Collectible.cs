using UnityEngine;
using System.Collections;

public class Collectible : GamePiece 
{
    public bool clearedByBomb = false;
    public bool clearedAtBottom = true;


    // Use this for initialization
    void Start () 
    {
        matchValue = MatchValue.None;
    }
	
    // Update is called once per frame
    void Update () 
    {
	
    }
}