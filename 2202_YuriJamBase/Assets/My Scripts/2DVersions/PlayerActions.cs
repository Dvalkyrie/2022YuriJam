using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public float JumpSpeed = 1.0f;
    [HideInInspector]
    public Animator Anim;
    private AnimatorStateInfo AnimStateInfo;
    private AudioSource MyPlayer;
    public AudioClip PunchWhoosh;
    public AudioClip KickWhoosh;
    private AudioManager audioManager, voiceManager;
    private bool HeavyMoving = false;
    private bool HeavyReact = false;
    public float PunchSlideAmt = 2f;
    public float HeavyReactAmt = 4f;
    public bool Hits = false;
    public bool FlyingJump = false;
    public GameObject vfx_transform;


    private PlayerMove2D thisMove;
    private Animator opponentAnim;

    // Start is called before the first frame update
    void Start()
    {

        thisMove = GetComponent<PlayerMove2D>();

        Anim = GetComponent<Animator>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.Log("Object " + gameObject.name + " cannot fin audio manager");
        }
        voiceManager = GameObject.Find("VoiceManager").GetComponent<AudioManager>();
        opponentAnim = thisMove.Opponent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.TimeOut == false)
        {
            //Heavy Punch Slide
            if (HeavyMoving == true)
            {
                if (thisMove.FacingRight == true)
                {
                    this.transform.Translate(-PunchSlideAmt * Time.deltaTime, 0, 0);
                }
                if (thisMove.FacingLeft == true)
                {
                    this.transform.Translate(PunchSlideAmt * Time.deltaTime, 0, 0);
                }
            }

            //Heavy React Slide
            if (HeavyReact == true)
            {
                if (thisMove.FacingRight == true)
                {
                    this.transform.Translate(HeavyReactAmt * Time.deltaTime, 0, 0);
                }
                if (thisMove.FacingLeft == true)
                {
                    this.transform.Translate(-HeavyReactAmt * Time.deltaTime, 0, 0);
                }
            }

            //Listen to the Animator
            AnimStateInfo = Anim.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo OpponentStateInfo = opponentAnim.GetCurrentAnimatorStateInfo(0);
            if (OpponentStateInfo.IsTag("Attack"))
            {
                if ((Input.GetAxis("Horizontal") < 0 && thisMove.FacingLeft) || (Input.GetAxis("Horizontal") > 0 && thisMove.FacingRight))
                {
                    Anim.SetTrigger("BlockOn");
                    Debug.Log("Blocked");
                }
            }


            //Standing attacks
            if (AnimStateInfo.IsTag("Motion") || AnimStateInfo.IsTag("Block"))
            {
                if (Input.anyKeyDown)
                {
                    // Hide vfx empty
                    vfx_transform.SetActive(false);
                }
                // Custom mapping for mouse: allow to do kicks when holding shift
                bool shift_down = false;
                if (Input.GetKey(KeyCode.LeftShift))
                    shift_down = true;

                if (Input.GetButtonDown("lightPunch") && !shift_down)
                {
                    Anim.SetTrigger("LightPunch");
                    Hits = false;;
                }
                if (Input.GetButtonDown("mediumPunch") && !shift_down)
                {
                    Anim.SetTrigger("MediumPunch");
                    Hits = false;
                }
                if (Input.GetButtonDown("heavyPunch") && !shift_down)
                {
                    Anim.SetTrigger("HeavyPunch");
                    Hits = false;
                }
                if (Input.GetButtonDown("lightKick") || (Input.GetButtonDown("lightPunch")&&shift_down))
                {
                    Anim.SetTrigger("LightKick");
                    Hits = false;
                }
                if (Input.GetButtonDown("mediumKick") || (Input.GetButtonDown("mediumPunch") && shift_down))
                {
                    Anim.SetTrigger("MediumKick");
                    Hits = false;
                }
                if (Input.GetButtonDown("heavyKick") || (Input.GetButtonDown("heavyPunch") && shift_down))
                {
                    Anim.SetTrigger("HeavyKick");
                    Hits = false;
                }
                if (AnimStateInfo.IsTag("Block")) {
                    if (Input.GetButtonDown("lightPunch") || Input.GetButtonDown("mediumPunch") || Input.GetButtonDown("heavyPunch") || Input.GetButtonDown("lightKick") || Input.GetButtonDown("mediumKick") || Input.GetButtonDown("heavyKick")){
                        Anim.SetTrigger("BlockOff");
                    }
                }
            }

            if (AnimStateInfo.IsTag("Block")) {
                // if not moving, or moving in the front direction block off /!\ Facingleft and Facing right are inverted
                if ((Input.GetAxis("Horizontal") >= 0 && thisMove.FacingLeft) || (Input.GetAxis("Horizontal") <= 0 && thisMove.FacingRight))
                    {
                      Anim.SetTrigger("BlockOff");
                    }
                
             }


            //Crouching attacks
            if (AnimStateInfo.IsTag("Crouching"))
            {
                if (Input.GetButtonDown("lightPunch") || Input.GetButtonDown("lightKick"))
                {
                    Anim.SetTrigger("LightKick");
                    Hits = false;
                }

                if (Input.GetButtonDown("mediumPunch") || Input.GetButtonDown("mediumKick"))
                {
                    Anim.SetTrigger("MediumKick");
                    Hits = false;
                }

                if (Input.GetButtonDown("heavyPunch") || Input.GetButtonDown("heavyKick"))
                {
                    Anim.SetTrigger("HeavyKick");
                    Hits = false;
                }
            }
        }
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
        this.transform.Translate(0, JumpSpeed * Time.deltaTime, 0);
        FlyingJump = true;
    }
    public void FlipBack()
    {
        this.transform.Translate(0, JumpSpeed * Time.deltaTime, 0);
        FlyingJump = true;
    }

    public void IdleSpeed()
    {
        FlyingJump = false;
    }

    public void ResetTime()
    {
        Time.timeScale = 1.0f;
    }

    
    public void PlaySound(string sound)
    {
        // Debug.Log("Play " + sound);
        if (audioManager)
        {
            audioManager.Play(sound);
        }
    }
    public void PlayVocal(string sound)
    {
        if (voiceManager)
        {
            //Debug.Log("Play " + sound);
            voiceManager.Play(sound);
        }
    }

    public void PunchWooshSound()
    {

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

    public IEnumerator KnockBack(float duration, bool dazed)
    {
        // Use "heavyReact" (a state where the player go backward) to do knock back), in PlayerActionAI there is a daze option but not here so idk...
        HeavyReact = true;
        // Dazed = true;
        yield return new WaitForSeconds(duration);
        HeavyReact = false;
        // yield return new WaitForSeconds(DazedTime);
        // Dazed = false;
    }
    public IEnumerator dashAttack(float duration)
    {
        HeavyMoving = true;
        yield return new WaitForSeconds(duration);
        HeavyMoving = false;

    }
    public void TriggerDashAttack(float duration)
    {
        StartCoroutine(dashAttack(duration));
    }
}
