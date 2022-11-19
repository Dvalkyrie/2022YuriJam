using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static victoryScreenManager;

public class victoryScreenManager : MonoBehaviour
{

    public enum Player { P1, P2};
    public enum Character { Mio, Beatriz};

    public Player winnerP;
    public Character winnerC;

    public bool valueChanged = false;

    public Color beaColor;
    public Color mioColor;

    public string result; // something like "2-3" or "3-0"

    // UI
    public Image triangleWinUI;
    public Image triangleLooseUI;
    public TMP_Text PlayerWinUI;
    public TMP_Text PlayerLooseUI;
    public TMP_Text ResultText;
    public Image MainImageUI;
    public Image MainImageShadowUI;
    public Image NameImageUI;

    // Sprites
    public Sprite MioMain;
    public Sprite BeaMain;
    public Sprite MioName;
    public Sprite BeaName;


    // Soundbites
    private AudioManager audioManager, voiceManager;

    // Start is called before the first frame update
    void Start()
    {
        setUi();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        voiceManager = GameObject.Find("VoiceManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (valueChanged)
        {
            setUi();
        }
    }

    void setUi()
    {
        if (winnerP == Player.P1)
        {
            PlayerWinUI.text = "P1";
            PlayerLooseUI.text = "P2";
        }
        else
        {
            PlayerWinUI.text = "P2";
            PlayerLooseUI.text = "P1";
        }
        if (winnerC == Character.Mio)
        {
            triangleWinUI.color = mioColor;
            triangleLooseUI.color = beaColor;
            MainImageUI.sprite = MainImageShadowUI.sprite = MioMain;
            NameImageUI.sprite = MioName;
            
            
        }
        else
        {
            triangleWinUI.color = beaColor;
            triangleLooseUI.color = mioColor;
            MainImageUI.sprite = MainImageShadowUI.sprite = BeaMain;
            NameImageUI.sprite = BeaName;
        }
        ResultText.text = "RESULT: " + result;
    }
    public void PlaySound(string sound)
    {
        Debug.Log("Play " + sound);
        audioManager.Play(sound);
    }
    public void PlayVocal(string sound)
    {
        //Debug.Log("Play " + sound);
        voiceManager.Play(sound);
    }

}
