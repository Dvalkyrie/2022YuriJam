using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Player1Trigger : MonoBehaviour
{
    public Collider Col;
    public float DamageAmt = 0.1f;

    public bool EmitFX = false;
    public ParticleSystem Particles;
    public float PauseSpeed = 0.6f;

    public bool Player1 = true;

    public string ParticleType = "P21";

    private GameObject ChosenParticles;

    private void Start()
    {
        ChosenParticles = GameObject.Find(ParticleType);
        Particles = ChosenParticles.gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player1 == true)
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
        else
        {
            if (Player2Actions.HitsP2 == false)
            {
                Col.enabled = true;
            }
            else
            {
                Col.enabled = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (SaveScript.P1Reacting == false)
        {
            if (Player1 == true)
            {
                if (other.gameObject.CompareTag("Player2"))
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
        if (SaveScript.P2Reacting == false)
        {
            if (Player1 == false)
            {
                if (other.gameObject.CompareTag("Player1"))
                {
                    if (EmitFX == true)
                    {
                        Particles.Play();
                        Time.timeScale = PauseSpeed;
                    }
                    Player2Actions.HitsP2 = true;
                    SaveScript.Player1Health -= DamageAmt;
                    if (SaveScript.Player1Timer < 2.0f)
                    {
                        SaveScript.Player1Timer += 2.0f;
                    }
                }
            }
        }
    }
}
