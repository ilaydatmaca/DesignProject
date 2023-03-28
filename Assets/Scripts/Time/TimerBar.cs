
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    private Slider _slider;

    public float maxTime;
    private bool _stopTimer;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Start()
    {
        _stopTimer = true;
        _slider.maxValue = maxTime;
        _slider.value = maxTime;

    }

    void StartCountDown()
    {
        _stopTimer = false;
        UpdateBar();
    }
    void UpdateBar()
    {
        if (_stopTimer)
        {
            return;
        }
        float time = maxTime - Time.time;

        if (time <= 0)
        {
            _stopTimer = true;
        }
        else
        {
            _slider.value = time;
            
        }
    }
}
