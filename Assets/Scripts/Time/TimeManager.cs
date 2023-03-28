
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public Timer timer;

    public Text timeLeftText;

    public float currentTime;
    public int maxTime;

    private void Start()
    {
        currentTime = maxTime;
        EnableTimerArea();
        timer.InitTimer();
        UpdateTimeLeftText();
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
    public void UpdateTimeLeftText()
    {
        if (timeLeftText != null)
        {
            timeLeftText.text = ((int)currentTime).ToString();
        }
    }

    public void AddTime(int timeValue)
    {
        currentTime += timeValue;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);

        //timer.UpdateTimer();
    }

}
