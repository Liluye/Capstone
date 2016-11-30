/*****************************************************************
Script to control the movement of the fire used by the skeleton.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

    /** the speed at which the fire travels */
    float speed = 2.5f;

    /** the Transform (position) associated with the player */
    private Transform target;

    /** the GameObject associated with the player */
    private GameObject play;

    /** starting position of the fire sprite */
    private Vector2 init;

    /** starting position of the player sprite */
    Vector3 playerPos;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {
        // define the player
        play = GameObject.FindGameObjectWithTag("Player");

        // get current player position
        target = play.transform;
        playerPos = target.position;

        // get the current position of the fire sprite
        init = transform.position;

        // create array of all objects tagged "enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject enemy in enemies)
        {
            // find the skeleton enemy (enemy1)
            if (enemy == GameObject.Find("enemy1"))
            {
                // ignore collisions between fire and skeleton
                Physics.IgnoreCollision(GetComponent<Collider>(), enemy.GetComponent<Collider>(), true);
            }
        }
        
    }

    /*******************************************************************
	 * Method called once per frame to update sprite
     * Calls ShootAtPlayer() for fire movement
	 ******************************************************************/
    void Update ()
    {
        ShootAtPlayer();
	}

    /*******************************************************************
	 * Directs the fire sprite toward the player
     * Calls Reset() if the player is hit or if the sprite gets
     * too far away from the original position
	 ******************************************************************/
    public void ShootAtPlayer()
    {
        // move the fire toward the player
        transform.position += (playerPos - transform.position).normalized * speed / 2 * Time.deltaTime;

        Vector3 newPos = transform.position;

        // if the player is hit or fire gets too far away from
        // initial position, reset
        if(newPos.x > playerPos.x - .15 &&
            newPos.x < playerPos.x + .15 &&
            newPos.y > playerPos.y - .15 &&
            newPos.y < playerPos.y + .15 ||
            (newPos.x < init.x - 2.5 ||
            newPos.x > init.x + 2.5 ||
            newPos.y < init.y - 2.5 ||
            newPos.y > init.y + 2.5))
        {
            Reset();
        }
    }

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * Calls Reset() to reset the position of the fire
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
    {
        Reset();
    }

    /*******************************************************************
	 * Resets the fire back to its original position
	 ******************************************************************/
    void Reset()
    {
        transform.position = init;

        // update the player's current position
        playerPos = target.position;
    }
}
