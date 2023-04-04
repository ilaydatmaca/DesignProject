using Photon.Pun;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PhotonView photonView;
    private ItemFactory _itemFactory;
    private SwapManager _swapManager;
    private Board _board;
    private ShuffleManager _shuffleManager;
    private void Awake()
    {
        _swapManager = FindObjectOfType<Board>().GetComponent<SwapManager>();
        _itemFactory = FindObjectOfType<Board>().GetComponent<ItemFactory>();
        _board = FindObjectOfType<Board>().GetComponent<Board>();
        _shuffleManager = FindObjectOfType<Board>().GetComponent<ShuffleManager>();
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        photonView = GetComponent<PhotonView>();
            
        if (!photonView.IsMine || photonView == null)
            return;
        _swapManager.photonView = photonView;
        _board.photonView = photonView;
    }
    
    [PunRPC]
    public void RPC_InitGameObject(int index, int x, int y)
    {
        _itemFactory.InitGameObject(index, x, y);
    }
    
    [PunRPC]
    public void RPC_SwapCells(int cell1X, int cell1Y, int cell2X, int cell2Y)
    {
        _swapManager.SwapCells(cell1X, cell1Y, cell2X, cell2Y);
    }
    
    [PunRPC]
    public void RPC_Shuffle()
    {
        _shuffleManager.ShuffleBoard();
    }
    [PunRPC]
    public void RPC_FillBoardFromList()
    {
        _shuffleManager.FillBoardFromList();
    }
    
    [PunRPC]
    public void RPC_MovePieces()
    {
        _shuffleManager.MovePieces();
    }
    
    [PunRPC]
    public void RPC_SwapArrayItems(int index1, int index2)
    {
        _shuffleManager.SwapArrayItems(index1, index2);
    }


}
