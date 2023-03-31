
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
            
    public bool IsGameOver()
    {
        /*if (TimeManager.Instance.currentTime <= 0 || MovesManager.Instance.movesLeft <= 0)
        {
            return true;
        }

        return false;*/
        return false;
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
