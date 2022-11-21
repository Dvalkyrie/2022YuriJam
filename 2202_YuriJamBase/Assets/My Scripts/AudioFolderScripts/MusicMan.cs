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
        Debug.Log("hello!");
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
    private void Update()
    {
        float TE = 0; //time elapsed
        float TTF = 1.75f; //time to fade

        if (SaveScript.Player1Health == SaveScript.Player2Health)
        {
            //Debug.Log("bbam");
            Sound s = Array.Find(sounds, sound => sound.name == "DefaultTheme");
            s.source.volume = Mathf.Lerp(1, 0, TE / TTF);
            TE += Time.deltaTime;

            //Debug.Log("bbang");
            Sound s2 = Array.Find(sounds, sound => sound.name == "ChefTheme");
            s2.source.volume = Mathf.Lerp(0, 1, TE / TTF);
            TE += Time.deltaTime;

            Sound s3 = Array.Find(sounds, sound => sound.name == "BakerTheme");
            s3.source.volume = Mathf.Lerp(0, 1, TE / TTF);
            TE += Time.deltaTime;
        }
        else if (SaveScript.Player1Health > SaveScript.Player2Health)
        {
            //Debug.Log("bbam");
            Sound s = Array.Find(sounds, sound => sound.name == "DefaultTheme");
            s.source.volume = Mathf.Lerp(0, 1, TE / TTF);
            TE += Time.deltaTime;

            //Debug.Log("bbang");
            Sound s2 = Array.Find(sounds, sound => sound.name == "BakerTheme");
            s2.source.volume = Mathf.Lerp(1, 0, TE / TTF);
            TE += Time.deltaTime;

            //Debug.Log("bbang");
            Sound s3 = Array.Find(sounds, sound => sound.name == "ChefTheme");
            s3.source.volume = Mathf.Lerp(0, 1, TE / TTF);
            TE += Time.deltaTime;
        }

        else if (SaveScript.Player1Health < SaveScript.Player2Health)
        {
            //Debug.Log("bbam");
            Sound s = Array.Find(sounds, sound => sound.name == "DefaultTheme");
            s.source.volume = Mathf.Lerp(0, 1, TE / TTF);
            TE += Time.deltaTime;

            //Debug.Log("bbang");
            Sound s2 = Array.Find(sounds, sound => sound.name == "ChefTheme");
            s2.source.volume = Mathf.Lerp(1, 0, TE / TTF);
            TE += Time.deltaTime;

            Sound s3 = Array.Find(sounds, sound => sound.name == "BakerTheme");
            s3.source.volume = Mathf.Lerp(0, 1, TE / TTF);
            TE += Time.deltaTime;
        }

    }

}


