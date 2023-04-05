
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public bool gameOver;
    public GameObject losingPopup;
    public GameObject winningPopup;

    public bool IsGameOver()
    {
        /*if (TimeManager.Instance.currentTime <= 0 || MovesManager.Instance.movesLeft <= 0)
        {
            return true;
        }

        return false;*/
        if (gameOver)
        {
            gameOver = false;
            Debug.Log("Game Ended");
            losingPopup.SetActive(true);
            

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
        if (ScoreManager.Instance != null)
        {
            return true;
        }
        return false;
    }


}
