using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PhotonView photonView;
    private ItemFactory itemFactory;
    private SwapManager _swapManager;
    private Board _board;
    private ClearManager _clearManager;
    private void Awake()
    {
        _swapManager = FindObjectOfType<Board>().GetComponent<SwapManager>();
        itemFactory = FindObjectOfType<Board>().GetComponent<ItemFactory>();
        _board = FindObjectOfType<Board>().GetComponent<Board>();
        _clearManager = FindObjectOfType<Board>().GetComponent<ClearManager>();

    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
            
        var views = FindObjectsOfType<PhotonView>();
        foreach (var view in views)
        {
            if (view.IsMine)
            {
                photonView = view;
                itemFactory.photonView = photonView;
                _swapManager.photonView = photonView;
                _board.photonView = photonView;

            }
        }
        
        if (!photonView.IsMine)
            return;
    }
    
    [PunRPC]
    public void RPC_InitGameObject(int index, int x, int y)
    {
        itemFactory.InitGameObject(index, x, y);
    }
    
    [PunRPC]
    public void RPC_SwapCells(int cell1X, int cell1Y, int cell2X, int cell2Y)
    {
        _swapManager.SwapCells(cell1X, cell1Y, cell2X, cell2Y);
    }
    
    [PunRPC]
    public void RPC_DestroyAt(int x, int y)
    {
        _clearManager.DestroyAt(x, y);
    }
    
}
