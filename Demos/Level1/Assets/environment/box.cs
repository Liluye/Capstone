using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {
	private int pushTime = 0;
	private Vector2 init;
	// Use this for initialization
	void Start () {
		init = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey("r")) {
			Reset();
		}
	}
	
	void Reset() {
		transform.position = init;
		
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		// note: freeze Z rotation must be checked within Unity
		if (coll.gameObject.tag == "Player") {
			if (pushTime > 30) {
				GetComponent<Rigidbody2D>().isKinematic = false;
			} 
			else {
				pushTime++;
			}
		}
		if (coll.gameObject.tag == "reset")
			Reset();
	}

	void OnCollisionExit2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Player") {
			GetComponent<Rigidbody2D>().isKinematic = true;
			pushTime = 0;
		}
	}
}
