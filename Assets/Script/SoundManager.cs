using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Sfx;
    public AudioSource AudioSource;
    [SerializeField] private AudioClip ARGun;
    [SerializeField] private AudioClip ManPain;
    

    public void Awake()
    {
        Sfx = this;
        DontDestroyOnLoad(this);
    }
    
    public void playARSound()
    {
        AudioSource.PlayOneShot(ARGun);
    }

    public void playManPain()
    {
        AudioSource.PlayOneShot(ManPain);
    }

    
}
