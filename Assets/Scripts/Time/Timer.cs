using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool paused = true;
    
    private TimeManager _timeManager;
    public Slider _slider;

    private void Awake()
    {
        _timeManager = GetComponent<TimeManager>();
    }

    public void InitTimer()
    {
        
        _slider.maxValue = _timeManager.maxTime;
        _slider.value = _timeManager.maxTime;
    }

    public void Update()
    {
        if (paused)
        {
            return;
        }
        _timeManager.currentTime -= Time.deltaTime;

        if (_timeManager.currentTime <= 0)
        {
            paused = true;
        }
        if( paused == false)
        {
            _slider.value = _timeManager.currentTime;
            
        }

    }
    

    // Zaman dolduğunda flashi durdur ve win ya da lose screen göster
    /*public void FadeOff()
    {
        ScreenFader[] screenFaders = GetComponentsInChildren<ScreenFader>();
        foreach (ScreenFader fader in screenFaders)
        {
            fader.FadeOff();
        }
    }*/
}
