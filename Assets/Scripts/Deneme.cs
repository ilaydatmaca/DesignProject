using Photon.Pun;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    public GameObject[] cubePrefabs;
    public PhotonView photonView;
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
            
        var views = FindObjectsOfType<PhotonView>();
        foreach (var view in views)
        {
            if (view.IsMine)
            {
                photonView = view;
            }
        }
        
        if (!photonView.IsMine)
            return;
        Ins();
    }
    
    void Ins()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomIdx = Random.Range(0, cubePrefabs.Length);
            Debug.Log(randomIdx);

            photonView.RPC("RPC_piece", RpcTarget.All, randomIdx);
        }
        


    }

    [PunRPC]
    private void RPC_piece(int random)
    {
        Instantiate(cubePrefabs[random], Vector3.zero, Quaternion.identity);

    }
}
