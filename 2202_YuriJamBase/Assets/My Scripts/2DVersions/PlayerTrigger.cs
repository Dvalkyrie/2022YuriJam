using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Collider2D Col;
    public float DamageAmt = 0.1f;

    public float PauseSpeed = 0.6f;

    [SerializeField]
    private string hitSfx;
    private AudioManager SfxManager;

    public int selfP;
    public int otherP;

    private GameObject P1;
    private GameObject P2;

    PlayerActions P1_action;
    PlayerActionsAI P2_action;

    PlayerMove2D P1_move;
    PlayerMoveAI P2_move;

    public float knockBackPower = 1;
    public bool knockBackDaze = false;

    public GameObject vfx_transform;

    public bool projectile;

    private void Start()
    {
        Col = GetComponent<BoxCollider2D>();
        P1 = GameObject.FindGameObjectWithTag("Player1");
        P2 = GameObject.FindGameObjectWithTag("Player2");


        P2_action = P2.GetComponent<PlayerActionsAI>();
        P1_action = P1.GetComponent<PlayerActions>();

        P1_move = P1.GetComponent<PlayerMove2D>();
        P2_move = P2.GetComponent<PlayerMoveAI>();

        SfxManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (!projectile)
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
        if (DebugObj.isDebug)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            DamageAmt *= 0.2f;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
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
                playHit();
                if (vfx_transform!= null)
                    vfx_transform.SetActive(true);
                // Block, if P1 is walking away from P2 at that moment, block is on
                if ((Input.GetAxis("Horizontal") < 0 && P1_move.FacingLeft) || (Input.GetAxis("Horizontal") > 0 && P1_move.FacingRight))
                {
                    P1_action.Anim.SetTrigger("BlockOn");
                    Debug.Log("Blocked");
                }
                else
                {
                    Debug.Log("Collistion " + this.name + " with trigger " + collision.name);
                    P2_action.HitsAI = true;

                    SaveScript.Player1Health -= DamageAmt;
                    if (SaveScript.Player1Timer < 2.0f)
                    {
                        SaveScript.Player1Timer += 2.0f;
                    }
                    P1_action.StartCoroutine(P1_action.KnockBack(knockBackPower, knockBackDaze));
                    Debug.Log("Knock mio");
                }
            }
            
        }
        if((SaveScript.P2Reacting == false) && (selfP == 1))
         {
            if (collision.gameObject.CompareTag("Player2") && collision.isTrigger)
            {
                playHit();
                vfx_transform.SetActive(true);
                // add block!
                P1_action.Hits = true;
                SaveScript.Player2Health -= DamageAmt;
                if (SaveScript.Player2Timer < 2.0f)
                {
                    SaveScript.Player2Timer += 2.0f;
                }
                P2_action.StartCoroutine(P2_action.KnockBack(knockBackPower, knockBackDaze));
            }
            
        }
    }

    void playHit()
    {
        if (hitSfx != "")
        {
            // Added sound for when the attack hits the opponent
            SfxManager.Play(hitSfx);
        }
    }
}
