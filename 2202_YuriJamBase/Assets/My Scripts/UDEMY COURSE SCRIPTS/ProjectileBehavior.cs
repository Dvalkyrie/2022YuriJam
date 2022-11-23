using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Vector2 launchAngle = new Vector2 (1, 0);
    public float launchPower = 5f;
    public float scale = 1; // Scale of the instancied projectile 
    public List<Sprite> sprites;
    public int damage = 50;
    public GameObject projectileInstance;

    public bool isLauncher = true; // Choose if the object is a projectile instancer or a projectile itself

    private SpriteRenderer sr;
    private bool isPlayerOne; //if not is player two
    private Transform projectilesContainer;

    private int selfP;
    private int otherP;
    private GameObject selfP_object;

    // Start is called before the first frame update
    private void Start()
    {
        if (isLauncher)
        {
            selfP_object = gameObject.transform.parent.parent.gameObject;
            Debug.Log("selfP " + selfP_object);
            if (selfP_object.CompareTag("Player1")) // second parent is Beatriz, first is Misc
            {
                selfP = 1;
                otherP = 2;
            }
            else
            {
                otherP = 1;
                selfP = 2;
            }
            sr = gameObject.GetComponent<SpriteRenderer>();
            projectilesContainer = GameObject.Find("ProjectileContainer").transform; // projectiles need to be put outside of the character that created them or else it will move with her
        }
    }


    public void RandomiseSprite()
    {
        Debug.Log("Randomise pumkin sprite");
        // Call this function to randomise the sprite of current object (used in animations)
        if (sprites.Count > 0)
        {
            sr.sprite = sprites[Random.Range(0, sprites.Count )];
        }
    }
    public void LaunchProjectile()
    {
        // Instanciate projectile with correct infos

        ProjectileBehavior projectile = Instantiate(projectileInstance, this.transform).GetComponent<ProjectileBehavior>(); //se positionne au meme endroit
        projectile.transform.SetParent(projectilesContainer, true);
        projectile.launchAngle = launchAngle * launchPower * new Vector2(-1, 1);
        projectile.transform.localScale = new Vector2( scale, scale);
        projectile.launchPower = launchPower;
        projectile.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        projectile.gameObject.GetComponent<SpriteRenderer>().sprite = sr.sprite;
        projectile.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        PlayerTrigger tg = projectile.gameObject.GetComponent<PlayerTrigger>();
        tg.enabled = true;
        tg.selfP = selfP;
        tg.otherP = otherP;
        projectile.isLauncher = false;
        projectile.isPlayerOne = isPlayerOne;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = launchAngle * launchPower * new Vector2(-1, 1) ;
        projectile.StartCoroutine("Autodestroy");
    }

    public IEnumerator Autodestroy()
    {
        // remove the projectile after some time
        Debug.Log("start Autodestroy");
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
