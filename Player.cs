using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    float speed = 2.5f;
    float test = 2;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //Player movement, taken currently from arrow keys
    //May change to wsad later
    void Move()
    {
        if (Input.GetKey("up"))
            transform.Translate(0, speed * Time.deltaTime, 0);
        if (Input.GetKey("down"))
            transform.Translate(0, -speed * Time.deltaTime, 0);
        if (Input.GetKey("left"))
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        if (Input.GetKey("right"))
            transform.Translate(speed * Time.deltaTime, 0, 0);
    }


}
