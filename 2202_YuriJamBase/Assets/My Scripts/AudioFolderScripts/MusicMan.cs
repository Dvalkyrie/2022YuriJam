using System;
using System.Collections;
using UnityEngine;

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
        //Debug.Log("hello!");
        Play("DefaultTheme");
        Play("BakerTheme");
        Play("ChefTheme");

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

    public void Update()
    {
        StopAllCoroutines();
        StartCoroutine(Fade());
    }
    private IEnumerator Fade()
    {
        float timeEl = 0; //time elapsed
        float TTF = 0.75f; //time to fade
        Sound s = Array.Find(sounds, sound => sound.name == "DefaultTheme");
        Sound s2 = Array.Find(sounds, sound => sound.name == "ChefTheme");
        Sound s3 = Array.Find(sounds, sound => sound.name == "BakerTheme");
        // 1, 0 means it goes from 0 to 1
        // 0, 1 means it goes from 1 to 0
        if (SaveScript.Player1Health == SaveScript.Player2Health)
        {
            while (timeEl < TTF)
            {
                
                s.source.volume = Mathf.Lerp(1, 0, timeEl / TTF);
                timeEl += Time.deltaTime;

                s2.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);
                timeEl += Time.deltaTime;

                s3.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);
                timeEl += Time.deltaTime;
                yield return null;
            }
        }
        else if (SaveScript.Player1Health > SaveScript.Player2Health)
        {
            while (timeEl < TTF)
            {
                s.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);
                timeEl += Time.deltaTime;

                s2.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);
                timeEl += Time.deltaTime;

                s3.source.volume = Mathf.Lerp(1, 0, timeEl / TTF);
                timeEl += Time.deltaTime;
                yield return null;
            }
        }

        else if (SaveScript.Player1Health < SaveScript.Player2Health)
        {
            while (timeEl < TTF)
            {
                s.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);
                timeEl += Time.deltaTime;

                s2.source.volume = Mathf.Lerp(1, 0, timeEl / TTF);
                timeEl += Time.deltaTime;

                s3.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);
                timeEl += Time.deltaTime;
                yield return null;
            }
        }

    }
}

