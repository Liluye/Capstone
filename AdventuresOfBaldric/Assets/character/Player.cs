/*****************************************************************
Script to control the movement of the Player.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
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

    /** location of map borders */
	private Vector2 leftInit;
	private Vector2 rightInit;
	
	/** game objects for the health icons */
	private GameObject h1;
	private GameObject h2;
	private GameObject h3;

    /** positions of the health icons */
	private Vector2 h1Init;
	private Vector2 h2Init;
	private Vector2 h3Init;

    /** player animation states */
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

    /** player health */
    int health;
	
	/** determines current invulnerability */
	bool invulnerable = false;

	/** the item currently equipped by the player */
    int currentItem = 0;

    /** current direction (0 down, 1 left, 2 up, 3 right) */
    int facingDirection = 0;

    /** game objects associated with items */
    public GameObject boomerang, bomb, grapplingHook, sword, bonusCharacter;
    public GameObject itemNorth, itemWest, itemSouth, itemEast;

    /** the weapon currently equipped */
    private GameObject activeWeapon;
    private GameObject activeBonusChar;
	private Vector3 grappleLoc;
    private bool grappling;
	private bool grapplingHookActive;
	private Collider2D water;
    private Rigidbody2D rb;
    private bool warped = false;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
    {
        // get initial position of player
        init = transform.position;

        // get the start position for camera
		camInit = Camera.main.transform.position;
		
		// define animator attached to player
        animator = this.GetComponent<Animator>();
		
		// get borders so they can be moved
		borders = GameObject.FindGameObjectsWithTag("border");
		leftInit = borders[0].transform.position;
		rightInit = borders[1].transform.position;
		
		// get health icons
		h1 = GameObject.FindWithTag("health1");
		h1Init = h1.transform.position;
		h2 = GameObject.FindWithTag("health2");
		h2Init = h2.transform.position;
		h3 = GameObject.FindWithTag("health3");
		h3Init = h3.transform.position;

        // set player's health
        health = 3;
		
		//Find the river in the level
		water = GameObject.FindWithTag("water").GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    /*******************************************************************
	 * Method called once per frame to update items
	 ******************************************************************/
    void Update()
    {
        if (Input.GetKeyUp("b"))
        {
            // player will use currently equipped item
            if (currentItem == 0)
            {
                currentItem = 1;
                Debug.Log("Item set to bomb");
            }
            else if (currentItem == 1)
            {
                currentItem = 2;
                Debug.Log("Item set to grappling hook");
            }
            else if (currentItem == 2)
            {
                currentItem = 0;
                Debug.Log("Item set to boomerang");
            }
        }
    }

    /*******************************************************************
	 * Method called once per frame to update sprite
	 ******************************************************************/
    void FixedUpdate()
    {

        if (!warped)
            Move();

		UpdateWeapons();

        // water collision is active while not grappling
		if (!grappling) {
			Physics2D.IgnoreCollision(water, this.GetComponent<BoxCollider2D>(), false);
			Physics2D.IgnoreCollision(water, this.GetComponent<CircleCollider2D>(), false);
		}

        // check current health and update health sprites
		switch(health) {
			case 3:
				h1.GetComponent<SpriteRenderer>().enabled = true;
				h2.GetComponent<SpriteRenderer>().enabled = true;
				h3.GetComponent<SpriteRenderer>().enabled = true;
				break;
			case 2:
				h1.GetComponent<SpriteRenderer>().enabled = true;
				h2.GetComponent<SpriteRenderer>().enabled = true;
				h3.GetComponent<SpriteRenderer>().enabled = false;
				break;
			case 1:
				h1.GetComponent<SpriteRenderer>().enabled = true;
				h2.GetComponent<SpriteRenderer>().enabled = false;
				h3.GetComponent<SpriteRenderer>().enabled = false;
				break;
			case 0 :
				h1.GetComponent<SpriteRenderer>().enabled = false;
				h2.GetComponent<SpriteRenderer>().enabled = false;
				h3.GetComponent<SpriteRenderer>().enabled = false;
				break;
		}
    }

    /*******************************************************************
	 * Method that moves the sprite
     * Takes input from the keyboard to move, use items, or
     * interact with the environment
     * Game uses WASD for directional movement
	 ******************************************************************/
    void Move()
    {
        // continue to grapple if still grappling
        if (grapplingHookActive)
        {
            if (grappling)
            {
                Grapple();
            }
        }
		else if (Input.GetKey("w"))
        {
            facingDirection = 2;
			changeState(STATE_WALKU);
            transform.Translate(0, speed * Time.deltaTime, 0);
			
        }
        else if (Input.GetKey("s"))
        {
			facingDirection = 0;
			changeState(STATE_WALKD);
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey("a"))
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
        else if (Input.GetKey("space"))
        {
            // player will swing their sword
            useItem(100);
        }
        else if (Input.GetKey("e"))
        {
            // player will use currently equipped item
            useItem(currentItem);
        }
        
        else if (Input.GetKey("r"))
		{
			// return player to start position in room
			Reset();
		}
		else
        {
            if(currentAnimationState == STATE_WALKD)
            {
                changeState(STATE_IDLED);
            } else if (currentAnimationState == STATE_WALKU)
            {
                changeState(STATE_IDLEU);
            } else if (currentAnimationState == STATE_WALKR)
            {
                changeState(STATE_IDLER);
            } else if (currentAnimationState == STATE_WALKL)
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
    * Assigns the current weapon
    * @param item integer corresponding to the item being assigned
    ******************************************************************/
    void useItem(int item)
    {
        if (!activeWeapon)
        {
            switch (item)
            {
                case 0:
                    {
                        // inititialize based off facing direction: 0 down, 1 left, 2 up, 3 right
                        activeWeapon = Instantiate(boomerang, SpawnItemLocation(facingDirection), new Quaternion()) as GameObject;
                        activeWeapon.SendMessage("InitialDirection", facingDirection);
                        break;
                    }
                case 1:
                    {
                        activeWeapon = Instantiate(bomb, SpawnItemLocation(facingDirection), new Quaternion()) as GameObject;
                        break;
                    }

                case 2:
                    {
                        // inititialize based off facing direction: 0 down, 1 left, 2 up, 3 right
                        activeWeapon = Instantiate(grapplingHook, SpawnItemLocation(facingDirection), new Quaternion()) as GameObject;
                        activeWeapon.SendMessage("InitialDirection", facingDirection);
                        break;
                    }

                case 100:
                    {
                        activeWeapon = Instantiate(sword, transform.position, new Quaternion()) as GameObject;
                        activeWeapon.SendMessage("InitialDirection", facingDirection);
                        break;
                    }
            }
        }
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
			h1.transform.Translate(0, 8, 0);
			h2.transform.Translate(0, 8, 0);
			h3.transform.Translate(0, 8, 0);
        }
        if (dir.Equals("east"))
        {
            transform.Translate(1.75f, 0, 0);
            Camera.main.transform.Translate(8, 0, 0);
			borders[0].transform.Translate(8, 0, 0);
			borders[1].transform.Translate(8, 0, 0);
			h1.transform.Translate(8, 0, 0);
			h2.transform.Translate(8, 0, 0);
			h3.transform.Translate(8, 0, 0);
        }
        if (dir.Equals("west"))
        {
            transform.Translate(-1.75f, 0, 0);
            Camera.main.transform.Translate(-8, 0, 0);
			borders[0].transform.Translate(-8, 0, 0);
			borders[1].transform.Translate(-8, 0, 0);
			h1.transform.Translate(-8, 0, 0);
			h2.transform.Translate(-8, 0, 0);
			h3.transform.Translate(-8, 0, 0);
        }
        if (dir.Equals("south"))
        {
            this.transform.Translate(0, -1.75f, 0);
            Camera.main.transform.Translate(0, -8, 0);
			borders[0].transform.Translate(0, -8, 0);
			borders[1].transform.Translate(0, -8, 0);
			h1.transform.Translate(0, -8, 0);
			h2.transform.Translate(0, -8, 0);
			h3.transform.Translate(0, -8, 0);
        }
        
        else if (dir.Equals("secret"))		
        {		
            Camera.main.transform.Translate(-8, -8, 0);		
            borders[0].transform.Translate(-8, -8, 0);		
            borders[1].transform.Translate(-8, -8, 0);	
            h1.transform.Translate(-8, -8, 0);
			h2.transform.Translate(-8, -8, 0);
			h3.transform.Translate(-8, -8, 0);            
            warped = true;		
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            activeBonusChar = Instantiate(bonusCharacter, new Vector3(-19.16f, -12.29f, 0), new Quaternion()) as GameObject;		
        }

        // resets enemy position when player moves to a different room
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] fires = GameObject.FindGameObjectsWithTag("fire");
        foreach(GameObject enemy in enemies)
        {
            enemy.SendMessage("Reset");
        }
        foreach(GameObject fire in fires)
        {
            fire.SendMessage("Reset");
        }
		//init = this.transform.position;
    }

    /*******************************************************************
	 * Resets the player back to its original position, resets health,
     * and moves camera, map borders, and health icons
	 ******************************************************************/
    void Reset() {
		transform.position = init;
        health = 3;
		Camera.main.transform.position = camInit;
		borders[0].transform.position = leftInit;
		borders[1].transform.position = rightInit;
		h1.transform.position = h1Init;
		h2.transform.position = h2Init;
		h3.transform.position = h3Init;
        if (activeWeapon != null)
            Destroy(activeWeapon);
	}

    /*******************************************************************
	 * Sent when an incoming collider makes contact with 
     * this object's collider
     * Ignore collisions with water
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "water" && grappling) {
			Physics2D.IgnoreCollision(coll.collider, this.GetComponent<BoxCollider2D>());
			Physics2D.IgnoreCollision(coll.collider, this.GetComponent<CircleCollider2D>());
		}
	}

    /*******************************************************************
	 * Sent each frame where a collider on another object 
     * is touching this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionStay2D(Collision2D coll)
    {
        // if the player is grappling and collides with something,
        // break the grapple
		if (grappling)
        {
            if (activeWeapon != null)
            {
                activeWeapon.SendMessage("BreakGrapple");
                grappling = false;
				Physics2D.IgnoreCollision(water, this.GetComponent<BoxCollider2D>(), false);
				Physics2D.IgnoreCollision(water, this.GetComponent<CircleCollider2D>(), false);
            }
        }

        // move the current room
        if (coll.gameObject.tag == "northDoor")
            ShiftRoom("north");
        if (coll.gameObject.tag == "eastDoor")
            ShiftRoom("east");
        if (coll.gameObject.tag == "westDoor")
            ShiftRoom("west");
        if (coll.gameObject.tag == "southDoor")
            ShiftRoom("south");
        if (coll.gameObject.tag == "secretEnt" && !warped)
            ShiftRoom("secret");

		if (!invulnerable){
			// reset the player's position if they lose all health
			if (coll.gameObject.tag == "enemy" || coll.gameObject.tag == "fire")
			{
				health--;
				if(health == 0)
				{
					Reset();
				}
				invulnerable = true;
				Invoke("SetVulnerable", 1.5f);
			}
		}
	}

    /*******************************************************************
	 * Sent when a collider on another object stops touching 
     * this object's collider
     * @param coll the Collision2D data associated with this collision
	 ******************************************************************/
    void OnCollisionExit2D(Collision2D coll) 
	{
		
	}

    /*******************************************************************
	 * Updated the current weapon being used
	 ******************************************************************/
    private void UpdateWeapons()
    {
        if (activeWeapon != null)
        {
            // if the current item being used is a boomerang,
            // update the location of the sprite
            if (activeWeapon.gameObject.tag == "Boomerang")
            {
                activeWeapon.SendMessage("UpdateLocation", transform.position);
            }

            // if the current item being used is the grappling hook,
            // update the grapple action
            if (activeWeapon.gameObject.tag == "GrapplingHook")
            {
                grapplingHookActive = true;
                grapplingHookAction gha = activeWeapon.GetComponent<grapplingHookAction>();
                rb.isKinematic = true;
                if (gha.getGrapple())
                {
                    if (!grappling)
                    {
                        grappleLoc = gha.getGrappleLocation();
                    }
                    grappling = true;
                }
            }
        }
		else
        {
            grapplingHookActive = false;
            grappling = false;
            rb.isKinematic = false;
        }
    }

    /*******************************************************************
	 * Updates the player's position while using a grappling hook
	 ******************************************************************/
    private void Grapple()
    {
        if (activeWeapon != null)
            activeWeapon.SendMessage("playerLocation", transform.position);
        transform.position = Vector2.Lerp(transform.position, grappleLoc, Time.deltaTime);
        if (Vector3.SqrMagnitude(transform.position - grappleLoc) < 0.2)
        {
            grappling = false;
        }
    }

    /*******************************************************************
	 * Returns the position of the item
     * @param direction Integer for item direction
     * @return Vector3 item position
	 ******************************************************************/
    private Vector3 SpawnItemLocation(int direction)
    {
        switch (direction)
        {
            case 0:
                return itemSouth.transform.position;
            case 1:
                return itemWest.transform.position;
            case 2:
                return itemNorth.transform.position;
            case 3:
                return itemEast.transform.position;
            default:
                // something went wrong, do nothing
                return new Vector3(0,0,0);
        }
    }

    /*******************************************************************
	 * Sets vulnerability/invulnerability for player
	 ******************************************************************/
    private void SetVulnerable() 
	{
		invulnerable = false;
	}

    /*******************************************************************
	 * Sets the current item being used
     * @param item Integer associted with an item
	 ******************************************************************/
    public void SetItem(int item)
    {
        currentItem = item;
    }

    /*******************************************************************
	 * Warps the player to a new location based on which pipe they
     * go into
     * @param pipeNum String associated with a pipe
	 ******************************************************************/
    public void Warp(string pipeNum)
    {
        warped = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        Camera.main.transform.position = camInit;
        borders[0].transform.position = leftInit;
        borders[1].transform.position = rightInit;
        h1.transform.position = h1Init;
        h2.transform.position = h2Init;
        h3.transform.position = h3Init;
        switch (pipeNum)
        {
            case "pipe2":
                transform.position = new Vector3(-8.5f, -2.25f, 0);
                Camera.main.transform.Translate(0, 8, 0);
                borders[0].transform.Translate(0, 8, 0);
                borders[1].transform.Translate(0, 8, 0);
                h1.transform.Translate(0, 8, 0);
                h2.transform.Translate(0, 8, 0);
                h3.transform.Translate(0, 8, 0);
                break;

            case "pipe3":
                transform.position = new Vector3(-8.5f, 5.5f, 0);
                Camera.main.transform.Translate(0, 16, 0);
                borders[0].transform.Translate(0, 16, 0);
                borders[1].transform.Translate(0, 16, 0);
                h1.transform.Translate(0, 16, 0);
                h2.transform.Translate(0, 16, 0);
                h3.transform.Translate(0, 16, 0);
                break;

            case "pipe4":
                transform.position = new Vector3(-2.5f, 7.8f, 0);
                Camera.main.transform.Translate(8, 16, 0);
                borders[0].transform.Translate(8, 16, 0);
                borders[1].transform.Translate(8, 16, 0);
                h1.transform.Translate(8, 16, 0);
                h2.transform.Translate(8, 16, 0);
                h3.transform.Translate(8, 16, 0);
                break;

        }
    }
}
