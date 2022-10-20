﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2TriggerAI : MonoBehaviour
{
    public Collider2D Col;
    public float DamageAmt = 0.1f;

    public bool EmitFX = false;
    public ParticleSystem Particles;
    public float PauseSpeed = 0.6f;
    public string ParticleType = "P11";

    public bool Player2 = true;

    private GameObject ChosenParticles;

    private void Start()
    {
        ChosenParticles = GameObject.Find(ParticleType);
        Particles = ChosenParticles.gameObject.GetComponent<ParticleSystem>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Player2 == true)
        {
            if (Player2ActionsAI.HitsAI == false)
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
            if (Player1Actions.Hits == false)
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
        if (SaveScript.P1Reacting == false)
        {
            if (Player2 == true)
            {
                if (collision.gameObject.CompareTag("Player1"))
                {
                    if (EmitFX == true)
                    {
                        Particles.Play();
                        Time.timeScale = PauseSpeed;
                    }
                    Player2ActionsAI.HitsAI = true;
                    SaveScript.Player1Health -= DamageAmt;
                    if (SaveScript.Player1Timer < 2.0f)
                    {
                        SaveScript.Player1Timer += 2.0f;
                    }
                }
            }
        }
        if (SaveScript.P2Reacting == false)
        {
            if (Player2 == false)
            {
                if (collision.gameObject.CompareTag("Player2"))
                {
                    if (EmitFX == true)
                    {
                        Particles.Play();
                        Time.timeScale = PauseSpeed;
                    }
                    Player1Actions.Hits = true;
                    SaveScript.Player2Health -= DamageAmt;
                    if (SaveScript.Player2Timer < 2.0f)
                    {
                        SaveScript.Player2Timer += 2.0f;
                    }

                }
            }
        }
    }
}
