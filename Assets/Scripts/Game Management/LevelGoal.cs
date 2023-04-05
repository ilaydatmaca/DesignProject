
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public GameObject losingPopup;

    public bool IsGameOver()
    {
        if (RoundManager.Instance.roundNumber == RoundManager.Instance.currentRoundNumber)
        {
            return true;
        }
        return false;
    }

    public void Restart()
    {
        losingPopup.SetActive(false);
        SceneManager.LoadScene("StartMenu");
    }

    public bool IsWinner()
    {
        if (ScoreManager.Instance.currentScorePlayer1 > ScoreManager.Instance.currentScorePlayer2)
        {
            if (RoundManager.Instance.player1View.IsMine)
            {
                return true;
            }
            return false;
        }
        if (ScoreManager.Instance.currentScorePlayer2 > ScoreManager.Instance.currentScorePlayer1)
        {
            if (RoundManager.Instance.player2View.IsMine)
            {
                return true;
            }
            return false;
        }
        return false;
    }


}
