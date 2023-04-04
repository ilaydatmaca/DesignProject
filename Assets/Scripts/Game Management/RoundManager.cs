using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int roundNumber = 5;

    public int moveNumberPerRound = 2;

    public PhotonView player1View;
    public PhotonView player2View;

    private void Start()
    {
        player1View = PhotonView.Find(1001);
        player2View = PhotonView.Find(2001);
    }
}
