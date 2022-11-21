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
    // Start is called before the first frame update
    void Start()
    {
        LeftCharacter = GameObject.FindWithTag("SelectionLeft");
        LeftImage = LeftCharacter.GetComponent<Image>();
        LeftImage.color = Color.grey;
        RightCharacter = GameObject.FindWithTag("SelectionRight");
        RightImage = RightCharacter.GetComponent<Image>();
        RightImage.color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        
        // select the character, left or right
        if (Input.GetAxis("Horizontal") > 0)
        {
            RightImage.color = Color.white;
            LeftImage.color = Color.grey;
            leftSelect = true;
            rightSelect = false;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            RightImage.color = Color.grey;
            LeftImage.color = Color.white;
            rightSelect = true;
            leftSelect = false;
        }

        // use any of the attack key to select the character, then move on to the next scene
        if (Input.GetButtonDown("lightPunch") || Input.GetButtonDown("mediumPunch") 
            ||Input.GetButtonDown("lightKick") ||Input.GetButtonDown("heavyPunch") 
            ||Input.GetButtonDown("mediumKick") || Input.GetButtonDown("heavyKick"))
        {
            if(leftSelect == true)
            {
                SaveScript.P1Select = LeftCharacter.name;
                SceneTransitionManager.LoadSceneFight();
            }
            else if (rightSelect == true)
            {
                SaveScript.P1Select = RightCharacter.name;
                SceneTransitionManager.LoadSceneFight();
            }
        }
        

    }
}
