using TMPro;
using UnityEngine;

public class CollectionGoal : MonoBehaviour
{
    private int _countGoal = 6;

    private MatchValue _matchValue;

    public bool canUseBooster;

    public TMP_Text goalText;
    private void Start()
    {
        if (RoundManager.Instance.player1View.IsMine)
        {
            _matchValue = MatchValue.Blue;
        }
        else if (RoundManager.Instance.player2View.IsMine)
        {
            _matchValue = MatchValue.Red;
        }
    }

    private void UpdateGoal(MatchValue matchValue)
    {
        if (RoundManager.Instance.turnView.IsMine && matchValue == _matchValue)
        {
            _countGoal--;

            if (_countGoal == 0)
            {
                canUseBooster = true;
                _countGoal = 6;
            }
            UpdateGoalText();
        }
    }

    private void UpdateGoalText()
    {
        goalText.text = _countGoal.ToString();
    }
}
