using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {
	private int pushTime;
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
		if (coll.gameObject.tag == "Player") {
			if (pushTime > 30) {
				GetComponent<Rigidbody2D>().isKinematic = false;
			} 
			else {
				pushTime++;
			}
		}
			
	}

	void OnCollisionExit2D() 
	{
		GetComponent<Rigidbody2D>().isKinematic = true;
		pushTime = 0;
	}
}
