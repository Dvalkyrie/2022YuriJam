using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static victoryScreenManager;
using UnityEngine.SceneManagement;

public class victoryScreenManager : MonoBehaviour
{

    public enum Player { P1, P2};
    public enum Character { Mio, Beatriz};

    public Player winnerP;
    public Character winnerC;

    public bool valueChanged = false;

    public Color beaColor;
    public Color mioColor;

    private string result; // something like "2-3" or "3-0"
    private string winnerName;

    // UI
    public Image triangleWinUI;
    public Image triangleLooseUI;
    [SerializeField] TextMeshProUGUI PlayerWinUI;
    [SerializeField] TextMeshProUGUI PlayerLooseUI;
    [SerializeField] TextMeshProUGUI ResultText;
    [SerializeField] TextMeshProUGUI DisplayEsc;
    public float escDelay = 3.5f;
    public Image MainImageUI;
    public Image MainImageShadowUI;
    public Image NameImageUI;

    // Sprites
    public Sprite MioMain;
    public Sprite BeaMain;
    public Sprite MioName;
    public Sprite BeaName;
    private bool canReturn = false;


    // Soundbites
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveScript.Player1Wins >= 2 ){
            winnerP = Player.P1;

            if(SaveScript.P1Select.Contains("Mio")){
                winnerC = Character.Mio;
            }
            else {
                winnerC = Character.Beatriz;
            }
            
        }
        else {
            winnerP = Player.P2;
            if(SaveScript.P2Select.Contains("Mio")){
                winnerC = Character.Mio;
            }
            else {
                winnerC = Character.Beatriz;
            }
        }
        DisplayEsc.enabled = false;
        result = SaveScript.Player1Wins.ToString() + ":" + SaveScript.Player2Wins.ToString();
        setUi();

        StartCoroutine(DisplayReturnText());
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (valueChanged)
        {
            setUi();
        }
        if(canReturn){
            if(Input.GetKey(KeyCode.Escape)){
                SceneManager.LoadScene(0);
            }
        }
    }

    void resetSaved() {
        SaveScript.Player1Wins = 0;
        SaveScript.Player2Wins = 0;
        SaveScript.P1Select = null;
        SaveScript.P2Select = null;
        SaveScript.Round = 0;
        
    }

    void setUi()
    {
        Debug.Log("RESULT: " + result);
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

            Debug.Log("mio won!");
            triangleWinUI.color = mioColor;
            triangleLooseUI.color = beaColor;
            MainImageUI.sprite = MainImageShadowUI.sprite = MioMain;
            NameImageUI.sprite = MioName;
            MioWinS();

        }
        else
        {
            Debug.Log("bea won!");
            triangleWinUI.color = beaColor;
            triangleLooseUI.color = mioColor;
            MainImageUI.sprite = MainImageShadowUI.sprite = BeaMain;
            NameImageUI.sprite = BeaName;
            BeaWinS();
        }
        ResultText.text = "RESULT: " + result;
    }
    public void MioWinS()
    {
        FindObjectOfType<AudioManager>().Play("Bakertheme");
        FindObjectOfType<AudioManager>().Play("BVIC");
    }

    public void BeaWinS()
    {
        FindObjectOfType<AudioManager>().Play("Cheftheme");
        FindObjectOfType<AudioManager>().Play("CVIC");
    }
    //public void PlaySound(string sound)
    //{
    //    Debug.Log("Play " + sound);
    //    audioManager.Play(sound);
    //}
    //public void PlayVocal(string sound)
    //{
    //    //Debug.Log("Play " + sound);
    //    voiceManager.Play(sound);
    //}

    IEnumerator DisplayReturnText()
    {
        yield return new WaitForSeconds(escDelay);
        DisplayEsc.enabled = true;
        canReturn = true;
    }
}
