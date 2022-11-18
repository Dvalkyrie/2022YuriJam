﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Collider2D Col;
    public float DamageAmt = 0.1f;

    public bool EmitFX = false;
    public GameObject ParticlePrefab;
    public float PauseSpeed = 0.6f;

    [SerializeField]
    private AudioClip hitSfx;

    private int selfP;
    private int otherP;

    private GameObject P1;
    private GameObject P2;

    PlayerActions P1_action;
    PlayerActionsAI P2_action;

    AudioSource P1_audio_src, P2_audio_src;

    PlayerMove2D P1_move;


    private void Start()
    {
        Col = GetComponent<BoxCollider2D>();
        P1 = GameObject.FindGameObjectWithTag("Player1");
        P2 = GameObject.FindGameObjectWithTag("Player2");


        P2_action = P2.GetComponent<PlayerActionsAI>();
        P1_action = P1.GetComponent<PlayerActions>();

        P1_move = P1.GetComponent<PlayerMove2D>();

        P1_audio_src = P1.GetComponent<AudioSource>();
        P2_audio_src = P2.GetComponent<AudioSource>();

        Transform characterGameobject = transform.parent.parent;
        if (characterGameobject.gameObject.CompareTag("Player1"))
        {
            selfP = 1;
            otherP = 2;
        }
        else
        {
            otherP = 1;
            selfP = 2;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (selfP == 2)
        {
            // TODO! Change stuff so we can recognise when we need a Ai or Not, or make them the same script
            if (P2_action.HitsAI == false)
            {
                Col.enabled = true;
            }
            else
            {
                Col.enabled = false;
            }
        }
        else
        {
            if (P1_action.Hits == false)
            {
                Col.enabled = true;
            }
            else
            {
                Col.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((SaveScript.P1Reacting == false) && (selfP == 2))
        {
            if (collision.gameObject.CompareTag("Player1") && collision.isTrigger)
            {
                playHit(P1_audio_src);
                //Debug.Log("Collistion " + this.name + " with trigger " + collision.name);
                if (EmitFX == true)
                {
                    Instantiate(ParticlePrefab, transform.parent.transform);
                    // Particles.Play();
                    Time.timeScale = PauseSpeed;
                }
                P2.GetComponent<PlayerActionsAI>().HitsAI = true;

                // Block, if P1 is walking away from P2 at that moment, block is on
                if ((Input.GetAxis("Horizontal") < 0 && P1_move.FacingLeft) || (Input.GetAxis("Horizontal") > 0 && P1_move.FacingRight))
                {
                    P1_action.Anim.SetTrigger("BlockOn");
                }
                else
                {
                    SaveScript.Player1Health -= DamageAmt;
                    if (SaveScript.Player1Timer < 2.0f)
                    {
                        SaveScript.Player1Timer += 2.0f;
                    }
                }
            }
            
        }
        if((SaveScript.P2Reacting == false) && (selfP == 1))
         {
            if (collision.gameObject.CompareTag("Player2") && collision.isTrigger)
            {
                playHit(P2_audio_src);
                // Debug.Log("Collistion " + this.name + " with trigger " + collision.name);
                if (EmitFX == true)
                {
                    Instantiate(ParticlePrefab, transform.parent.transform);
                    Time.timeScale = PauseSpeed;
                }
                P1.GetComponent<PlayerActions>().Hits = true;
                SaveScript.Player2Health -= DamageAmt;
                if (SaveScript.Player2Timer < 2.0f)
                {
                    SaveScript.Player2Timer += 2.0f;
                }

            }
            
        }
    }

    void playHit(AudioSource src)
    {
        src.clip = hitSfx;
        src.loop = false;
        src.Play();
    }
}
