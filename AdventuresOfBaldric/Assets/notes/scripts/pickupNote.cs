using UnityEngine;
using System.Collections;

public class pickupNote : MonoBehaviour {
	string note;

	void OnTriggerEnter2D(Collider2D other){
		GameObject impactor = other.gameObject;


		if (impactor.CompareTag("Player") && impactor.name.Equals("character")) {
			Time.timeScale = 0.0f;
			GameObject.Find ("Canvas").GetComponent<notePanel> ().activateReadPanel (note);
			Destroy (gameObject);
		}
	}

	public void setNote(string note){
		this.note = note;
	}
}
