using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public AudioClip[] musicClips;
    public AudioClip[] winClips;
    public AudioClip[] loseClips;

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
    
    public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool randomizePitch = true)
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

            source.Play();
            
            Destroy(go, clip.length);
            
        }
    }
    

    // play a random sound from an array of sounds
    void PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1f)
    {
        if (clips != null)
        {
            if (clips.Length != 0)
            {
                int randomIndex = Random.Range(0, clips.Length);

                if (clips[randomIndex] != null)
                {
                   PlayClipAtPoint(clips[randomIndex], position, volume);
                }
            }
        }
    }

    void PlayRandomMusic()
    {
        PlayRandom(musicClips, Vector3.zero, musicVolume);
    }

    public void PlayWinSound()
    {
        PlayRandom(winClips, Vector3.zero, fxVolume);
    }

    public void PlayLoseSound()
    {
        PlayRandom(loseClips, Vector3.zero, fxVolume * 0.5f);
    }

   /*public void PlayBonusSound()
    {
        PlayRandom(bonusClips, Vector3.zero, fxVolume);
    }*/


}
