using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public int maxTime;

    private float _currentTime;
    
    public bool paused = true;
    public bool isTimeUp = false;

    public Slider slider;

    private RoundManager _roundManager;

    public override void Awake()
    {
        base.Awake();
        _roundManager = FindObjectOfType<GameManager>().GetComponent<RoundManager>();
    }
    
    public void Update()
    {
        if (paused || isTimeUp)
        {
            return;
        }
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            isTimeUp = true;
            _roundManager.SetRound();
            ResetTimer();
        }
        if (!isTimeUp)
        {
            slider.value = _currentTime;
        }

    }
    public void AddTime(int timeValue)
    {
        _currentTime += timeValue;
        _currentTime = Mathf.Clamp(_currentTime, 0, maxTime);
    }

    public void ResetTimer()
    {
        StartCoroutine(ResetTimerRoutine());
    }

    IEnumerator ResetTimerRoutine()
    {
        yield return new WaitForSeconds(1f);
        
        //Reset slider
        _currentTime = maxTime;
        slider.maxValue = maxTime;
        slider.value = maxTime;
        
        isTimeUp = false;
        paused = false;
    }
    
}
