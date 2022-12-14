using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class HealthBars : MonoBehaviour
{
    public UnityEngine.UI.Image Player1Green;
    public UnityEngine.UI.Image Player2Green;
    public UnityEngine.UI.Image Player1Red;
    public UnityEngine.UI.Image Player2Red;
    public Image P1Win1;
    public Image P1Win2;
    public Image P2Win1;
    public Image P2Win2;
    public TextMeshProUGUI TimerText;
    public float LevelTime = 90;
    public GameObject WinCondition;
    public RectTransform TimerArrow;

    private bool setHealth, applyNative;
    public Image mioIcon, mioHeart1, mioHeart2, mioNameTag;
    public Image beaIcon, beaHeart1, beaHeart2, beaNameTag;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        SaveScript.Round++;
        SaveScript.TimeOut = true; 
        if(SaveScript.Player1Wins == 1)
        {
            P2Win1.gameObject.SetActive(false);
        }
        if (SaveScript.Player1Wins == 2)
        {
            P2Win1.gameObject.SetActive(false);
            P2Win2.gameObject.SetActive(false);
        }
        if (SaveScript.Player2Wins == 1)
        {
            P1Win1.gameObject.SetActive(false);
        }
        if (SaveScript.Player2Wins == 2)
        {
            P1Win1.gameObject.SetActive(false);
            P1Win2.gameObject.SetActive(false);
        }

        // Switch nametags and icons based on P1 select
        Debug.Log(SaveScript.P1Select);
        setSpritesHealth();
    }
    void SwitchSprites(Image i1, Image i2, bool flip)
    {
        Sprite savei1 = i1.sprite;
        i1.sprite = i2.sprite;
        i2.sprite = savei1;
        if (flip)
        {
            i1.rectTransform.localScale *= new Vector2(-1, 1);
            i2.rectTransform.localScale *= new Vector2(-1, 1);
            i1.SetNativeSize();
            i2.SetNativeSize();
        }
    }
    void ApplyNative()
    {
        mioNameTag.SetNativeSize();
        beaNameTag.SetNativeSize();
    }
    void setSpritesHealth()
    {
        if (SaveScript.P1Select != null)
        {
            setHealth = true;
            if (SaveScript.P1Select.Contains("Bea"))
            {
                // switch icons
                SwitchSprites(mioIcon, beaIcon, true);
                SwitchSprites(mioHeart1, beaHeart1, false);
                SwitchSprites(mioHeart2, beaHeart2, false);
                SwitchSprites(mioNameTag, beaNameTag, false);
                applyNative = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!setHealth)
        {
            setSpritesHealth();
        }

        TimerArrow.rotation = Quaternion.Euler(0, 0, 360 / 90 * LevelTime);

        if (SaveScript.TimeOut == false)
        {
            if (LevelTime > 0)
            {
                LevelTime -= 1 * Time.deltaTime;
            }
            if (LevelTime <= 0.1)
            {
                SaveScript.TimeOut = true;
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin>().enabled = true;
            }
        }

        TimerText.text = Mathf.Round(LevelTime).ToString();


        Player1Green.fillAmount = SaveScript.Player1Health;
        Player2Green.fillAmount = SaveScript.Player2Health;

        if(SaveScript.Player2Timer > 0)
        {
            SaveScript.Player2Timer -= 2.0f * Time.deltaTime;
        }
        if (SaveScript.Player1Timer > 0)
        {
            SaveScript.Player1Timer -= 2.0f * Time.deltaTime;
        }

        if (SaveScript.Player2Timer <= 0)
        {
            if(Player2Red.fillAmount > SaveScript.Player2Health)
            {
                Player2Red.fillAmount -= 0.03f;
            }
        }
        if (SaveScript.Player1Timer <= 0)
        {
            if (Player1Red.fillAmount > SaveScript.Player1Health)
            {
                Player1Red.fillAmount -= 0.03f;
            }
        }
    }

    private void LateUpdate()
    {
        if (applyNative)
        {
            ApplyNative();
        }
    }
}
