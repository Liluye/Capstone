﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    Animator animator;

    // travel 2.5 grid squares per second
    float speed = 2.5f;

	// start position
	private Vector2 init;	
	
    // character animations states
    // note: states added within unity and tied to animation
    const int STATE_IDLED = 0;
    const int STATE_IDLEU = 1;
    const int STATE_IDLER = 2;
    const int STATE_IDLEL = 3;
    const int STATE_WALKD = 4;
    const int STATE_WALKU = 5;
    const int STATE_WALKR = 6;
    const int STATE_WALKL = 7;

    // character starts out in idle state
    int currentAnimationState = STATE_IDLED;
    // weapon and item could be two seperate objects
    // 0 = no item or weapon equipped
    int currentItem = 0;
    int currentWeapon = 0;
    //0 down, 1 left, 2 up, 3 right
    int facingDirection = 0;
    public GameObject boomerang, bomb;
    public GameObject itemNorth, itemWest, itemSouth, itemEast;
    private GameObject activeWeapon;
    

    // Use this for initialization
    void Start()
    {
        
		// game start position for reset
		init = transform.position;
		
		// define animator attached to character
        animator = this.GetComponent<Animator>();
		
    }

    // Update is called once per frame
    // note: FixedUpdate instead of Update keeps sprite from jittering on collisions
    void FixedUpdate()
    {
        Move();
        //Communicate with boomerang object to update your position
        UpdateBoomerang();
    }

    //Player movement, taken currently from arrow keys
    void Move()
    {
        if (Input.GetKey("w"))
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
            // player will punch
            // if a weapon is equipped, player will attack with that weapon

        }
        else if (Input.GetKey("e"))
        {
            // player will use currently equipped item
            useItem(currentItem);
        }
        //Hardcoded bomb drop key for now
        else if (Input.GetKeyUp("b"))
        {
            // player will use currently equipped item
            if (currentItem == 0)
            {
                currentItem = 1;
                Debug.Log("Item set to bomb");
            }
            else if (currentItem == 1)
            {
                currentItem = 0;
                Debug.Log("Item set to boomerang");
            }
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

    void changeState(int state)
    {
        // note: Has Exit Time must not be checked or animation will not loop
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

    void useItem(int item)
    {
        if (!activeWeapon)
        {
            if (item == 0)
            {
                //Inititialize based off facing direction: 0 down, 1 left, 2 up, 3 right
                activeWeapon = Instantiate(boomerang, SpawnItemLocation(facingDirection), new Quaternion()) as GameObject;
                activeWeapon.SendMessage("InitialDirection", facingDirection);
            }
            if (item == 1)
            {
                activeWeapon = Instantiate(bomb, SpawnItemLocation(facingDirection), new Quaternion()) as GameObject;
            }
        }
    }

    // Moves both player and main camera into adjacent room
	// Sets new reset position
    void ShiftRoom(string dir)
    {
        if (dir.Equals("north"))
        {
            this.transform.Translate(0, 2, 0);
            Camera.main.transform.Translate(0, 8, 0);
        }
        if (dir.Equals("east"))
        {
            transform.Translate(2, 0, 0);
            Camera.main.transform.Translate(8, 0, 0);
        }
        if (dir.Equals("west"))
        {
            transform.Translate(-2, 0, 0);
            Camera.main.transform.Translate(-8, 0, 0);
        }
        if (dir.Equals("south"))
        {
            this.transform.Translate(0, -2, 0);
            Camera.main.transform.Translate(0, -8, 0);
        }
		init = this.transform.position;
    }
	
	void Reset() {
		transform.position = init;
		
	}

    void OnCollisionStay2D(Collision2D coll)
    {
        // note: freeze Z rotation must be checked within Unity
        if (coll.gameObject.tag == "northDoor")
            ShiftRoom("north");
        if (coll.gameObject.tag == "eastDoor")
            ShiftRoom("east");
        if (coll.gameObject.tag == "westDoor")
            ShiftRoom("west");
        if (coll.gameObject.tag == "southDoor")
            ShiftRoom("south");
    }
	
	void OnCollisionExit2D() 
	{

	}

    private void UpdateBoomerang()
    {
        if (activeWeapon != null && activeWeapon.gameObject.tag == "Boomerang")
        {
                activeWeapon.SendMessage("UpdateLocation", transform.position);
         
        }
    }

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
                //something went wrong, do nothing
                return new Vector3(0,0,0);
        }
    }
}