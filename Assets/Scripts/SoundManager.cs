using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip[] musicClips;
    public AudioClip[] winClips;
    public AudioClip[] loseClips;
    public AudioClip[] bonusClips;

    [Range(0, 1)] public float musicVolume = 0.5f;

    [Range(0, 1)] public float fxVolume = 1.0f;

    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

   void Start()
    {
        PlayRandomMusic();
    }

   public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
   {
       if (clip != null)
       {
           GameObject go = new GameObject("SoundFX" + clip.name);
           go.transform.position = position;

           AudioSource source = go.AddComponent<AudioSource>();
           source.clip = clip;

           float randomPitch = Random.Range(lowPitch, highPitch);
           source.pitch = randomPitch;

           source.volume = volume;
           
           source.Play();
           Destroy(go, clip.length);
           return source;
       }

       return null;
   }

   public AudioSource PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1f)
   {
       if (clips != null)
       {
           if (clips.Length != 0)
           {
               int ranIndex = Random.Range(0, clips.Length);
               if (clips[ranIndex] != null)
               {
                   AudioSource source = PlayClipAtPoint(clips[ranIndex], position, volume);
               }
           }
       }

       return null;
   }

   public void PlayRandomMusic()
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
   
   public void PlayBonusSound()
   {
       PlayRandom(bonusClips, Vector3.zero, fxVolume);
   }

}
