using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    Animator animator;

    //travel 2.5 grid squares per second
    float speed = 2.5f;
    // character animations states
    // note: states added within unity and tied to animation
    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_2PUNCH = 2;
    // character starts out in idle state
    int currentAnimationState = STATE_IDLE;
    // weapon and item could be two seperate objects
    // 0 = no item or weapon equipped
    int currentItem = 0;
    int currentWeapon = 0;

    // Use this for initialization
    void Start()
    {
        // define animator attached to character
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    // note: FixedUpdate instead of Update keeps sprite from jittering on collisions
    void FixedUpdate()
    {
        Move();
    }

    //Player movement, taken currently from arrow keys
    void Move()
    {
        if (Input.GetKey("w"))
        {
            changeState(STATE_WALK);
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey("s"))
        {
            changeState(STATE_WALK);
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey("a"))
        {
            changeState(STATE_WALK);
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("d"))
        {
            changeState(STATE_WALK);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("space"))
        {
            // player will punch
            // if a weapon is equipped, player will attack with that weapon
            changeState(STATE_2PUNCH);
        }
        else if (Input.GetKey("e"))
        {
            // player will use currently equipped item
            useItem(currentItem);
        }
        else
        {
            changeState(STATE_IDLE);
        }
    }

    void changeState(int state)
    {
        // note: Has Exit Time must not be checked or animation will not loop
        if (currentAnimationState == state)
            return;
        if (state == STATE_IDLE)
        {
            animator.SetInteger("state", STATE_IDLE);
        }
        else if (state == STATE_WALK)
        {
            animator.SetInteger("state", STATE_WALK);
        }
        else if (state == STATE_2PUNCH)
        {
            animator.SetInteger("state", STATE_2PUNCH);
        }

        currentAnimationState = state;
    }

    void useItem(int item)
    {

    }

    //Moves both player and main camera into adjacent room
    void ShiftRoom(string dir)
    {
        if (dir.Equals("north"))
        {
            this.transform.Translate(0, 3, 0);
            Camera.main.transform.Translate(0, 10, 0);
        }
        if (dir.Equals("east"))
        {
            transform.Translate(3, 0, 0);
            Camera.main.transform.Translate(10, 0, 0);
        }
        if (dir.Equals("west"))
        {
            transform.Translate(-3, 0, 0);
            Camera.main.transform.Translate(-10, 0, 0);
        }
        if (dir.Equals("south"))
        {
            this.transform.Translate(0, -3, 0);
            Camera.main.transform.Translate(0, -10, 0);
        }

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
}
