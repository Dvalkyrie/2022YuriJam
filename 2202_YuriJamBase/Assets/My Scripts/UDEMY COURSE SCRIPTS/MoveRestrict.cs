using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRestrict : MonoBehaviour
{
    private int otherP;
    private PlayerMove2D thisMove;

    private void Start()
    {
        if (this.transform.parent.CompareTag("Player1"))
            otherP = 1;
        else
            otherP = 2;
        thisMove = GameObject.FindGameObjectWithTag("Player" + otherP).GetComponent<PlayerMove2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If Player collide with opponent, disable movement on the evaluated side
        if(collision.gameObject.CompareTag("P"+ otherP + "Left"))
        {
            thisMove.WalkRight = false;
        }
        if (collision.gameObject.CompareTag("P" + otherP + "Right"))
        {
            thisMove.WalkRight = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // When collision end, give back the ability to move this side
        if (collision.gameObject.CompareTag("P" + otherP + "Left"))
        {
            thisMove.WalkRight = true;
        }
        if (collision.gameObject.CompareTag("P" + otherP + "Right"))
        {
            thisMove.WalkRight = true;
        }
    }

}
