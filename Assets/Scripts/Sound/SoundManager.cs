using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public AudioClip musicClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    [Range(0,1)]
    public float musicVolume = 0.5f;

    [Range(0,1)]    // sound effects volume
    public float fxVolume = 1.0f;

    // boundaries for random variation in pitch
    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

	void Start () 
    {
        PlayRandomMusic();
	}
    
    public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool randomizePitch = true, bool loop = false)
    {
        if (clip != null)
        {
            GameObject go = new GameObject("SoundFX" + clip.name);
            go.transform.position = position;

            // add an AudioSource component and set the AudioClip
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;

            // change the pitch of the sound within some variation
            if (randomizePitch)
            {
                float randomPitch = Random.Range(lowPitch, highPitch);
                source.pitch = randomPitch;
            }

            source.volume = volume;
            source.loop = loop;

            source.Play();

            if (loop)
            {
                if (GameManager.Instance.IsGameOver)
                {
                    Destroy(go);
                }
            }
            
        }
    }

    void PlayRandomMusic()
    {
        PlayClipAtPoint(musicClip, Vector3.zero, musicVolume, true, true);
    }

    public void PlayWinSound()
    {
        PlayClipAtPoint(winClip, Vector3.zero, fxVolume);
    }

    public void PlayLoseSound()
    {
        PlayClipAtPoint(loseClip, Vector3.zero, fxVolume * 0.5f);
    }


}
