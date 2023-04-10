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
    
    public void InitRound()
    {
        turnView = player1View;
        
        IncreaseRoundNumber();
        EnableRoundText();
        Set();
        EnableTurnStateText();

    }

    public void SetRound()
    {
        if (currentRoundNumber > roundNumber)
        {
            return;
        }
        
        if (turnView == player1View)
        {
            turnView = player2View;
        }
        else if (turnView == player2View)
        {
            turnView = player1View;
            IncreaseRoundNumber();
        }
        Set();
    }
    
    void Set()
    {
        SetTurnStateText();
        
        TimeManager.Instance.ResetTimer();
        MovesManager.Instance.Init();
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

    private void EnableRoundText()
    {
        roundText.enabled = true;
    }
    
    private void SetTurnStateText()
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

    private void EnableTurnStateText()
    {
        yourTurnText.enabled = true;

    }
}
