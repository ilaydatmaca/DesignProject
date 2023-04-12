using Photon.Pun;
using TMPro;

public class RoundManager : Singleton<RoundManager>
{
    public int roundNumber = 5;
    public int currentRoundNumber;

    public TMP_Text roundText;
    public TMP_Text yourTurnText;
    
    public PhotonView player1View;
    public PhotonView player2View;
    public PhotonView turnView;

    public bool roundComplete;

    public float waitTimeForRounds = 2f;
    public void InitRound()
    {
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
}
