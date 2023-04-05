using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int roundNumber = 5;
    public int moveNumberPerRound = 2;

    public PhotonView player1View;
    public PhotonView player2View;

    public PhotonView turnView;

    public bool startRound;

    public int currentRoundNumber;

    public TMP_Text roundText;

    public TMP_Text yourTurnText;

    public void InitRound()
    {
        startRound = true;
        currentRoundNumber = 1;
        
        turnView = player1View;
        SetRoundText();
        TimeManager.Instance.ResetTimer();
    }

    public void SetRound()
    {
        if(!startRound || currentRoundNumber > roundNumber)
            return;

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
                SetRoundText();
            }
            
            TimeManager.Instance.ResetTimer();
            SetStateTurnText();
           
        }
    }

    private void IncreaseRoundNumber()
    {
        currentRoundNumber++;
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
