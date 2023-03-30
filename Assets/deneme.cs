using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class Deneme : MonoBehaviour
{
    public GameObject[] cubePrefabs;
    public PhotonView photonView;
    private void Start()
    {
        /*var views = FindObjectsOfType<PhotonView>();
        foreach (var view in views)
        {
            if (view.IsMine)
            {
                photonView = view;
            }
        }*/
        photonView = GetComponent<PhotonView>();
        Ins();
    }
    
    void Ins()
    {
        if (photonView.ViewID == 1001)
        {
            Debug.Log(photonView.ViewID);
            int randomIdx = Random.Range(0, cubePrefabs.Length);
            Debug.Log(randomIdx);

            photonView.RPC("RPC_piece", RpcTarget.All, randomIdx);
        }
        /*else
        {
            photonView.RPC("RPC_piece", RpcTarget.OthersBuffered, cubePrefabs[0]);
        }*/
        
        /*int randomIdx = Random.Range(0, cubePrefabs.Length);
        Debug.Log(randomIdx);
        photonView.RPC("RPC_piece", RpcTarget.All, randomIdx);*/
    }

    [PunRPC]
    private void RPC_piece(int random)
    {
        Instantiate(cubePrefabs[random], Vector3.zero, Quaternion.identity);

    }
}
