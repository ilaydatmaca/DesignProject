using System.Collections;
using UnityEngine;

public enum LevelCounter
{
    Timer,
    Moves
}

// class is abstract, use a subclass and re-define the abstract methods
public abstract class LevelGoal : Singleton<LevelGoal>
{
    public int scoreStars;
    public int[] scoreGoals;
    
    public int movesLeft;

    public int timeLeft;

    public LevelCounter levelCounter;

    int _maxTime;

    public virtual void Start()
    {
        scoreStars = 0;
        
        if (levelCounter == LevelCounter.Timer)
        {
            _maxTime = timeLeft;

            if (UIManager.Instance != null && UIManager.Instance.timer != null)
            {
                UIManager.Instance.timer.InitTimer(timeLeft);
            }
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
        
    // abstract methods to be re-defined in subclass
    public abstract bool IsWinner();
    public abstract bool IsGameOver();


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

}
