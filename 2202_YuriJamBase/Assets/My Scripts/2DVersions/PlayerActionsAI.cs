﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsAI : MonoBehaviour
{
    public float JumpSpeed = 1.0f;
    public float FlipSpeed = 0.8f;
    private Animator Anim;
    private AudioManager audioManager, voiceManager;
    private AnimatorStateInfo Player1Layer0;
    private bool HeavyMoving = false;
    private bool HeavyReact = false;
    public float PunchSlideAmt = 2f;
    public float HeavyReactAmt = 4f;
    public bool HitsAI = false;
    public bool FlyingJumpAI = false;
    public bool Dazed = false;
    //AkSoundEngine akSoundEngine; 
    private int AttackNumber = 1;
    private bool Attacking = true;
    public float AttackRate = 1.0f;
    public float DazedTime = 3.0f;

    PlayerMoveAI thisMove;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveScript.DifficultyAmt == 1.0)
        {
            AttackRate = 0.3f;
        }
        if (SaveScript.DifficultyAmt == 2.0)
        {
            AttackRate = 1.0f;
        }
        if (SaveScript.DifficultyAmt == 3.0)
        {
            AttackRate = 2.0f;
        }
        Anim = GetComponent<Animator>();
        thisMove = GetComponent<PlayerMoveAI>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        voiceManager = GameObject.Find("VoiceManager").GetComponent<AudioManager>();
        //akSoundEngine = GetComponent<AkSoundEngine>();
        Dazed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player1Layer0.IsTag("Motion"))
        {
            //Debug.Log("Animation is tag Motion");
        }
        //Debug.Log("Attacking = " + Attacking); //Expect this to say true
        //Debug.Log("Attack Number = " + AttackNumber); //Expect anything above 0

        if (SaveScript.TimeOut == false)
        {

            //Heavy Punch Slide
            if (HeavyMoving == true)
            {
                if (thisMove.FacingRightAI == true)
                {
                    this.transform.Translate(PunchSlideAmt * Time.deltaTime, 0, 0);
                }
                if (thisMove.FacingLeftAI == true)
                {
                    this.transform.Translate(-PunchSlideAmt * Time.deltaTime, 0, 0);
                }
            }

            //Heavy React Slide
            if (HeavyReact == true)
            {
                if (thisMove.FacingRightAI == true)
                {
                    this.transform.Translate(-HeavyReactAmt * Time.deltaTime, 0, 0);
                }
                if (thisMove.FacingLeftAI == true)
                {
                    this.transform.Translate(HeavyReactAmt * Time.deltaTime, 0, 0);
                }
            }

            //Listen to the Animator
            Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

            //Standing attacks
            if (Player1Layer0.IsTag("Motion"))
            {
                if (Attacking == true)
                {
                    Attacking = false;
                    if (AttackNumber == 1)
                    {
                        Anim.SetTrigger("LightPunch");
                        HitsAI = false;
                        StartCoroutine(SetAttacking());
                    }
                    if (AttackNumber == 2)
                    {
                        Anim.SetTrigger("HeavyPunch");
                        HitsAI = false;
                        StartCoroutine(SetAttacking());
                    }
                    if (AttackNumber == 3)
                    {
                        Anim.SetTrigger("LightKick");
                        HitsAI = false;
                        StartCoroutine(SetAttacking());
                    }
                    if (AttackNumber == 4)
                    {
                        Anim.SetTrigger("HeavyKick");
                        HitsAI = false;
                        StartCoroutine(SetAttacking());
                    }
                }
            }


            //Crouching attack
            if (Player1Layer0.IsTag("Crouching"))
            {
                Anim.SetTrigger("LightKick");
                HitsAI = false;
                Anim.SetBool("Crouch", false);
            }

            //Aerial moves
            if (Player1Layer0.IsTag("Jumping"))
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Anim.SetTrigger("HeavyKick");
                    HitsAI = false;
                }
            }
        }
    }

    public void JumpUp()
    {
        this.transform.Translate(0, JumpSpeed * Time.deltaTime, 0);
    }
    public void HeavyMove()
    {
        StartCoroutine(PunchSlide());
    }

    public void RandomAttack()
    {
        AttackNumber = Random.Range(1, 5);
        //StartCoroutine(SetAttacking());

    }

    public void HeavyReaction()
    {
        StartCoroutine(HeavySlide());
        AttackNumber = 3;
    }
    public void FlipUp()
    {
        this.transform.Translate(0, FlipSpeed * Time.deltaTime, 0);
        FlyingJumpAI = true;
    }
    public void FlipBack()
    {
        this.transform.Translate(0, FlipSpeed * Time.deltaTime, 0);
        FlyingJumpAI = true;
    }
    public void IdleSpeed()
    {
        FlyingJumpAI = false;
    }

    public void PlaySound(string sound)
    {
        //Debug.Log("Play " + sound);
        audioManager.Play(sound);
    }
    public void PlayVocal(string sound)
    {
        //Debug.Log("Play " + sound);
        voiceManager.Play(sound);
    }

    public void PunchWooshSound()
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
            Dazed = true;
            yield return new WaitForSeconds(0.3f);
            HeavyReact = false;
            yield return new WaitForSeconds(DazedTime);
            Dazed = false;
    }

    IEnumerator SetAttacking()
    {
        yield return new WaitForSeconds(AttackRate);
        Attacking = true;
    }

}
