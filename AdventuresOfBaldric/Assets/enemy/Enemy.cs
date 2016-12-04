/*****************************************************************
Script to control the movement of the enemies (NOT USED).

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    /** the animator connected to the sprite */
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

    /** movement counter */
    private int rndmMove = 0;

    /** the GameObject associated with the player */
    private GameObject play;

    /** the Transform (position) associated with the player */
    private Transform target;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {
        // define animator attached to character
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
     * Calls methods based on what the enemy type is
	 ******************************************************************/
    void Move()
    {
        // if enemy is a skeleton
        // move to face in the direction of the player
        if (enemy == GameObject.Find("enemy1"))
        {
            skeletonMove();
        }
        // if enemy is a zombie
        // chase the player
        else if (enemy == GameObject.Find("enemy2"))
        {
            zombieMove();
        }
        // if enemy is a rat
        // move in a randomly created direction/movement
        else if (enemy == GameObject.Find("enemy3"))
        {
            ratMove();
        }
    }

    /*******************************************************************
	 * Moves the skeleton to face in the general direction of the
     * player
	 ******************************************************************/
    void skeletonMove()
    {
        if (target.position.x >= transform.position.x && target.position.y < transform.position.y + 1 && target.position.y > transform.position.y - 1)
        {
            changeState(STATE_IDLER);
        }
        else if (target.position.y < transform.position.y && target.position.x < transform.position.x + 1 && target.position.x > transform.position.x - 1)
        {
            changeState(STATE_IDLED);
        }
        else if (target.position.y >= transform.position.y && target.position.x < transform.position.x + 1 && target.position.x > transform.position.x - 1)
        {
            changeState(STATE_IDLEU);
        }
        else if (target.position.x < transform.position.x && target.position.y < transform.position.y + 1 && target.position.y > transform.position.y - 1)
        {
            changeState(STATE_IDLEL);
        }
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
	 * Moves the rat sprite in random directions
	 ******************************************************************/
    void ratMove()
    {
        int movement = Random.Range(4, 8);
        if (rndmMove > 75)
        {
            switch (movement)
            {
                case STATE_WALKD:
                    changeState(STATE_WALKD);
                    transform.Translate(0, -speed * Time.deltaTime, 0);
                    break;
                case STATE_WALKU:
                    changeState(STATE_WALKU);
                    transform.Translate(0, speed * Time.deltaTime, 0);
                    break;
                case STATE_WALKR:
                    changeState(STATE_WALKR);
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                    break;
                case STATE_WALKL:
                    changeState(STATE_WALKL);
                    transform.Translate(-speed * Time.deltaTime, 0, 0);
                    break;
            }
            rndmMove = 0;
        }
        else
        {
            switch (currentAnimationState)
            {
                case STATE_WALKD:
                    transform.Translate(0, -speed * Time.deltaTime, 0);
                    break;
                case STATE_WALKU:
                    transform.Translate(0, speed * Time.deltaTime, 0);
                    break;
                case STATE_WALKR:
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                    break;
                case STATE_WALKL:
                    transform.Translate(-speed * Time.deltaTime, 0, 0);
                    break;
            }
            rndmMove++;
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
	 * Resets the enemy back to its original position
	 ******************************************************************/
    void Reset()
    {
        transform.position = init;
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
    {
        // if the enemy tries to leave the room, reset their position
        if (coll.gameObject.tag == "northDoor" ||
            coll.gameObject.tag == "southDoor" ||
            coll.gameObject.tag == "westDoor" ||
            coll.gameObject.tag == "eastDoor" &&
            enemy != GameObject.Find("enemy3"))
        {
            Reset();
        } 

        // if a rat runs into something, change direction
        if (enemy == GameObject.Find("enemy3"))
        {
            rndmMove = 100;
            ratMove();
        }
    }
}
