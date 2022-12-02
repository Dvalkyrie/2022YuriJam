using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LoseWin2D : MonoBehaviour
{
    public GameObject GameOverText;
    public GameObject RoundOverText;
    private AudioSource MyPlayer;
    public AudioClip RoundOverAudio;
    public AudioClip GameOverAudio;
    public float PauseTime = 1.0f;
    public AudioMixerGroup Output;

    private SceneTransitionManager stm;

    // Start is called before the first frame update
    void Start()
    {
        stm = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<SceneTransitionManager>();
        SaveScript.TimeOut = true;
        MyPlayer = gameObject.GetComponent<AudioSource>();
        GameOverText.gameObject.SetActive(false);
        RoundOverText.gameObject.SetActive(false);
        StartCoroutine(WinSet());
    }

    IEnumerator WinSet()
    {
        // the original script made a difference when the Mode of the game is single or two players, we have a separate victory scene so dont need that
        yield return new WaitForSeconds(0.4f);
        if ((SaveScript.Player1Health > SaveScript.Player2Health) || (SaveScript.Player2Health > SaveScript.Player1Health))
        {
            if (SaveScript.Player1Health > SaveScript.Player2Health)
            {
                SaveScript.Player1Wins++;
            }
            else if (SaveScript.Player2Health > SaveScript.Player1Health)
            {
                SaveScript.Player1Wins++;     
            }

            if (SaveScript.Player1Wins ==2 || SaveScript.Player2Wins == 2)
            {
                // Game Over, to victory scene
                GameOverText.gameObject.SetActive(true);
                MyPlayer.clip = GameOverAudio;
                MyPlayer.Play();
                MyPlayer.outputAudioMixerGroup = Output;
                yield return new WaitForSeconds(PauseTime);
                stm.ToNextScene();
            }
            else
            {
                // Round over, repeat scene
                RoundOverText.gameObject.SetActive(true);
                MyPlayer.clip = RoundOverAudio;
                MyPlayer.Play();
                MyPlayer.outputAudioMixerGroup = Output;
                yield return new WaitForSeconds(PauseTime);
                stm.ReloadScene();
            }
            
        }

    }
}
