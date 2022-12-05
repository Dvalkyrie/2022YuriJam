using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// character selection
public class CharacterSelector : MonoBehaviour
{
    private GameObject LeftCharacter;
    private GameObject RightCharacter;
    private Image LeftImage;
    private Image RightImage;
    private bool rightSelect = false;
    private bool leftSelect = false;

    public Image MioBg, BeaBg;

    private SceneTransitionManager stm;

    // Start is called before the first frame update
    void Start()
    {

        stm = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<SceneTransitionManager>();
        LeftCharacter = GameObject.FindWithTag("SelectionLeft");
        LeftImage = LeftCharacter.GetComponent<Image>();
        LeftImage.color = Color.grey;
        RightCharacter = GameObject.FindWithTag("SelectionRight");
        RightImage = RightCharacter.GetComponent<Image>();
        RightImage.color = Color.grey;
        MioBg.enabled = false;
        BeaBg.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        // select the character, left or right
        if (Input.GetAxis("Horizontal") < 0)
        {
            RightImage.color = Color.grey;
            LeftImage.color = Color.white;
            leftSelect = true;
            rightSelect = false;
            MioBg.enabled = false;
            BeaBg.enabled = true;
            
            // Debug.Log("left Select!");
            // Debug.Log(rightSelect);
            // Debug.Log(leftSelect);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            RightImage.color = Color.white;
            LeftImage.color = Color.grey;
            rightSelect = true;
            leftSelect = false;
            BeaBg.enabled = false;
            MioBg.enabled = true;
            // Debug.Log("right Select!");
            // Debug.Log(rightSelect);
            // Debug.Log(leftSelect);
        }

        // use any of the attack key to select the character, then move on to the next scene
        if (Input.GetButtonDown("lightPunch") || Input.GetButtonDown("mediumPunch") 
            ||Input.GetButtonDown("lightKick") ||Input.GetButtonDown("heavyPunch") 
            ||Input.GetButtonDown("mediumKick") || Input.GetButtonDown("heavyKick"))
        {
            if(leftSelect == true)
            {
                SaveScript.P1Select = LeftCharacter.name; 
                SaveScript.P2Select = "MioP2";
                stm.LoadSceneFight();
            }
            if (rightSelect == true)
            {
                SaveScript.P1Select = RightCharacter.name; 
                SaveScript.P2Select = "BeatrizP2";
                stm.LoadSceneFight();
            }
        }
        

    }
}
