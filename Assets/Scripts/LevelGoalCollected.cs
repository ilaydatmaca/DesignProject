using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalCollected : LevelGoal
{

    public override bool IsGameOver()
    {
        if (levelCounter == LevelCounter.Timer)
        {
            return (timeLeft <= 0);
        }
        else
        {
            return (movesLeft <= 0);
        }
    }

    public override bool IsWinner()
    {
        if (ScoreManager.Instance != null)
        {
            return (ScoreManager.Instance.CurrentScore >= scoreGoals[0]);
        }
        return false;
    }
}
