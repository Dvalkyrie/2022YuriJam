using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class MusicMan : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.Output;
        }
    }
    private void Start()
    {
        Debug.Log("hello!");
        Play("DefaultTheme");
        Play("BakerTheme");
        Play("ChefTheme");

    }
    private void Update()
    {
        if (SaveScript.Player1Health > SaveScript.Player2Health)
        {
            //Debug.Log("bam");
        }
    }
    

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound: " + name + " not found!");
        }
        s.source.Play();
    }
}

