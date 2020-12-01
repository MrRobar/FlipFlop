using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer masterMixer;

    private void Awake() 
    {
        masterMixer.SetFloat("SFXVol", 0f);
        masterMixer.SetFloat("MusicVol", 0f);
    }

    public void SetSFXLevel(float sfxLevel) 
    {
        masterMixer.SetFloat("SFXVol", sfxLevel);
    }

    public void SetMusicLevel(float musicLevel) 
    {
        masterMixer.SetFloat("MusicVol", musicLevel);
    }
}
