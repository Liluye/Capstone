/*****************************************************************
Script to control box functionality.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {

    /** time until box movement */
    private int pushTime = 0;

    /** starting position of the box */
    private Vector2 init;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start () {

        // get initial position of box
        init = transform.position;

	}

    /*******************************************************************
	 * Method called once per frame to check key inputs
	 ******************************************************************/
    void FixedUpdate () {

		if (Input.GetKey("r")) {
			Reset();
		}

	}

    /*******************************************************************
	 * Resets the box back to its original position
	 ******************************************************************/
    void Reset() {

		transform.position = init;
		
	}

    /*******************************************************************
	 * Sent each frame where another object is within a trigger 
     * collider attached to this object
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnTriggerStay2D(Collider2D coll) {

        // destroy the object associated with this switch and box
		if (this.name == "BoxSwitch" && coll.gameObject.tag == "box"){
			GameObject des = GameObject.Find("disappear1");
			Destroy(des);		
		}

        // destroy the object associated with this switch and boomerang
        if (this.name == "bSwitch" && coll.gameObject.tag == "Boomerang") {
			GameObject des = GameObject.Find("disappear2");
			Destroy(des);
		}

	}

    /*******************************************************************
	 * Sent when an incoming collider makes contact with 
     * this object's collider
     * Ignore collisions with the grappling hook
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionEnter2D(Collision2D coll) 
	{

        // ignore collision with grappling hook
		if (coll.gameObject.tag == "GrapplingHook") {
			Physics2D.IgnoreCollision(coll.collider, this.GetComponent<BoxCollider2D>());
		}

	}

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
	{

        // alows player to push the box
		if (this.tag == "box" && coll.gameObject.tag == "Player") {
			if (pushTime > 30) {
				GetComponent<Rigidbody2D>().isKinematic = false;
			} 
			else {
				pushTime++;
			}
		}

        // destroy wall if a bomb is using on it
		if (this.tag == "bombWall" && coll.gameObject.tag == "Bomb") {
			Destroy(gameObject);
		}

        // resets player if they step on a reset block
		if (coll.gameObject.tag == "reset")
			Reset();
	}

    /*******************************************************************
	 * Sent when a collider on another object stops touching 
     * this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionExit2D(Collision2D coll) 
	{

        // allows player to push the box
		if (this.tag == "box" && coll.gameObject.tag == "Player") {
			GetComponent<Rigidbody2D>().isKinematic = true;
			pushTime = 0;
		}
		
	}
}
