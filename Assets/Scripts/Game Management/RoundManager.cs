using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    public int roundNumber = 5;
    public int currentRoundNumber;

    public TMP_Text roundText;
    public TMP_Text yourTurnText;
    
    [HideInInspector] public PhotonView player1View;
    [HideInInspector] public PhotonView player2View;
    [HideInInspector] public PhotonView turnView;
    
    
    public void InitRound()
    {
        turnView = player1View;
        
        IncreaseRoundNumber();
        SetStateTurnText();
        TimeManager.Instance.ResetTimer();
        MovesManager.Instance.InitMoves();
    }

    public void SetRound()
    {
        if (currentRoundNumber >= roundNumber)
        {
            return;
        }

        if (TimeManager.Instance.isTimeUp)
        {
            if (turnView == player1View)
            {
                turnView = player2View;
            }
            else if (turnView == player2View)
            {
                turnView = player1View;
                IncreaseRoundNumber();
            }
            SetStateTurnText();
            MovesManager.Instance.InitMoves();
        }
    }

    private void IncreaseRoundNumber()
    {
        currentRoundNumber++;
        SetRoundText();
    }
    private void SetRoundText()
    {
        roundText.text = "ROUND " + currentRoundNumber;
    }

    private void SetStateTurnText()
    {
        yourTurnText.enabled = turnView.IsMine;
    }
}
