using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using UnityEngine;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int roomSize;
    public void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);

    }

    public void JoinRoom()
    {
        CreateRoom();
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("deneme");

    }
    
}
