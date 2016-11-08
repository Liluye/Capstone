using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    Animator animator;

    //travel 2.5 grid squares per second
    float speed = 2.5f;

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

    // start position
    private Vector2 init;

    private GameObject play;
    private Transform target;
    private Animation playerAnim;

    // Use this for initialization
    void Start()
    {
        // define animator attached to character
        animator = this.GetComponent<Animator>();

        // define the player
        play = GameObject.FindGameObjectWithTag("Player");
        target = play.transform;

        // game start position for reset
        init = transform.position;
    }

    // Update is called once per frame
    // note: FixedUpdate instead of Update keeps sprite from jittering on collisions
    void FixedUpdate()
    {
        Vector2 p = target.position;
        // check if the player is in the room
        // currently fixed to distance enemy and player are apart
        // NEED TO CHANGE
        if (Vector2.Distance(p, transform.position) > 4)
        {
            Reset();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        // initial position of the enemy
        Vector2 initPos = transform.position;
        // move the enemy toward the player
        transform.position += (target.position - transform.position).normalized * speed/3 * Time.deltaTime;
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

    void Reset()
    {
        transform.position = init;
    }

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
