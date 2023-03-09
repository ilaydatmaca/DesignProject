

public class LevelGoalCollected : LevelGoal
{

    public override bool IsGameOver()
    {
        int maxScore = scoreGoals[scoreGoals.Length - 1];

        if (ScoreManager.Instance.CurrentScore >= maxScore || timeLeft <= 0 || movesLeft <= 0)
        {
            return true;
        }

        return false;
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
