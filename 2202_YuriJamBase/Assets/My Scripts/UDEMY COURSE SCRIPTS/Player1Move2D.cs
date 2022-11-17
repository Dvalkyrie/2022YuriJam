﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Move2D : MonoBehaviour
{
    private Animator Anim;
    public float WalkSpeed = 0.001f;
    public float JumpSpeed = 0.05f;
    private float MoveSpeed;
    private bool IsJumping = false;
    private AnimatorStateInfo Player1Layer0;
    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;
    public GameObject Player1;
    [HideInInspector]
    public GameObject Opponent;
    public AudioClip LightPunch;
    public AudioClip MediumPunch;
    public AudioClip HeavyPunch;
    public AudioClip LightKick;
    public AudioClip MediumKick;
    public AudioClip HeavyKick;
    public AudioClip LightCrouch;
    public AudioClip MediumCrouch;
    public AudioClip HeavyCrouch;
    private AudioSource MioSFX;
    private Vector3 OppPosition;
    public static bool FacingLeft = false;
    public static bool FacingRight = true;
    public static bool WalkLeftP1 = true;
    public static bool WalkRightP1 = true;
    public GameObject Restrict;
    public Rigidbody2D RB;
    [HideInInspector]
    public Collider2D BoxCollider; // hitbox collider
    [HideInInspector]
    public Collider2D CapsuleCollider; // collision with ground
    private SpriteRenderer spriteRenderer;
    private float Timer = 2.0f;
    private float CrouchTime = 0.0f;
    public GameObject WinCondition;


    // Start is called before the first frame update
    void Start()
    {
        FacingLeft = true;
        FacingRight = false;
        WalkLeftP1 = true;
        WalkRightP1 = true;
        Opponent = GameObject.FindGameObjectWithTag("Player2");
        WinCondition = GameObject.Find("WinCondition");
        WinCondition.gameObject.SetActive(false);
        Anim = GetComponent<Animator>();
        Debug.Log("Anim" + Anim);
        BoxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider = GetComponent<CircleCollider2D>();
        RB = GetComponent<Rigidbody2D>();
        StartCoroutine(FaceRight());
        MioSFX = GetComponent<AudioSource>();
        MoveSpeed = WalkSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(SaveScript.TimeOut == true)
        {
            Anim.SetBool("Forward", false);
            Anim.SetBool("Backward", false);

            //Get the opponent's position
            OppPosition = Opponent.transform.position;

            //Facing left or right of the Opponent
            if (OppPosition.x > Player1.transform.position.x)
            {
                StartCoroutine(FaceLeft());
            }
            if (OppPosition.x < Player1.transform.position.x)
            {
                StartCoroutine(FaceRight());
            }

        }
        if (SaveScript.TimeOut == false)
        {
            if (Player1Actions.FlyingJumpP1 == true)
            {
                WalkSpeed = JumpSpeed;
            }
            else
            {
                WalkSpeed = MoveSpeed;
            }
            //Check if we are knocked out
            if (SaveScript.Player1Health <= 0)
            {
                Anim.SetTrigger("KnockOut");
                Player1.GetComponent<Player1Actions>().enabled = false;
                StartCoroutine(KnockedOut());
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin>().enabled = true;
            }
            if (SaveScript.Player2Health <= 0)
            {
                Anim.SetTrigger("Victory");
                Player1.GetComponent<Player1Actions>().enabled = false;
                this.GetComponent<Player1Move>().enabled = false;
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin>().enabled = true;
            }

            //Listen to the Animator
            Player1Layer0 = Anim.GetCurrentAnimatorStateInfo(0);

            if(Player1Layer0.IsTag("React"))
            {
                SaveScript.P1Reacting = true;
            }
            else
            {
                SaveScript.P1Reacting = false;
            }

            //Cannot exit screen
            Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);

            if (ScreenBounds.x > Screen.width - 200)
            {
                CanWalkRight = false;
            }
            if (ScreenBounds.x < 200)
            {
                CanWalkLeft = false;
            }
            else if (ScreenBounds.x > 200 && ScreenBounds.x < Screen.width - 200)
            {
                CanWalkRight = true;
                CanWalkLeft = true;
            }

            //Get the opponent's position
            OppPosition = Opponent.transform.position;

            //Facing left or right of the Opponent
            if (OppPosition.x > Player1.transform.position.x)
            {
                StartCoroutine(FaceLeft());
            }
            if (OppPosition.x < Player1.transform.position.x)
            {
                StartCoroutine(FaceRight());
            }

            // Walking left and right
            if (Player1Layer0.IsTag("Motion"))
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (CanWalkRight == true)
                    {
                        if (WalkRightP1 == true)
                        {
                            Anim.SetBool("Forward", true);
                            transform.Translate(WalkSpeed * Time.deltaTime, 0, 0);
                        }
                    }
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    if (CanWalkLeft == true)
                    {
                        if (WalkLeftP1 == true)
                        {
                            Anim.SetBool("Backward", true);
                            transform.Translate(-WalkSpeed * Time.deltaTime, 0, 0);
                        }
                    }
                }
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                Anim.SetBool("Forward", false);
                Anim.SetBool("Backward", false);
            }


            //Jumping and crouching
            if (Input.GetAxis("Vertical") > 0)
            {
                if (IsJumping == false)
                {
                    IsJumping = true;
                    Anim.SetTrigger("Jump");
                    StartCoroutine(JumpPause());
                }
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                if (CrouchTime < Timer)
                {
                    CrouchTime += 1.0f * Time.deltaTime;
                    Anim.SetBool("Crouch", true);
                }
                else if (CrouchTime > Timer)
                {
                    Anim.SetBool("Crouch", false);
                    StartCoroutine(ResetCrouchTime());
                }
            }
            if (Input.GetAxis("Vertical") == 0)
            {
                Anim.SetBool("Crouch", false);
                CrouchTime = 0.0f;
            }

            //Resets the restrict
            if (Restrict.gameObject.activeInHierarchy == false)
            {
                WalkLeftP1 = true;
                WalkRightP1 = true;
            }

            if (Player1Layer0.IsTag("Block"))
            {
                RB.isKinematic = true;
                BoxCollider.enabled = false;
                CapsuleCollider.enabled = false;
            }
            else if (Player1Layer0.IsTag("Motion"))
            {
                BoxCollider.enabled = true;
                CapsuleCollider.enabled = true;
                RB.isKinematic = false;
            }

            if (Player1Layer0.IsTag("Crouching"))
            {
                BoxCollider.enabled = false;
            }
            if (Player1Layer0.IsTag("Sweep"))
            {
                BoxCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SaveScript.P1Reacting == false)
        {
            if (collision.gameObject.CompareTag("FistLight"))
            {
                Anim.SetTrigger("Hit1");
                MioSFX.clip = LightPunch;
                MioSFX.Play();
            }
            if (collision.gameObject.CompareTag("FistMedium"))
            {
                Anim.SetTrigger("Hit2");
                MioSFX.clip = MediumPunch;
                MioSFX.Play();
            }
            if (collision.gameObject.CompareTag("FistHeavy"))
            {
                Anim.SetTrigger("Hit3");
                MioSFX.clip = HeavyPunch;
                MioSFX.Play();

            }
        }
    }

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        IsJumping = false;
    }

    IEnumerator FaceLeft()
    {
        if (FacingLeft == true)
        {
            FacingLeft = false;
            FacingRight = true;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.flipX = false;
            //Player1.transform.Rotate(0, -180, 0);
            //Anim.SetLayerWeight(1, 0);
        }

    }

    IEnumerator FaceRight()
    {
        if (FacingRight == true)
        {
            FacingRight = false;
            FacingLeft = true;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.flipX = true;
            //Player1.transform.Rotate(0, 180, 0);
            //Anim.SetLayerWeight(1, 1);
        }

    }

    IEnumerator KnockedOut()
    {
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<Player1Move>().enabled = false;
    }

    IEnumerator ResetCrouchTime()
    {
        yield return new WaitForSeconds(2.0f);
        CrouchTime = 0.0f;
    }

}
