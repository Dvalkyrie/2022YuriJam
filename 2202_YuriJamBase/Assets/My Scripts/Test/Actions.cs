using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    // declare variables
    private List<string> Player1Settings = new List<string>(){"lightPunch", "mediumPunch", "heavyPunch", "lightKick", "mediumKick", "heavyKick", "Jump", "Block"};
    private List<string> Player2Settings = new List<string>(){"lightPunch_P2", "mediumPunch_P2", "heavyPunch_P2", "lightKick_P2", "mediumKick_P2", "heavyKick_P2", "Jump_P2", "Block_P2"};
    private List<string> Settings;
    public float JumpSpeed = 1.0f;
    public GameObject Player1;
    private Animator Anim;
    private AnimatorStateInfo Player1Layer0;
    private AudioSource MyPlayer;
    public AudioClip PunchWhoosh;
    public AudioClip KickWhoosh;
    private AudioSource Bakersfx;
    private bool HeavyMoving = false;
    private bool HeavyReact = false;
    public float PunchSlideAmt = 2f;
    public float HeavyReactAmt = 4f;
    public static bool Hits = false;
    public static bool FlyingJumpP1 = false;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Bakersfx = GetComponent<AudioSource>();
        if(this.name == "P1"){
            Player1 = GameObject.Find("P1");
            Settings = new List<string>(Player1Settings);
        }
        else {
            Player1 = GameObject.Find("P2");
            Settings = new List<string>(Player2Settings);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Listen to the Animator
        Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);
        
        //Standing attacks
        if (Player1Layer0.IsTag("Motion") || Player1Layer0.IsTag("Block"))
        {
            if (Input.GetButtonDown(Settings[0]))
            {
                Anim.SetTrigger("LightPunch");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown(Settings[1]))
            {
                Anim.SetTrigger("MediumPunch");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown(Settings[2]))
            {
                Anim.SetTrigger("HeavyPunch");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown(Settings[3]))
            {
                Anim.SetTrigger("LightKick");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown(Settings[4]))
            {
                Anim.SetTrigger("MediumKick");
            }
            if (Input.GetButtonDown(Settings[5]))
            {
                Anim.SetTrigger("HeavyKick");
                Anim.SetTrigger("BlockOff");
            }
            if (Input.GetButtonDown(Settings[6]))
            {
                Anim.SetTrigger("HeavyKick");
                Anim.SetTrigger("BlockOff");
                
            }
            if (Input.GetButtonDown(Settings[7]))
            {
                Anim.SetTrigger("BlockOn");
            }
        }

        //TODO: need to recheck with updated inputs maybe move this to Move since its where left and right are controlled.
        if (Player1Layer0.IsTag("Block"))
        {
            if (Input.GetButtonUp(Settings[7]))
            {
                Anim.SetTrigger("BlockOff");
            }
        }

        //Crouching attacks
        if (Player1Layer0.IsTag("Crouching"))
        {
            if (Input.GetButtonDown(Settings[0]) || Input.GetButtonDown(Settings[3]))
            {
                Anim.SetTrigger("LightKick");
            }

            if (Input.GetButtonDown(Settings[1]) || Input.GetButtonDown(Settings[4]))
            {
                Anim.SetTrigger("MediumKick");
            }

            if (Input.GetButtonDown(Settings[2]) || Input.GetButtonDown(Settings[5]))
            {
                Anim.SetTrigger("HeavyKick");
            }
        }


    }

    public void JumpUp()
    {
        Player1.transform.Translate(0, JumpSpeed * Time.deltaTime, 0);
    }
    public void HeavyMove()
    {
        StartCoroutine(PunchSlide());
    }
    public void HeavyReaction()
    {
        StartCoroutine(HeavySlide());
    }
    public void FlipUp()
    {
        Player1.transform.Translate(0, JumpSpeed * Time.deltaTime, 0);
        FlyingJumpP1 = true;
    }
    public void FlipBack()
    {
        Player1.transform.Translate(0, JumpSpeed * Time.deltaTime, 0);
        FlyingJumpP1 = true;
    }

    public void IdleSpeed()
    {
        FlyingJumpP1 = false;
    }

    public void ResetTime()
    {
        Time.timeScale = 1.0f;
    }


    public void KickWooshSound()
    {
        Bakersfx.clip = KickWhoosh;
        Bakersfx.Play();
    }

    public void PunchWooshSound()
    {
        Bakersfx.clip = PunchWhoosh;
        Bakersfx.Play();
    }

    public void RandomAttack()
    {

    }

    IEnumerator PunchSlide()
    {
        HeavyMoving = true;
        yield return new WaitForSeconds(0.1f);
        HeavyMoving = false;
    }

    IEnumerator HeavySlide()
    {
        HeavyReact = true;
        yield return new WaitForSeconds(0.3f);
        HeavyReact = false;
    }
}
