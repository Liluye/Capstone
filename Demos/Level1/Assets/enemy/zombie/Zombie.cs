/*****************************************************************
Script to control the movement of the Zombie enemy.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{

    /** the animator connected to the zombie sprite */
    Animator animator;

    /** the speed at which the enemy travels */
    float speed = 2.5f;

    /** enemy animation states */
    const int STATE_IDLED = 0;
    const int STATE_IDLEU = 1;
    const int STATE_IDLER = 2;
    const int STATE_IDLEL = 3;
    const int STATE_WALKD = 4;
    const int STATE_WALKU = 5;
    const int STATE_WALKR = 6;
    const int STATE_WALKL = 7;

    /** current state of the animation (starts idle down) */
    int currentAnimationState = STATE_IDLED;

    /** starting position of the enemy sprite */
    private Vector2 init;

    /** the GameObject associated with the enemy */
    private GameObject enemy;

    /** the GameObject associated with the player */
    private GameObject play;

    /** the Transform (position) associated with the player */
    private Transform target;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {
        // define animator attached to enemy
        animator = this.GetComponent<Animator>();

        // define the enemy game object
        enemy = this.gameObject;

        // define the player
        play = GameObject.FindGameObjectWithTag("Player");

        // get player position
        target = play.transform;

        // get initial position of enemy
        init = transform.position;
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
     * Calls zombieMove() for specific zombie movement
	 ******************************************************************/
    void Move()
    {
        zombieMove();
    }

    /*******************************************************************
	 * Moves the zombie toward the player and changes the direction
     * it faces to the general direction of the player
	 ******************************************************************/
    void zombieMove()
    {
        // initial position of the enemy
        Vector2 initPos = transform.position;

        // move the enemy toward the player
        transform.position += (target.position - transform.position).normalized * speed / 4 * Time.deltaTime;

        // new position of the enemy
        Vector2 newPos = transform.position;

        // turn in the correct general direction of the enemy movement
        if (initPos.y < newPos.y && (initPos.y - newPos.y) > (initPos.x - newPos.x))
        {
            changeState(STATE_WALKU);

        }
        else if (initPos.y > newPos.y && (initPos.y - newPos.y) > (initPos.x - newPos.x))
        {
            changeState(STATE_WALKD);
        }
        else if (initPos.x > newPos.x && (initPos.y - newPos.y) < (initPos.x - newPos.x))
        {
            changeState(STATE_WALKL);
        }
        else if (initPos.x < newPos.x && (initPos.y - newPos.y) < (initPos.x - newPos.x))
        {
            changeState(STATE_WALKR);
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
	 * Resets the zombie back to its original position
	 ******************************************************************/
    void Reset()
    {
        transform.position = init;
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * On collision with a door, the sprite will call Reset()
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
    {
        // if the enemy tries to leave the room, reset their position
        if (coll.gameObject.tag == "northDoor" ||
            coll.gameObject.tag == "southDoor" ||
            coll.gameObject.tag == "westDoor" ||
            coll.gameObject.tag == "eastDoor")
        {
            Reset();
        }
    }
}
