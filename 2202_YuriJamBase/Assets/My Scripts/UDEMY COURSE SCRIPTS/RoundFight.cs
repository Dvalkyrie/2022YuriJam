using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RoundFight : MonoBehaviour
{
    public GameObject Round1Text;
    public GameObject Round2Text;
    public GameObject Round3Text;
    public GameObject FightText;
    public AudioSource MyPlayer;
    public AudioSource MusicPlayer;
    public AudioClip FightAudio;
    public AudioClip Round1Audio;
    public AudioClip Round2Audio;
    public AudioClip Round3Audio;
    public AudioMixerGroup Output;
    public float PauseTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Round1Text.gameObject.SetActive(false);
        Round2Text.gameObject.SetActive(false);
        Round3Text.gameObject.SetActive(false);
        FightText.gameObject.SetActive(false);
        RoundSet();
    }

    IEnumerator roundUI(GameObject roundText, AudioClip roundAudio)
    {
        // yield return new WaitForSeconds(0.4f);
        yield return new WaitForSeconds(4f);
        roundText.gameObject.SetActive(true);
        MyPlayer.clip = roundAudio;
        MyPlayer.Play();
        MyPlayer.outputAudioMixerGroup = Output;
        yield return new WaitForSeconds(PauseTime);
        roundText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        FightText.gameObject.SetActive(true);
        MyPlayer.clip = FightAudio;
        MyPlayer.Play();
        MyPlayer.outputAudioMixerGroup = Output;
        yield return new WaitForSeconds(PauseTime);
        FightText.gameObject.SetActive(false);
        MusicPlayer.Play();
        MusicPlayer.outputAudioMixerGroup = Output;
        SaveScript.TimeOut = false;
        this.gameObject.SetActive(false);
    }
    void RoundSet()
    {
        if (SaveScript.Round == 1)
        {
            StartCoroutine(roundUI(roundText: Round1Text, roundAudio: Round1Audio));
        }
        if (SaveScript.Round == 2)
        {
            StartCoroutine(roundUI(roundText: Round2Text, roundAudio: Round2Audio));
        }
        if (SaveScript.Round == 3)
        {
            StartCoroutine(roundUI(roundText: Round3Text, roundAudio: Round3Audio));
        }

    }

}
