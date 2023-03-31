using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referencer : MonoBehaviour
{
    public ItemFactory ItemFactory;
    public static Referencer Inst { get; private set; }
    void Awake(){
        if(!Inst){
            Inst = this;
        }
        else if(Inst != this){
            Destroy(gameObject);
        }
    }
}
