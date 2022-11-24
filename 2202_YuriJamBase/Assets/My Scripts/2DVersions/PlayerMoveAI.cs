using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAI : MonoBehaviour
{
    private int selfP;
    private int otherP;

    private Animator Anim;
    public float WalkSpeed = 1f;
    public float JumpSpeed = 0.05f;
    private float MoveSpeed;
    private bool IsJumping = false;
    private AnimatorStateInfo AnimStateInfo;
    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;
    public GameObject Opponent;
    private Vector3 OppPosition;
    public bool FacingLeftAI = false;
    public bool FacingRightAI = true;
    public bool WalkLeftAI = true;
    public bool WalkRightAI = true;
    private AudioSource MyPlayer;
    private GameObject Restrict;
    private Rigidbody2D RB;
    [HideInInspector]
    public Collider2D BoxCollider;
    [HideInInspector]
    public Collider2D CapsuleCollider;

    PlayerActionsAI thisAction;

    private float OppDistance;
    public float AttackDistance = 1.5f;
    private bool MoveAI = true;
    public static bool AttackState = false;
    private int Defend = 0;
    private bool IsBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Player1"))
        {
            selfP = 1;
            otherP = 2;
        }
        else
        {
            otherP = 1;
            selfP = 2;
        }

        FacingLeftAI = false;
        FacingRightAI = true;
        WalkLeftAI = true;
        WalkRightAI = true;
        Opponent = GameObject.Find("Player"+otherP);
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider = GetComponent<CircleCollider2D>();

        StartCoroutine(FaceRight());

        MyPlayer = GetComponentInChildren<AudioSource>();
        MoveSpeed = WalkSpeed;

        Restrict = transform.Find("Restrict").gameObject;

        thisAction = GetComponent<PlayerActionsAI>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the time of the battle is up, disable movement
        if (SaveScript.TimeOut == true)
        {
            Anim.SetBool("Forward", false);
            Anim.SetBool("Backward", false);
        }
        if (SaveScript.TimeOut == false)
        {
            OppDistance = Vector3.Distance(Opponent.transform.position, this.transform.position);

            //Check if we are knocked out
            if ((SaveScript.Player2Health <= 0 && selfP ==1) || (SaveScript.Player1Health <= 0 && selfP == 2))
            {
                Debug.Log("KnockedOutCall");
                Anim.SetTrigger("KnockOut");
                thisAction.enabled = false;
                StartCoroutine(KnockedOut());

            }
            if ((SaveScript.Player1Health <= 0 && selfP == 1) || (SaveScript.Player2Health <= 0 && selfP == 2))
            {
                Anim.SetTrigger("Victory");
                thisAction.enabled = false;
                this.GetComponent<PlayerMoveAI>().enabled = false;
            }

            //Listen to the Animator
            AnimStateInfo = Anim.GetCurrentAnimatorStateInfo(0);

            if (AnimStateInfo.IsTag("React"))
            {
                if (selfP == 1)
                    SaveScript.P1Reacting = true;
                else
                    SaveScript.P2Reacting = true;
            }
            else
            {
                if (selfP == 1)
                    SaveScript.P1Reacting = false;
                else
                    SaveScript.P2Reacting = false;
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

            if (thisAction.Dazed == false)
            {
                //Facing left or right of the Opponent
                if (OppPosition.x > this.transform.position.x)
                {

                    StartCoroutine(FaceLeft());

                    if (AnimStateInfo.IsTag("Motion"))
                    {
                        Anim.SetBool("CanAttack", false);

                        if (OppDistance > AttackDistance)
                        {
                            if (MoveAI == true)
                            {
                                if (CanWalkRight == true)
                                {
                                    if (WalkRightAI == true)
                                    {
                                        Anim.SetBool("Forward", true);
                                        Anim.SetBool("Backward", false);
                                        AttackState = false;
                                        transform.Translate(WalkSpeed * Time.deltaTime, 0, 0);
                                    }
                                }
                            }
                        }
                        if (OppDistance < AttackDistance)
                        {
                            if (CanWalkRight == true)
                            {
                                if (MoveAI == true)
                                {
                                    MoveAI = false;
                                    Anim.SetBool("Forward", false);
                                    Anim.SetBool("Backward", false);
                                    Anim.SetBool("CanAttack", true);
                                    StartCoroutine(ForwardFalse());
                                }
                            }
                        }
                    }
                }
                if (OppPosition.x < this.transform.position.x)
                {
                    StartCoroutine(FaceRight());

                    if (AnimStateInfo.IsTag("Motion"))
                    {
                        Anim.SetBool("CanAttack", false);
                        if (OppDistance > AttackDistance)
                        {
                            if (MoveAI == true)
                            {
                                if (CanWalkLeft == true)
                                {
                                    if (WalkLeftAI == true)
                                    {
                                        Anim.SetBool("Backward", true);
                                        Anim.SetBool("Forward", false);
                                        AttackState = false;
                                        transform.Translate(-WalkSpeed * Time.deltaTime, 0, 0);
                                    }
                                }
                            }
                        }
                        if (OppDistance < AttackDistance)
                        {
                            if (CanWalkLeft == true)
                            {
                                if (MoveAI == true)
                                {
                                    MoveAI = false;
                                    Anim.SetBool("Forward", false);
                                    Anim.SetBool("Backward", false);
                                    // Anim.SetBool("CanAttack", true);
                                    StartCoroutine(ForwardFalse());
                                }
                            }
                        }
                    }
                }
            }

          

            //Jumping and crouching
            /*
            if (Input.GetAxis("VerticalP2") > 0)
            {
                if (IsJumping == false)
                {
                    IsJumping = true;
                    Anim.SetTrigger("Jump");
                    StartCoroutine(JumpPause());
                }
            }*/

            if (Defend == 3)
            {
                Anim.SetBool("Crouch", true);
                Defend = 0;
            }
            if (Defend == 2)
            {
                if (IsBlocking == false)
                {
                    IsBlocking = true;
                    Anim.SetTrigger("BlockOn");
                    StartCoroutine(EndBlock());
                }
            }
            /*
            if (Defend == 4)
            {
                Anim.SetTrigger("Jump");
                Defend = 0;
            }
            */
            //Resets the restrict
            if (Restrict.gameObject.activeInHierarchy == false)
            {
                WalkLeftAI = true;
                WalkRightAI = true;
            }

            if (AnimStateInfo.IsTag("Block"))
            {
                RB.isKinematic = true;
                BoxCollider.enabled = false;
                CapsuleCollider.enabled = false;
            }
            else if (AnimStateInfo.IsTag("Motion"))
            {
                BoxCollider.enabled = true;
                CapsuleCollider.enabled = true;
                RB.isKinematic = false;
            }

            if (AnimStateInfo.IsTag("Crouching"))
            {
                BoxCollider.enabled = false;
            }
            if (AnimStateInfo.IsTag("Sweep"))
            {
                BoxCollider.enabled = false;
            }
        }
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SaveScript.P2Reacting == false && selfP==2 || SaveScript.P1Reacting == false && selfP == 1)
        {
            if (!AnimStateInfo.IsTag("Block"))
            {
                if (collision.gameObject.CompareTag("FistLight"))
                {
                    Anim.SetTrigger("Hit1");
                    Defend = Random.Range(0, 5);
                }
                if (collision.gameObject.CompareTag("FistMedium"))
                {
                    Anim.SetTrigger("Hit2");
                }
                if (collision.gameObject.CompareTag("FistHeavy"))
                {
                    Anim.SetTrigger("Hit3");
                }
            }
        }
    }
    /*
    public void KnockBack(float power)
    {
        // Knock the character back at the opposite of facing direction
        Vector2 dir = Vector2.zero;
        if (FacingLeftAI)
        {
            dir = Vector2.left;
        }
        else
        {
            dir = Vector2.right;
        }

        RB.AddForce(dir * power);
    }
    */

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        IsJumping = false;
    }

    IEnumerator FaceRight()
    {
        if (FacingLeftAI == true)
        {
            FacingLeftAI = false;
            FacingRightAI = true;
            yield return new WaitForSeconds(0.15f);
            this.transform.localScale = new Vector2(1, 1);
            //spriteRenderer.flipX = false;
        }

    }

    IEnumerator FaceLeft()
    {
        if (FacingRightAI == true)
        {
            FacingRightAI = false;
            FacingLeftAI = true;
            yield return new WaitForSeconds(0.15f);
            this.transform.localScale = new Vector2(-1, 1);
            //spriteRenderer.flipX = true;
        }
    }

    IEnumerator KnockedOut()
    {
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<PlayerMoveAI>().enabled = false;
    }

    IEnumerator ForwardFalse()
    {
        yield return new WaitForSeconds(0.6f);
        MoveAI = true;
    }

    IEnumerator EndBlock()
    {
        yield return new WaitForSeconds(2.0f);
        IsBlocking = false;
        Anim.SetTrigger("BlockOff");
        Defend = 0;
    }

}
