using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {
	private int pushLength;
	float speed = 2.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		// note: freeze Z rotation must be checked within Unity
		if (pushLength > 30) {
			if (coll.gameObject.tag == "Player") {
				if (Input.GetKey ("w")) {
					transform.Translate (0, speed * Time.deltaTime, 0);
				} else if (Input.GetKey ("s")) {
					transform.Translate (0, -speed * Time.deltaTime, 0);
				} else if (Input.GetKey ("a")) {
					transform.Translate (-speed * Time.deltaTime, 0, 0);
				} else if (Input.GetKey ("d")) {
					transform.Translate (speed * Time.deltaTime, 0, 0);
				}
			}
		} else
			pushLength++;	
			
	}

	void OnCollisionExit2D() 
	{
		pushLength = 0;
	}
}
