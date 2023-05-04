using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    public int roundNumber;
    [HideInInspector] public int currentRoundNumber;

    public TMP_Text roundText;
    public TMP_Text yourTurnText;
    
    [HideInInspector] public PhotonView player1View;
    [HideInInspector] public PhotonView player2View;
    [HideInInspector] public PhotonView turnView;

    [HideInInspector] public bool roundComplete;

    public float waitTimeForRounds;

    public TMP_Text user1Text;
    public TMP_Text user2Text;

    public void InitRound()
    {
        SetYouText();
        IncreaseRoundNumber();
        
        SetTurnText();
        SetStateTurnText(true);
        SetStateRoundText(true);
    }

    public void CheckAllRoundsComplete()
    {
        if (currentRoundNumber == roundNumber && turnView == player2View)
        {
            roundComplete = true;
            SetStateTurnText(false);
            SetStateRoundText(false);

        }
    }
    public void UpdateRound()
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
        SetTurnText();
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

    private void SetStateRoundText(bool state)
    {
        roundText.enabled = state;
    }
    
    
    private void SetTurnText()
    {
        if (turnView.IsMine)
        {
            yourTurnText.text = "Your Turn";
        }
        else
        {
            yourTurnText.text = "The Opponent's Turn";
        }
    }

    private void SetStateTurnText(bool state)
    {
        yourTurnText.enabled = state;

    }

    private void SetYouText()
    {
        if (player1View.IsMine)
        {
            user1Text.text = "You";
            user2Text.text = "User123";
        }
        else
        {
            user2Text.text = "You";
            user1Text.text = "User123";
        }
    }
}
