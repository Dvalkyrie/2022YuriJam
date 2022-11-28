using System;
using System.Collections;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    public Sound[] sounds;
    // private float timeEl = 0; //time elapsed

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
        float timeEl = 0;
        float TTF = 1.0f; //time to fade
        float changeInVolume = 0.01f;
        string p1Sound = "ChefTheme";
        string p2Sound = "BakerTheme";

        if ((SaveScript.P1Select != null) && (SaveScript.P1Select.Contains("Bea")))
        {
            p1Sound = "BakerTheme";
            p2Sound = "ChefTheme";
        }

        Sound s = Array.Find(sounds, sound => sound.name == "DefaultTheme");
        Sound s2 = Array.Find(sounds, sound => sound.name == p1Sound);
        Sound s3 = Array.Find(sounds, sound => sound.name == p2Sound);
        // 1, 0 means it goes from 0 to 1
        // 0, 1 means it goes from 1 to 0
        if (SaveScript.Player1Health == SaveScript.Player2Health)
        {
            // if volumes are good, don't bother changing it.
            // if(s.source.volume < 0.95f || s2.source.volume > 0.05f || s3.source.volume > 0.05f)
            // {
                while (timeEl < TTF)
                {
                    // changes volumes option 1
                    // s.source.volume = Mathf.Lerp(1, 0, timeEl / TTF);

                    // s2.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);

                    // s3.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);

                    // changes volumes option 2
                    if(s.source.volume < 0.95f){
                        s.source.volume += changeInVolume;
                    }
                    if(s2.source.volume > 0.05f){
                        s2.source.volume -= changeInVolume;
                    }
                    if(s3.source.volume > 0.05f){
                        s3.source.volume -= changeInVolume;
                    }
                    timeEl += Time.deltaTime;
                    // Debug.Log("Time elapsed: " + timeEl);
                    // Debug.Log("Equals Default at " + s.source.volume + 
                    //             " Chef at " + s2.source.volume+ 
                    //             " Baker at " + s3.source.volume);
                    yield return null;
                }
            // }
        }
        else if (SaveScript.Player1Health > SaveScript.Player2Health)
        {
            // if(s.source.volume > 0.05f || s2.source.volume > 0.05f || s3.source.volume < 0.95f)
            // {
                while (timeEl < TTF)
                {
                    // changes volumes option 1
                    // s.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);

                    // s2.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);

                    // s3.source.volume = Mathf.Lerp(1, 0, timeEl / TTF);
                    
                    // changes volumes option 2
                    if(s.source.volume > 0.05f)
                    {
                        s.source.volume -= changeInVolume;}
                    if(s2.source.volume > 0.05f)
                    {
                        s2.source.volume -= changeInVolume;}
                    if(s3.source.volume < 0.95f)
                    {
                        s3.source.volume += changeInVolume;}
                    
                    timeEl += Time.deltaTime;
                    // Debug.Log("Time elapsed: " + timeEl);
                    // Debug.Log("MioWin Default at " + s.source.volume + 
                    //             " Chef at " + s2.source.volume+ 
                    //             " Baker at " + s3.source.volume);
                    yield return null;
                }
            // }
            
        }

        else if (SaveScript.Player1Health < SaveScript.Player2Health)
        {
            // if(s.source.volume > 0.1f || s2.source.volume < 0.9f || s3.source.volume > 0.1f)
            // {
            
                while (timeEl < TTF)
                {
                    // changes volumes option 1
                    // s.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);

                    // s2.source.volume = Mathf.Lerp(1, 0, timeEl / TTF);

                    // s3.source.volume = Mathf.Lerp(0, 1, timeEl / TTF);

                    // changes volumes option 2
                    if(s.source.volume > 0.05f)
                    {
                        s.source.volume -= changeInVolume;
                    }
                    if(s2.source.volume < 0.95f)
                    {
                        s2.source.volume += changeInVolume;}
                    if(s3.source.volume > 0.05f)
                    {
                        s3.source.volume -= changeInVolume;}
                    timeEl += Time.deltaTime;
                    // Debug.Log("Beatriz Win Default at " + s.source.volume + 
                    //             " Chef at " + s2.source.volume+ 
                    //             " Baker at " + s3.source.volume);
                    yield return null;
                }
            // }
        }
        

    }
}

