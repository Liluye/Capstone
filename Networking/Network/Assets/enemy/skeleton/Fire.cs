using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
    //travel 2.5 grid squares per second
    float speed = 2.5f;
    private Transform target;
    private GameObject play;
    // start position
    private Vector2 init;

    Vector3 playerPos;

    // Use this for initialization
    void Start()
    {
        // define the player
        play = GameObject.FindGameObjectWithTag("Player");
        // get player position
        target = play.transform;
        playerPos = target.position;

        init = transform.position;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject enemy in enemies)
        {
            if (enemy == GameObject.Find("enemy1"))
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), enemy.GetComponent<Collider>(), true);
            }
        }
        
    }

    // Update is called once per frame
    void Update ()
    {
        ShootAtPlayer();
	}

    public void ShootAtPlayer()
    {
        // move the fire toward the player
        transform.position += (playerPos - transform.position).normalized * speed / 2 * Time.deltaTime;

        Vector3 newPos = transform.position;

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


    void OnCollisionStay2D(Collision2D coll)
    {
        Reset();
    }

    void Reset()
    {
        transform.position = init;
        playerPos = target.position;
    }
}
