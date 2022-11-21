using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //Setup variables
    // TODO: using one script to control both character not working ATM.  Think the original design makes it 
    // TODO: harder to alter the code for it work.  Need to think of a different method or just redesign the system.
    private List<string> Player1Settings = new List<string>(){"Player2", "Horizontal", "Vertical", "lightPunch"};
    private List<string> Player2Settings = new List<string>(){"Player", "Horizontal_P2", "Vertical_P2", "lightPunch_P2"};
    private List<string> Settings;
    private Animator Anim;
    private AnimatorStateInfo PlayerLayer0;
    public float WalkSpeed = 0.0025f;
    public float JumpSpeed = 1.0f;
    private float MoveSpeed;
    private bool IsJumping = false;
    private bool CanWalkLeft = true;
    private bool CanWalkRight = true;
    public GameObject Player;
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
    private AudioSource SFXPlayer;
    private Vector3 OppPosition;
    public static bool FacingLeft = false;
    public static bool FacingRight = true;
    public static bool WalkLeftP1 = true;
    public static bool WalkRightP1 = true;
    public GameObject Restrict;
    public Rigidbody2D RB;
    // [HideInInspector]
    public Collider2D BoxCollider; // hitbox collider
    // [HideInInspector]
    public Collider2D CapsuleCollider; // collision with ground
    private SpriteRenderer spriteRenderer;
    private float Timer = 2.0f;
    private float CrouchTime = 0.0f;
    public GameObject WinCondition;

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = WalkSpeed;
        FacingLeft = false;
        FacingRight = true;
        WalkLeftP1 = true;
        WalkRightP1 = true;
        // Debug.Log("I'm : " + this.name);
        if(this.name == "P1"){
            Player = GameObject.Find("P1");
            Opponent = GameObject.Find("P2");
            Settings = new List<string>(Player1Settings);
            // StartCoroutine(FaceRight());
        }
        else {
            Player = GameObject.Find("P2");
            Opponent = GameObject.Find("P1");
            Settings = new List<string>(Player2Settings);
            // StartCoroutine(FaceLeft());
        }
        WinCondition = GameObject.Find("WinCondition");
        WinCondition.gameObject.SetActive(false);
        Anim = GetComponentInChildren<Animator>();
        BoxCollider = GetComponentInChildren<BoxCollider2D>();
        CapsuleCollider = GetComponentInChildren<CircleCollider2D>();
        RB = GetComponentInChildren<Rigidbody2D>();
        SFXPlayer = GetComponentInChildren<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            if (OppPosition.x > Player.transform.position.x)
            {
                StartCoroutine(FaceLeft());
            }
            if (OppPosition.x < Player.transform.position.x)
            {
                StartCoroutine(FaceRight());
            }

        }
        // Debug.Log("I am: " + Player.name + " with Opponent: " + Opponent.name);
        // Check character location to ensure player doesn't walk off the screen.
        if (SaveScript.TimeOut == false)
        {
            //Check if we are knocked out
            if (SaveScript.Player1Health <= 0)
            {
                Anim.SetTrigger("KnockOut");
                Player.GetComponent<Actions>().enabled = false;
                StartCoroutine(KnockedOut());
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin>().enabled = true;
            }
            if (SaveScript.Player2Health <= 0)
            {
                Anim.SetTrigger("Victory");
                Player.GetComponent<Actions>().enabled = false;
                this.GetComponent<Move>().enabled = false;
                WinCondition.gameObject.SetActive(true);
                WinCondition.gameObject.GetComponent<LoseWin>().enabled = true;
            }

            //Listen to the Animator
            PlayerLayer0 = Anim.GetCurrentAnimatorStateInfo(0);

            if(PlayerLayer0.IsTag("React"))
            {
                SaveScript.P1Reacting = true;
            }
            else
            {
                SaveScript.P1Reacting = false;
            }

            // Check player position, so character won't exit the screen
            Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);

            if (ScreenBounds.x > Screen.width - 150)
            {
                CanWalkRight = false;
            }
            if (ScreenBounds.x < 150)
            {
                CanWalkLeft = false;
            }
            else if (ScreenBounds.x > 150 && ScreenBounds.x < Screen.width - 150)
            {
                CanWalkRight = true;
                CanWalkLeft = true;
            }

            //Get the opponent's position
            OppPosition = Opponent.transform.position;

            //Facing left or right of the Opponent
            if (OppPosition.x > Player.transform.position.x)
            {
                StartCoroutine(FaceLeft());
            }
            if (OppPosition.x < Player.transform.position.x)
            {
                StartCoroutine(FaceRight());
            }

            // Controls for moving left and right
            if(PlayerLayer0.IsTag("Motion"))
            {
                // Move forward and backward
                if(Input.GetAxis(Settings[1]) > 0)
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
                }
                if (Input.GetAxis(Settings[1]) < 0)
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
                if (Input.GetAxis(Settings[1]) == 0)
                {
                    Anim.SetBool("Forward", false);
                    Anim.SetBool("Backward", false);
                }

            }
            
            
            // Jump and Crouch
            if(Input.GetAxis(Settings[2]) > 0)
            {
                if (IsJumping == false)
                {
                    IsJumping = true;
                    Anim.SetTrigger("Jump");
                    StartCoroutine(JumpPause());
                }
            }
            if (Input.GetAxis(Settings[2]) < 0)
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
            if (Input.GetAxis(Settings[2]) == 0)
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

            if (PlayerLayer0.IsTag("Block"))
            {
                RB.isKinematic = true;
                BoxCollider.enabled = false;
                CapsuleCollider.enabled = false;
            }
            else if (PlayerLayer0.IsTag("Motion"))
            {
                BoxCollider.enabled = true;
                CapsuleCollider.enabled = true;
                RB.isKinematic = false;
            }

            if (PlayerLayer0.IsTag("Crouching"))
            {
                BoxCollider.enabled = false;
            }
            if (PlayerLayer0.IsTag("Sweep"))
            {
                BoxCollider.enabled = false;
            }
        }
    }
    
   private void OnTriggerEnter(Collider other)
    {
        if (SaveScript.P1Reacting == false)
        {
            if (other.gameObject.CompareTag("FistLight"))
            {
                Anim.SetTrigger("HeadReact");
                SFXPlayer.clip = LightPunch;
                SFXPlayer.Play();
            }
            if (other.gameObject.CompareTag("FistHeavy"))
            {
                Anim.SetTrigger("HeadReact");
                //SFXPlayer.clip = HeavyPunch;
                //SFXPlayer.Play();

            }
            if (other.gameObject.CompareTag("KickHeavy"))
            {
                Anim.SetTrigger("BigReact");
                //SFXPlayer.clip = HeavyKick;
                //SFXPlayer.Play();
            }
            if (other.gameObject.CompareTag("KickLight"))
            {
                Anim.SetTrigger("HeadReact");
                //SFXPlayer.clip = LightKick;
                //SFXPlayer.Play();
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
            Player.transform.Rotate(0, -180, 0);
            Anim.SetLayerWeight(1, 0);
        }

    }

    IEnumerator FaceRight()
    {
        if (FacingRight == true)
        {
            FacingRight = false;
            FacingLeft = true;
            yield return new WaitForSeconds(0.15f);
            Player.transform.Rotate(0, 180, 0);
            Anim.SetLayerWeight(1, 1);
        }

    }

    IEnumerator KnockedOut()
    {
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<Move>().enabled = false;
    }

    IEnumerator ResetCrouchTime()
    {
        yield return new WaitForSeconds(2.0f);
        CrouchTime = 0.0f;
    }
}
