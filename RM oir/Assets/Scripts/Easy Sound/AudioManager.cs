using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;
   
    [Space]

    public AudioClip BackgroundSound;

    public void Awake()
    {if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        
        MusicSource.clip = BackgroundSound;
        MusicSource.Play();
    }
}
