using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Collider2D Col;
    public float DamageAmt = 0.1f;

    public bool EmitFX = false;
    public GameObject ParticlePrefab;
    public float PauseSpeed = 0.6f;

    private int selfP;
    private int otherP;

    private GameObject P1;
    private GameObject P2;


    private void Start()
    {
        Col = GetComponent<BoxCollider2D>();
        P1 = GameObject.FindGameObjectWithTag("Player1");
        P2 = GameObject.FindGameObjectWithTag("Player2");

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
            if (P2.GetComponent<PlayerActionsAI>().HitsAI == false)
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
            if (P1.GetComponent<PlayerActions>().Hits == false)
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
                //Debug.Log("Collistion " + this.name + " with trigger " + collision.name);
                if (EmitFX == true)
                {
                    Instantiate(ParticlePrefab, transform.parent.transform);
                    // Particles.Play();
                    Time.timeScale = PauseSpeed;
                }
                P2.GetComponent<PlayerActionsAI>().HitsAI = true;
                SaveScript.Player1Health -= DamageAmt;
                if (SaveScript.Player1Timer < 2.0f)
                {
                    SaveScript.Player1Timer += 2.0f;
                }
                
            }
            
        }
        if((SaveScript.P2Reacting == false) && (selfP == 1))
         {
            if (collision.gameObject.CompareTag("Player2") && collision.isTrigger)
            {
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
}
