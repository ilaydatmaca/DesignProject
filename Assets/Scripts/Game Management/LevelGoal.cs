using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public bool IsGameOver()
    {
        if (RoundManager.Instance.roundComplete)
        {
            return true;
        }
        return false;
    }

    public GameState IsWinner()
    {
        GameState state = GameState.Lose;
        if (ScoreManager.Instance.currentScorePlayer1 > ScoreManager.Instance.currentScorePlayer2)
        {
            if (RoundManager.Instance.player1View.IsMine)
            {
                state = GameState.Win;
            }
        }
        else if (ScoreManager.Instance.currentScorePlayer2 > ScoreManager.Instance.currentScorePlayer1)
        {
            if (RoundManager.Instance.player2View.IsMine)
            {
                state = GameState.Win;
            }
        }
        else
        {
            state = GameState.Draw;
        }
        return state;
    }
    
}
