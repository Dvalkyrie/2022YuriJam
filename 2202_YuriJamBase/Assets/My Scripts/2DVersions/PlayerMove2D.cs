using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove2D : MonoBehaviour
{
    private int selfP;
    private int otherP;

    private Animator Anim;
    public float WalkSpeed = 1f;
    public float JumpSpeed = 0.05f;
    private float MoveSpeed;
    private bool IsJumping = false;

    private AnimatorStateInfo animInfoLayer0;

    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;
    [HideInInspector]
    public GameObject Opponent;
    private AudioSource SFX;
    private Vector3 OppPosition;
    public bool FacingLeft = false;
    public bool FacingRight = true;
    public bool WalkLeft = true;
    public bool WalkRight = true;
    private GameObject Restrict;
    private Rigidbody2D RB;
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

        FacingLeft = true;
        FacingRight = false;

        WalkLeft = true;
        WalkRight = true;
  
        Opponent = GameObject.FindGameObjectWithTag("Player"+otherP);
        
        //WinCondition = GameObject.Find("WinCondition");
        WinCondition.gameObject.SetActive(false);
        Anim = GetComponent<Animator>();

        BoxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider = GetComponent<CircleCollider2D>();
        RB = GetComponent<Rigidbody2D>();
        StartCoroutine(FaceRight());
        SFX = GetComponent<AudioSource>();
        MoveSpeed = WalkSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Restrict = transform.Find("Restrict").gameObject;
        BoxCollider.offset *= new Vector2(-1, 1);
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
            if (OppPosition.x > this.transform.position.x)
            {
                StartCoroutine(FaceLeft());
            }
            if (OppPosition.x < this.transform.position.x)
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
            if ((SaveScript.Player1Health <= 0 && selfP ==1) || (SaveScript.Player2Health <= 0 && selfP == 2))
            {
                Anim.SetTrigger("KnockOut");
                this.GetComponent<PlayerActions>().enabled = false;
                StartCoroutine(KnockedOut());
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin>().enabled = true;

            }
            if ((SaveScript.Player2Health <= 0 && selfP == 1) || (SaveScript.Player1Health <= 0 && selfP == 2))
            {
                Anim.SetTrigger("Victory");
                this.GetComponent<PlayerActions>().enabled = false;
                this.GetComponent<PlayerMove2D>().enabled = false;
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin2D>().enabled = true;
            }

            //Listen to the Animator
            animInfoLayer0 = Anim.GetCurrentAnimatorStateInfo(0);

            if(animInfoLayer0.IsTag("React"))
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

            //Facing left or right of the Opponent
            if (OppPosition.x > this.transform.position.x)
            {
                Debug.Log(gameObject.name + " Face Left");
                StartCoroutine(FaceLeft());
            }
            if (OppPosition.x < this.transform.position.x)
            {
                Debug.Log(gameObject.name + " Face Right");
                StartCoroutine(FaceRight());
            }

            // Walking left and right
            if (animInfoLayer0.IsTag("Motion"))
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (CanWalkRight == true)
                    {
                        if (WalkRight == true)
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
                        if (WalkLeft == true)
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
            /*
            if (Input.GetAxis("Vertical") > 0)
            {
                if (IsJumping == false)
                {
                    IsJumping = true;
                    Anim.SetTrigger("Jump");
                    StartCoroutine(JumpPause());
                }
            }*/
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
                WalkLeft = true;
                WalkRight = true;
            }

            if (animInfoLayer0.IsTag("Block"))
            {
                RB.isKinematic = true;
                BoxCollider.enabled = false;
                CapsuleCollider.enabled = false;
            }
            else if (animInfoLayer0.IsTag("Motion"))
            {
                BoxCollider.enabled = true;
                CapsuleCollider.enabled = true;
                RB.isKinematic = false;
            }

            if (animInfoLayer0.IsTag("Crouching"))
            {
                BoxCollider.enabled = false;
            }
            if (animInfoLayer0.IsTag("Sweep"))
            {
                BoxCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((SaveScript.P1Reacting == false && selfP ==1) || (SaveScript.P2Reacting == false && selfP == 2))
        {
            if (!animInfoLayer0.IsTag("Block"))
            {
                if (collision.gameObject.CompareTag("FistLight"))
                {
                    Anim.SetTrigger("Hit1");
                }
                if (collision.gameObject.CompareTag("FistMedium"))
                {
                    Anim.SetTrigger("Hit2");
                    Debug.Log("Hit2");
                }
                if (collision.gameObject.CompareTag("FistHeavy"))
                {
                    Debug.Log("Hit3");
                    Anim.SetTrigger("Hit3");

                }
            }
        }
    }

    public void KnockBack(float power)
    {
        // Knock the character back at the opposite of facing direction
        Vector2 dir = Vector2.zero;
        if (FacingLeft) {
            dir = Vector2.left;
        }
        else
        {
            dir = Vector2.right;
        }

        RB.AddForce(dir * power);
    }

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        IsJumping = false;
    }

    IEnumerator FaceRight()
    {
        if (FacingLeft == true)
        {
            FacingLeft = false;
            FacingRight = true;
            yield return new WaitForSeconds(0.15f);
            this.transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            //this.transform.localScale = new Vector2(1, 1);
            spriteRenderer.flipX = false;
            // flip the colliders
            BoxCollider.offset *= new Vector2(-1, 1);
        }

    }

    IEnumerator FaceLeft()
    {
        if (FacingRight == true)
        {
            FacingRight = false;
            FacingLeft = true;
            yield return new WaitForSeconds(0.15f);
            //this.transform.localScale = new Vector2(-1, 1);
            this.transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            spriteRenderer.flipX = true;
            // flip the colliders
            BoxCollider.offset *= new Vector2(-1, 1);
            
        }

    }

    IEnumerator KnockedOut()
    {
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<PlayerMove2D>().enabled = false;
    }

    IEnumerator ResetCrouchTime()
    {
        yield return new WaitForSeconds(2.0f);
        CrouchTime = 0.0f;
    }

}
