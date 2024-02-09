using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;

public class SoundSystem : MonoBehaviour
{

    public Sound[] sounds;
    public Sound[] music;
    public static SoundSystem instance;
    public AudioSource musicSource;
    public AudioSource lastPlayedSFX;
    public List<AudioSource> soundEffectsSources;
    public bool mutesounds = false;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            soundEffectsSources.Add(s.source);

        }

        musicSource = gameObject.AddComponent<AudioSource>();

    }

    public void Start()
    {

    }

    public void PlaySound(string name)
    {



        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        else 
        {

           Debug.Log("play sound: " + s.name);

        }


        lastPlayedSFX = s.source;

        s.source.Play();

    }

    public void StopLastSound() 
    {
        if (lastPlayedSFX)
        {
            lastPlayedSFX.Stop();
        }
    }

    public void PlayMusic(string name)
    {
        musicSource.Stop();
        Sound s = Array.Find(music, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        musicSource.clip = s.clip;        
        musicSource.volume = s.volume;
        musicSource.pitch = s.pitch;
        musicSource.loop = s.loop;

        musicSource.Play();

    }

    public void StopMusic() 
    {
        musicSource.Stop();

    }

    public void PlaySoundWithRandomPitch(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        else
        {

           Debug.Log("play sound: " + s.name);

        }


        StartCoroutine(PlaySoundChangePitch(name,Random.Range(0.8f,1.2f)));

    }


    public void PlaySoundWithSpecficPitch(string name,float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        else
        {

            Debug.Log("play sound: " + s.name);

        }


        StartCoroutine(PlaySoundChangePitch(name, pitch));

    }


    public IEnumerator PlaySoundChangePitch(string name,float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        lastPlayedSFX = s.source;

        s.source.pitch = pitch;

        s.source.Play();

        yield return new WaitForSeconds(s.source.clip.length);

        s.source.pitch = 1;



    }


    public void PlayRandomSound(string[] soundNames) 
    {

        
        List<Sound> bucket = new List<Sound>();

        for (int i  =0; i < soundNames.Length; i++) 
        {
            for (int b = 0; b < sounds.Length ; b++) 
            {
                if (soundNames[i] == sounds[b].name) 
                {
                    Debug.Log(soundNames[i]);
                    bucket.Add(sounds[b]);
                  //  return;
                }
            
            }

        }
        bucket[Random.Range(0, bucket.Count)].source.Play();
    }


    public void PlayRandomSound(List<String> soundnames) 
    {
        List<Sound> bucket = new List<Sound>();
        foreach (Sound s in sounds) 
        {
            

            if (soundnames.Contains(s.name))
            {
                Debug.Log(s.name);
                bucket.Add(s);
            }
            else 
            {
                Debug.Log(s.name);
                Debug.Log("not in here");
            }
        }

        bucket[Random.Range(0, bucket.Count)].source.Play();

    }


    public void ToggleSFX(bool newState) 
    {
        for (int i = 0; i < soundEffectsSources.Count; i++)
        {
            if (newState == false)
            {
                soundEffectsSources[i].playOnAwake = false;
                soundEffectsSources[i].enabled = false;

            }
            else
            {

                soundEffectsSources[i].enabled = true;

            }

        }

    }

    public void ToggleMuteMusic(bool ChangeToThisState)
    {       
          musicSource.enabled = ChangeToThisState;

        if (musicSource.enabled)
        {
            musicSource.Play();
       
        }

    }

    public void ExtraMusicSounds() 
    {
    
    }

    public void ExtraSFXSounds()
    {
 
    }


    public void StopAllSounds() 
    {

        foreach (AudioSource s in soundEffectsSources) { s.Stop(); }

    }
}
