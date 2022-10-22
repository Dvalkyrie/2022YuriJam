using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    // declare variables

    private Animator Anim;
    private AnimatorStateInfo Player1Layer0;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Listen to the Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);
        
        //Standing attacks
        if (Player1Layer0.IsTag("Motion") || Player1Layer0.IsTag("Block"))
        {
            if (Input.GetButtonDown("lightPunch"))
            {
                Anim.SetTrigger("LightPunch");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown("mediumPunch"))
            {
                Anim.SetTrigger("MediumPunch");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown("heavyPunch"))
            {
                Anim.SetTrigger("HeavyPunch");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown("lightKick"))
            {
                Anim.SetTrigger("LightKick");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown("mediumKick"))
            {
                Anim.SetTrigger("MediumKick");
            }
            if (Input.GetButtonDown("heavyKick"))
            {
                Anim.SetTrigger("HeavyKick");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown("Jump"))
            {
                Anim.SetTrigger("HeavyKick");
                
            }
            if (Input.GetButtonDown("Block"))
            {
                Anim.SetTrigger("BlockOn");
            }
        }

        //TODO: need to recheck with updated inputs
        if (Player1Layer0.IsTag("Block"))
        {
            if (Input.GetButtonUp("Block"))
            {
                Anim.SetTrigger("BlockOff");
            }
        }

        //Crouching attacks
        if (Player1Layer0.IsTag("Crouching"))
        {
            if (Input.GetButtonDown("lightPunch") || Input.GetButtonDown("lightKick"))
            {
                Anim.SetTrigger("LightKick");
            }

            if (Input.GetButtonDown("mediumPunch") || Input.GetButtonDown("mediumKick"))
            {
                Anim.SetTrigger("MediumKick");
            }

            if (Input.GetButtonDown("heavyPunch") || Input.GetButtonDown("heavyKick"))
            {
                Anim.SetTrigger("HeavyKick");
            }
        }


    }
}
