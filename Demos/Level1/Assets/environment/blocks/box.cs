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
		this.GetComponent<SpriteRenderer>().enabled = true;
	}
	
	void Reset() {
		transform.position = init;
		
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (this.name == "BoxSwitch" && coll.gameObject.tag == "box"){
			GameObject des = GameObject.Find("disappear1");
			Destroy(des);		
		}
		if (this.name == "bSwitch" && coll.gameObject.tag == "Boomerang") {
			GameObject des = GameObject.Find("disappear2");
			Destroy(des);
		}
	}
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "GrapplingHook") {
			Physics2D.IgnoreCollision(coll.collider, this.GetComponent<BoxCollider2D>());
		}
	}
	void OnCollisionStay2D(Collision2D coll)
	{
		// note: freeze Z rotation must be checked within Unity
		if (this.tag == "box" && coll.gameObject.tag == "Player") {
			if (pushTime > 30) {
				GetComponent<Rigidbody2D>().isKinematic = false;
			} 
			else {
				pushTime++;
			}
		}
		if (this.tag == "bombWall" && coll.gameObject.tag == "Bomb") {
			Destroy(gameObject);
		}
		if (coll.gameObject.tag == "reset")
			Reset();
	}

	void OnCollisionExit2D(Collision2D coll) 
	{
		if (this.tag == "box" && coll.gameObject.tag == "Player") {
			GetComponent<Rigidbody2D>().isKinematic = true;
			pushTime = 0;
		}
		
	}
}
