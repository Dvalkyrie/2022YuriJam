using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class victoryScreenManager : MonoBehaviour
{

    public enum Player { P1, P2};
    public enum Character { Mio, Beatriz};

    public Player winnerP;
    public Character winnverC;

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

    // Start is called before the first frame update
    void Start()
    {
        setUi();
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
        if (winnverC == Character.Mio)
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
}
