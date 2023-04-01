using Photon.Pun;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PhotonView photonView;
    private ItemFactory _itemFactory;
    private SwapManager _swapManager;
    private Board _board;
    private ClearManager _clearManager;
    private void Awake()
    {
        _swapManager = FindObjectOfType<Board>().GetComponent<SwapManager>();
        _itemFactory = FindObjectOfType<Board>().GetComponent<ItemFactory>();
        _board = FindObjectOfType<Board>().GetComponent<Board>();
        _clearManager = FindObjectOfType<Board>().GetComponent<ClearManager>();

    }

    private void Start()
    {

        photonView = GetComponent<PhotonView>();
            
        if (!photonView.IsMine || photonView == null)
            return;
        _itemFactory.photonView = photonView;
        _swapManager.photonView = photonView;
        _board.photonView = photonView;
    }
    
    [PunRPC]
    public void RPC_InitGameObject(int index, int x, int y)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _itemFactory.InitGameObject(index, x, y);
    }
    
    [PunRPC]
    public void RPC_SwapCells(int cell1X, int cell1Y, int cell2X, int cell2Y)
    {
        _swapManager.SwapCells(cell1X, cell1Y, cell2X, cell2Y);
    }


}
