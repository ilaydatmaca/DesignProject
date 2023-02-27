using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    public float solidAlpha = 1f;

    public float clearAlpha = 0f;

    public float delay = 0f;
    public float timeToFade = 1f;

    MaskableGraphic m_graphic;
    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        //FadeOff();
    }

    IEnumerator FadeRoutine(float alpha)
    {
        yield return new WaitForSeconds(delay);
        m_graphic.CrossFadeAlpha(alpha, timeToFade, true);
    }

    public void FadeOn()
    {
        StartCoroutine(FadeRoutine(solidAlpha));
    }

    public void FadeOff()
    {
        StartCoroutine(FadeRoutine(clearAlpha));
    }
    
}
