using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Trigger : MonoBehaviour
{
    public Collider2D Col;
    public float DamageAmt = 0.1f;

    public bool EmitFX = false;
    public GameObject ParticlePrefab;
    // public ParticleSystem Particles;
    public float PauseSpeed = 0.6f;
    //public string ParticleType = "P11";

    public bool Player2 = true;

    private int selfP;
    private int otherP;

    private void Start()
    {
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
        if (Player2 == true)
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
        Debug.Log("Collistion with trigger");
        if (SaveScript.P1Reacting == false)
        {
            if (Player2 == true)
            {
                if (collision.gameObject.CompareTag("Player1"))
                {
                    if (EmitFX == true)
                    {
                        Instantiate(ParticlePrefab, transform.parent.transform);
                        // Particles.Play();
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
        if(SaveScript.P2Reacting == false)
        { 
            if (Player2 == false)
            {
                if (collision.gameObject.CompareTag("Player2"))
                {
                    if (EmitFX == true)
                    {
                        Instantiate(ParticlePrefab, transform.parent.transform);
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
