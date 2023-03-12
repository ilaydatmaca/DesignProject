using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Timer timer;

    public Text timeLeftText;

    public int currentTime;
    public int maxTime;
    private void Start()
    {
        currentTime = maxTime;
        EnableTimerArea();
        timer.InitTimer(currentTime);
    }
    
    void EnableTimerArea()
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(true);
        }
    }
    
    
    /*void UpdateTimeLeftText()
    {
        if (timeLeftText != null)
        {
            timeLeftText.text = timeLeftText.movesLeft.ToString();
        }
    }*/
    
    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    // decrement the timeLeft each second
    IEnumerator CountdownRoutine()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            timer.UpdateTimer(currentTime);
        }
    }

    public void AddTime(int timeValue)
    {
        currentTime += timeValue;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);

        timer.UpdateTimer(currentTime);
    }

}
