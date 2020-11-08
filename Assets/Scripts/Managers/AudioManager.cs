using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource[] sources;


    public void PlaySound(int soundId)
    {
        sources[soundId].Play();
    }   
    
    public void StopAll()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].Stop();
        }
    }

    public void StopSound(int soundId)
    {
        sources[soundId].Stop();
    }

}
