
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public bool gameOver;
    public GameObject uiCanvas;
            
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
            uiCanvas.SetActive(true);
        }
        return false;
    }

    void Restart()
    {
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
