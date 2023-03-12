using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image clockImage;

    public int flashTimeLimit = 10; // kırmızı uyarının başlama zamanı
    public float flashInterval = 1f; //flash uyarasını her 1 saniyede bir
    
    public AudioClip flashBeep;
    
    public Color flashColor = Color.red; // the color of flash warning
    
    public bool paused;
    
    private TimeManager _timeManager;

    private void Awake()
    {
        _timeManager = GetComponent<TimeManager>();
    }

    public void InitTimer()
    {
        // make sure the image is using Radial360 fillMethod with origin at top
        if (clockImage != null)
        {
            clockImage.type = Image.Type.Filled;
            clockImage.fillMethod = Image.FillMethod.Radial360;
            clockImage.fillOrigin = (int) Image.Origin360.Top;
        }

    }

    public void UpdateTimer()
    {
        if (clockImage != null && !paused)
        {

            clockImage.fillAmount = (float) _timeManager.currentTime / (float) _timeManager.maxTime;

            if (_timeManager.currentTime <= flashTimeLimit)
            {
                StartCoroutine(FlashRoutine());

                if (SoundManager.Instance != null && flashBeep != null)
                {
                    SoundManager.Instance.PlayClipAtPoint(flashBeep, Vector3.zero, SoundManager.Instance.fxVolume, false);
                }
            }
        }

        _timeManager.UpdateTimeLeftText();

    }
  
    IEnumerator FlashRoutine()
    {
        if (clockImage != null)
        {
            Color originalColor = clockImage.color;
            clockImage.CrossFadeColor(flashColor, flashInterval * 0.3f, true, true);
            yield return new WaitForSeconds(flashInterval * 0.5f);

            clockImage.CrossFadeColor(originalColor, flashInterval * 0.3f, true, true);
            yield return new WaitForSeconds(flashInterval * 0.5f);
        }
    }
    

    // Zaman dolduğunda flashi durdur ve win ya da lose screen göster
    public void FadeOff()
    {
        StopCoroutine(FlashRoutine());

        ScreenFader[] screenFaders = GetComponentsInChildren<ScreenFader>();
        foreach (ScreenFader fader in screenFaders)
        {
            fader.FadeOff();
        }
    }
}
