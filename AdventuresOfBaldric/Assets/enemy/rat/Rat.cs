/*****************************************************************
Script to control the movement of the Rat enemy.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class Rat : MonoBehaviour
{

    /** the animator connected to the rat sprite */
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
     * Calls Move() for sprite movement
	 ******************************************************************/
    void FixedUpdate()
    {
        Move();
    }

    /*******************************************************************
	 * Method that moves the sprite
     * Calls ratMove() for specific rat movement
	 ******************************************************************/
    void Move()
    {
        ratMove();
    }

    /*******************************************************************
	 * Moves the rat sprite in random directions
	 ******************************************************************/
    void ratMove()
    {
        // create a random movement for the rat
        int movement = Random.Range(4, 8);

        // if the rat hasn't changed direction recently or collides,
        // change the direction
        if (rndmMove > 75)
        {
            // determine animation direction based on random int
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

            // reset the movement counter
            rndmMove = 0;
        }
        else
        {
            // continue moving in the same direction
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

            // increment the movement counter
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
	 * Resets the rat back to its original position
	 ******************************************************************/
    void Reset()
    {
        transform.position = init;
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * On collision, the rat will change direction
     * Calls ratMove() to update movement
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
    {
        // change movement counter to change direction
        rndmMove = 100;
        ratMove();
    }

    /*******************************************************************
	 * Sent when an incoming collider makes contact with 
     * this object's collider
     * If a sword hits the rat, destory it
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Sword")
			Destroy(gameObject);
	}
}
