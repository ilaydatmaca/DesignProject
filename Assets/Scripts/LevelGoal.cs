
public class LevelGoal : Singleton<LevelGoal>
{
    public int scoreStars;
    public int[] scoreGoals;
    
    
    public virtual void Start()
    {
        scoreStars = 0;
    }

    // return number of stars given a score value
    public int UpdateScore(int score)
    {
        for (int i = 0; i < scoreGoals.Length; i++)
        {
            if (score < scoreGoals[i])
            {
                return i;
            }
        }
        return scoreGoals.Length;

    }

    // set scoreStars based on current score
    public void UpdateScoreStars(int score)
    {
        scoreStars = UpdateScore(score);
    }
            
    public bool IsGameOver()
    {
        int maxScore = scoreGoals[scoreGoals.Length - 1];

        if (ScoreManager.Instance.CurrentScore >= maxScore || TimeManager.Instance.currentTime <= 0 || MovesManager.Instance.movesLeft <= 0)
        {
            return true;
        }

        return false;
    }

    public bool IsWinner()
    {
        if (ScoreManager.Instance != null)
        {
            return (ScoreManager.Instance.CurrentScore >= scoreGoals[0]);
        }
        return false;
    }


}
