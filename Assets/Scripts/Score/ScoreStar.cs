using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreStar : MonoBehaviour
{
    // when complete the stage
    public Image star;
    public ParticlePlayer starFX;
    public AudioClip starSound;
    public float delay = 0.5f;


    // have we been activated already?
    public bool activated = false;


    void Start()
    {
        SetActive(false);
    }

    // turn the icon on or off
    void SetActive(bool state)
    {
        if (star != null)
        {
            star.gameObject.SetActive(state);
        }
    }

    // activate the star
    public void Activate()
    {
        // only activate once
        if (activated)
        {
            return;
        }
        StartCoroutine(ActivateRoutine());
    }

    IEnumerator ActivateRoutine()
    {
        activated = true;

        if (starFX != null)
        {
            starFX.Play();
        }

        // play the starSound
        if (SoundManager.Instance != null && starSound != null)
        {
            SoundManager.Instance.PlayClipAtPoint(starSound, Vector3.zero, SoundManager.Instance.fxVolume);
        }

        yield return new WaitForSeconds(delay);
        SetActive(true);
    }

}
