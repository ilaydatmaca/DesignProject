using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public bool IsGameOver()
    {
        if (RoundManager.Instance.roundNumber < RoundManager.Instance.currentRoundNumber)
        {
            return true;
        }
        return false;
    }

    public bool IsWinner()
    {
        bool isWin = false;
        if (ScoreManager.Instance.currentScorePlayer1 > ScoreManager.Instance.currentScorePlayer2)
        {
            if (RoundManager.Instance.player1View.IsMine)
            {
                isWin = true;
            }
        }
        else if (ScoreManager.Instance.currentScorePlayer2 > ScoreManager.Instance.currentScorePlayer1)
        {
            if (RoundManager.Instance.player2View.IsMine)
            {
                isWin = true;
            }
        }
        return isWin;
    }
    
}
