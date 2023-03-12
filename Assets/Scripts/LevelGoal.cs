using System.Collections;
using UnityEngine;


public class LevelGoal : Singleton<LevelGoal>
{
    public int scoreStars;
    public int[] scoreGoals;
    
    public int movesLeft;

    public int timeLeft;

    int _maxTime;

    public virtual void Start()
    {
        scoreStars = 0;
        _maxTime = timeLeft;

        if (UIManager.Instance != null && UIManager.Instance.timer != null)
        {
            UIManager.Instance.timer.InitTimer(timeLeft);
        }
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


    // public method to start the timer
    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    // decrement the timeLeft each second
    IEnumerator CountdownRoutine()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;

            if (UIManager.Instance != null && UIManager.Instance.timer != null)
            {
                UIManager.Instance.timer.UpdateTimer(timeLeft);
            }
        }
    }

    public void AddTime(int timeValue)
    {
        timeLeft += timeValue;
        timeLeft = Mathf.Clamp(timeLeft, 0, _maxTime);

        if (UIManager.Instance != null && UIManager.Instance.timer != null)
        {
            UIManager.Instance.timer.UpdateTimer(timeLeft);
        }
    }
    
            
    public bool IsGameOver()
    {
        int maxScore = scoreGoals[scoreGoals.Length - 1];

        if (ScoreManager.Instance.CurrentScore >= maxScore || timeLeft <= 0 || movesLeft <= 0)
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
