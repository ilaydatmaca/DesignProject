using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    //TestConnnect
    public MasterManager MyMaster;
    private void Start()
    {
        print("Connecting to server");
        PhotonNetwork.NickName = MyMaster._gameSettings.NickName;
        PhotonNetwork.GameVersion = MyMaster._gameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the  " + PhotonNetwork.CloudRegion + " server!");
        print(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("disconnect: " + cause.ToString());
    }
}
