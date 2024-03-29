using TMPro;

public class CollectionGoal : Singleton<CollectionGoal>
{
    private int _countGoalPlayer1;
    private int _countGoalPlayer2;

    private MatchValue _matchValue1 = MatchValue.Blue;
    private MatchValue _matchValue2 = MatchValue.Red;

    public int boosterCount;

    public TMP_Text player1GoalText;
    public TMP_Text player2GoalText;
    
    
    public void UpdateGoal(MatchValue matchValue)
    {
        if (RoundManager.Instance.turnView == RoundManager.Instance.player1View && matchValue == _matchValue1)
        {
            _countGoalPlayer1++;
            
            if (_countGoalPlayer1 == 6)
            {
                _countGoalPlayer1 = 0;
                if (RoundManager.Instance.turnView.IsMine)
                {
                    boosterCount++;
                }
            }

        }
        else if (RoundManager.Instance.turnView == RoundManager.Instance.player2View && matchValue == _matchValue2)
        {
            _countGoalPlayer2++;
            
            if (_countGoalPlayer2 == 6)
            {
                _countGoalPlayer2 = 0;
                if (RoundManager.Instance.turnView.IsMine)
                {
                    boosterCount++;
                }
            }

        }
        UpdateGoalText();
        
    }

    private void UpdateGoalText()
    {
        if (player1GoalText != null)
        {
            player1GoalText.text = _countGoalPlayer1 + " / 6";
        }
        if (player2GoalText != null)
        {
            player2GoalText.text = _countGoalPlayer2 + " / 6";
        }
    }
}
