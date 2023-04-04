using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public Timer timer;

    public float currentTime;
    public int maxTime;

    private void Start()
    {
        currentTime = maxTime;
        EnableTimerArea();
        timer.InitTimer();
    }
    
    void EnableTimerArea()
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(true);
        }
    }
    
    public void StartCountDown()
    {
        timer.paused = false;
    }

    public void AddTime(int timeValue)
    {
        currentTime += timeValue;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);
    }

}
