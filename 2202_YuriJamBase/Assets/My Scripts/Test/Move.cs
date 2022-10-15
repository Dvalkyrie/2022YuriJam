using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //Setup variables
    private Animator Anim;
    private AnimatorStateInfo Player1Layer0;
    public float WalkSpeed = 0.0025f;
    public float JumpSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // check the current state of the Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

        if(Player1Layer0.IsTag("Motion"))
        {
            // Move forward and backward
            if(Input.GetAxis("Horizontal") > 0)
            {
                Anim.SetBool("Forward", true);
                transform.Translate(-WalkSpeed, 0, 0);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                Anim.SetBool("Backward", true);
                transform.Translate(WalkSpeed, 0, 0);
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                Anim.SetBool("Forward", false);
                Anim.SetBool("Backward", false);
            }

        }
        
        // Jump and Crouch
        if(Input.GetAxis("Vertical") > 0)
        {
            Anim.SetTrigger("Jump"); // not used yet
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            Anim.SetBool("Crouch", true);
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            Anim.SetBool("Crouch", false);
        }
    }
}
