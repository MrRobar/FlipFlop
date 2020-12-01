using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource source;

    

    public enum Sounds 
    {
        Jump,
        GoThrow,
        Lose,
        Crystall,

    }

    private void Awake() 
    {
        source = GetComponent<AudioSource>();
        
    }

    public void PlaySound(Sounds sounds) 
    {
        source.PlayOneShot(GetAudioClip(sounds));
    }

    public AudioClip GetAudioClip(Sounds sounds) 
    {
        foreach(GameAssets.SoundAudioClip clip in GameAssets.GetInstance().clips) 
        {
            if(clip.sounds == sounds) 
            {
                return clip.clip;
            }
        }
        return null;
    }
}
