using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    Animator animator;

    //travel 2.5 grid squares per second
    float speed = 2.5f;
    // character animations states
    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_2PUNCH = 2;

    int currentAnimationState = STATE_IDLE;

    // Use this for initialization
    void Start()
    {
        // define animator attached to character
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //Player movement, taken currently from arrow keys
    //May change to wsad later
    void Move()
    {
        if (Input.GetKey("up"))
        {
            changeState(STATE_WALK);
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey("down"))
        {
            changeState(STATE_WALK);
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey("left"))
        {
            changeState(STATE_WALK);
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("right"))
        {
            changeState(STATE_WALK);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("space"))
        {
            changeState(STATE_2PUNCH);
        }
        else
        {
            changeState(STATE_IDLE);
        }
    }

    void changeState(int state)
    {
        if (currentAnimationState == state)
            return;
        if(state == STATE_IDLE)
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
