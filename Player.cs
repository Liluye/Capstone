using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //travel 2.5 grid squares per second
    float speed = 2.5f;

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
