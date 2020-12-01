using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance() 
    {
        return instance;
    }

    public Transform obstacle;

    public Transform crystall;

    public Transform backGround;

    public SoundAudioClip[] clips;

    private void Awake() 
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    [Serializable]
    public class SoundAudioClip 
    {
        public SoundManager.Sounds sounds;
        public AudioClip clip;
    }
}
