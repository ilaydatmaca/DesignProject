using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public Timer timer;

    public Text timeLeftText;

    public int currentTime;
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
    
    
    public void UpdateTimeLeftText()
    {
        if (timeLeftText != null)
        {
            timeLeftText.text = currentTime.ToString();
        }
    }
    
    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }


    IEnumerator CountdownRoutine()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            timer.UpdateTimer();
        }
    }

    public void AddTime(int timeValue)
    {
        currentTime += timeValue;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);

        timer.UpdateTimer();
    }

}
