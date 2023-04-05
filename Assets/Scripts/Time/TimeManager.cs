using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public Timer timer;

    public float currentTime;
    public int maxTime;
    
    public bool paused = true;
    public bool isTimeUp = false;

    public Slider slider;

    private RoundManager _roundManager;

    public override void Awake()
    {
        base.Awake();
        _roundManager = FindObjectOfType<GameManager>().GetComponent<RoundManager>();
    }

    private void Start()
    {
        EnableTimerArea();
    }
    
    
    void EnableTimerArea()
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(true);
        }
    }

    void InitTimer()
    {
        currentTime = maxTime;
        slider.maxValue = maxTime;
        slider.value = maxTime;
        isTimeUp = false;
    }
    
    public void Update()
    {
        if (paused || isTimeUp)
        {
            return;
        }
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            isTimeUp = true;
            _roundManager.SetRound();
        }
        if (!isTimeUp)
        {
            slider.value = currentTime;
        }

    }
    
    void StartCountDown()
    {
        paused = false;
    }

    public void AddTime(int timeValue)
    {
        currentTime += timeValue;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);
    }

    public void ResetTimer()
    {
        StartCoroutine(ResetTimerRoutine());
    }

    IEnumerator ResetTimerRoutine()
    {
        yield return new WaitForSeconds(1f);
        
        InitTimer();
        StartCountDown();

    }



}
