using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("We are now connected to the  " + PhotonNetwork.CloudRegion + " server!");
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Scenes/DelayStartWaitingRoom");
    }
}
