/*****************************************************************
Script to control the movement of the BONUS Player.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class BonusPlayer : MonoBehaviour
{

    /** the animator connected to the player sprite */
    Animator animator;

    /** the speed at which the player travels */
    float speed = 2.5f;

    /** starting position of the player sprite */
    private Vector2 init;

    /** starting position of the camera */
    private Vector3 camInit;

    /** game object for map borders */
    private GameObject[] borders;

    /** game object for the character */
    private GameObject character;

    /** start positions for left and right sides */
    private Vector2 leftInit;
    private Vector2 rightInit;

    /** player animation states */
    const int STATE_IDLED = 0;
    const int STATE_IDLEU = 1;
    const int STATE_IDLER = 2;
    const int STATE_IDLEL = 3;
    const int STATE_WALKD = 4;
    const int STATE_WALKU = 5;
    const int STATE_WALKR = 6;
    const int STATE_WALKL = 7;

    /** RigidBody associated with the player */
    private Rigidbody2D rb;

    /** maximum height of the player */
    private float maxHeight = 0;

    /** bools associated with player positions */
    private bool jumping = false;
    private bool grounded = true;

    /** current state of the animation (starts idle down) */
    int currentAnimationState = STATE_IDLED;

    /** player health */
    int health;

    /** current direction (0 down, 1 left, 2 up, 3 right) */
    int facingDirection = 0;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {
        // game start position for reset
        init = transform.position;
        camInit = Camera.main.transform.position;

        // define animator attached to character
        animator = this.GetComponent<Animator>();

        // get borders so they can be moved
        borders = GameObject.FindGameObjectsWithTag("border");
        leftInit = borders[0].transform.position;
        rightInit = borders[1].transform.position;

        // set player's health
        health = 3;
        rb = GetComponent<Rigidbody2D>();
    }

    /*******************************************************************
	 * Method called once per frame to update sprite
	 ******************************************************************/
    void FixedUpdate()
    {
        Move();
    }

    /*******************************************************************
	 * Method that moves the sprite
     * Takes input from the keyboard to move, use items, or
     * interact with the environment
	 ******************************************************************/
    void Move()
    {
        if (Input.GetKey("a"))
        {
            facingDirection = 1;
            changeState(STATE_WALKL);
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("d"))
        {
            facingDirection = 3;
            changeState(STATE_WALKR);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        

        else if (Input.GetKey("r"))
        {
            // return player to start position in room
            Reset();
        }
        else
        {
            if (currentAnimationState == STATE_WALKD)
            {
                changeState(STATE_IDLED);
            }
            else if (currentAnimationState == STATE_WALKU)
            {
                changeState(STATE_IDLEU);
            }
            else if (currentAnimationState == STATE_WALKR)
            {
                changeState(STATE_IDLER);
            }
            else if (currentAnimationState == STATE_WALKL)
            {
                changeState(STATE_IDLEL);
            }
        }

        if (Input.GetKey("w"))
        {
            transform.Translate(0, speed * Time.deltaTime * 1.2f, 0);
        }
    }

    /*******************************************************************
    * Changes the animation state of the sprite
    * @param state integer corresponding to the new animation state
    ******************************************************************/
    void changeState(int state)
    {

        if (currentAnimationState == state)
            return;

        switch (state)
        {
            case STATE_IDLED:
                animator.SetInteger("state", STATE_IDLED);
                break;
            case STATE_IDLEU:
                animator.SetInteger("state", STATE_IDLEU);
                break;
            case STATE_IDLER:
                animator.SetInteger("state", STATE_IDLER);
                break;
            case STATE_IDLEL:
                animator.SetInteger("state", STATE_IDLEL);
                break;
            case STATE_WALKD:
                animator.SetInteger("state", STATE_WALKD);
                break;
            case STATE_WALKU:
                animator.SetInteger("state", STATE_WALKU);
                break;
            case STATE_WALKR:
                animator.SetInteger("state", STATE_WALKR);
                break;
            case STATE_WALKL:
                animator.SetInteger("state", STATE_WALKL);
                break;
        }
        currentAnimationState = state;
    }


    /*******************************************************************
    * Moves both player and main camera into adjacent room
    * Sets new reset position
    * @param dir String corresponding to which direction the player 
    * is moving into a new room
    ******************************************************************/
    void ShiftRoom(string dir)
    {
        if (dir.Equals("north"))
        {
            this.transform.Translate(0, 1.75f, 0);
            Camera.main.transform.Translate(0, 8, 0);
            borders[0].transform.Translate(0, 8, 0);
            borders[1].transform.Translate(0, 8, 0);
        }
        if (dir.Equals("east"))
        {
            transform.Translate(1.75f, 0, 0);
            Camera.main.transform.Translate(8, 0, 0);
            borders[0].transform.Translate(8, 0, 0);
            borders[1].transform.Translate(8, 0, 0);
        }
        if (dir.Equals("west"))
        {
            transform.Translate(-1.75f, 0, 0);
            Camera.main.transform.Translate(-8, 0, 0);
            borders[0].transform.Translate(-8, 0, 0);
            borders[1].transform.Translate(-8, 0, 0);
        }
        if (dir.Equals("south"))
        {
            this.transform.Translate(0, -1.75f, 0);
            Camera.main.transform.Translate(0, -8, 0);
            borders[0].transform.Translate(0, -8, 0);
            borders[1].transform.Translate(0, -8, 0);
        }

        // resets enemy position when player moves to a different room
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("Reset");
        }

    }

    /*******************************************************************
	 * Resets the player back to its original position, resets health,
     * and moves camera, map borders, and health icons
	 ******************************************************************/
    void Reset()
    {
        transform.position = init;
        health = 3;
        Camera.main.transform.position = camInit;
        borders[0].transform.position = leftInit;
        borders[1].transform.position = rightInit;
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
    {
        if (Input.GetKey("s"))
        {
            BonusWarp(coll);
        }

    }

    /*******************************************************************
	 * Sent when a collider on another object stops touching 
     * this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionExit2D()
    {

    }

    /*******************************************************************
	 * Warps the player to a new location based on which pipe they
     * go into
     * @param coll Collision2D associated with a pipe
	 ******************************************************************/
    private void BonusWarp(Collision2D coll)
    {
        if (coll.collider.tag == "pipe2" ||
            coll.collider.tag == "pipe3" ||
            coll.collider.tag == "pipe4")
        {
            character = GameObject.FindGameObjectWithTag("Player");
            character.SendMessage("Warp", coll.collider.tag);
            Destroy(gameObject);
        }
      
    }
}
