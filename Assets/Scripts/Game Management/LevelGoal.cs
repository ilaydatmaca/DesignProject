
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
            
    public bool IsGameOver()
    {
        if (ScoreManager.Instance.CurrentScore >= ScoreManager.Instance.maxScore || TimeManager.Instance.currentTime <= 0 || MovesManager.Instance.movesLeft <= 0)
        {
            return true;
        }

        return false;
    }

    public bool IsWinner()
    {
        if (ScoreManager.Instance != null)
        {
            return (ScoreManager.Instance.CurrentScore >= ScoreManager.Instance.maxScore);
        }
        return false;
    }


}
