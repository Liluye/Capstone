using UnityEngine;
using System.Collections;

public class Rat : MonoBehaviour
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

    private GameObject enemy;
    private int rndmMove = 0;
    private GameObject play;
    private Transform target;
    private Animation playerAnim;

    // Use this for initialization
    void Start()
    {
        // define animator attached to character
        animator = this.GetComponent<Animator>();

        enemy = this.gameObject;

        // define the player
        play = GameObject.FindGameObjectWithTag("Player");
        // get player position
        target = play.transform;

        // game start position for reset
        init = transform.position;
    }

    // Update is called once per frame
    // note: FixedUpdate instead of Update keeps sprite from jittering on collisions
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        ratMove();
    }

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

            rndmMove = 100;
            ratMove();

    }
}
