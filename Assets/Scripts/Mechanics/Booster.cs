using Photon.Pun;
using TMPro;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public int bonusTime = 15;
    public TMP_Text amountText;
    public int boosterCount = 1;
    
    private Board _board;

    public GameObject particle;

    void Awake()
    {
        _board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void SetStateParticle(bool state)
    {
        particle.SetActive(state);
    }
    
    public void DropColorBomb()
    {
        if (BoosterCell.Instance.targetCell != null && CollectionGoal.Instance.boosterCount > 0)
        {
            _board.photonView.RPC("RPC_MakeColorBombBooster", RpcTarget.AllBuffered, BoosterCell.Instance.targetCell.xIndex, BoosterCell.Instance.targetCell.yIndex);
            CollectionGoal.Instance.boosterCount--;
            SetStateParticle(false);
        }
    }
    
    public void RocketBooster()
    {
        if (BoosterCell.Instance.targetCell  != null && CollectionGoal.Instance.boosterCount > 0)
        {
            _board.photonView.RPC("RPC_RemoveColumn", RpcTarget.AllBuffered, BoosterCell.Instance.targetCell .xIndex);
            CollectionGoal.Instance.boosterCount--;
            SetStateParticle(false);
        }
    }
    public void ShuffleBoard()
    {
        if (boosterCount > 0 && RoundManager.Instance.turnView.IsMine)
        {
            _board.photonView.RPC("RPC_Shuffle", RpcTarget.AllBuffered);
            UpdateText();
        }
    }
    
    public void RemoveOneGamePiece()
    {
        if (BoosterCell.Instance.targetCell  != null && boosterCount > 0)
        {
            _board.photonView.RPC("RPC_RemoveOneGamePiece", RpcTarget.AllBuffered, BoosterCell.Instance.targetCell.xIndex, BoosterCell.Instance.targetCell.yIndex);
            UpdateText();
            SetStateParticle(false);
        }
    }

    
    public void AddTime()
    {
        if (RoundManager.Instance.turnView.IsMine && boosterCount > 0)
        {
            _board.photonView.RPC("RPC_AddTime", RpcTarget.AllBuffered, bonusTime);
            UpdateText();
        }
    }

    private void UpdateText()
    {
        boosterCount--;
        amountText.text = boosterCount.ToString();
    }
}
