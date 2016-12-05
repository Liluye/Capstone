/*****************************************************************
Script for picking up notes.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class pickupNote : MonoBehaviour {

    /** the note that is picked up */
	string note;

    /*******************************************************************
	 * Sent when another object enters a trigger collider 
     * attached to this object
     * @param other The other Collider2D involved in this collision.
	 ******************************************************************/
    void OnTriggerEnter2D(Collider2D other){

		GameObject impactor = other.gameObject;

		if (impactor.CompareTag("Player") && impactor.name.Equals("character")) {
			Time.timeScale = 0.0f;
			GameObject.Find ("Canvas").GetComponent<notePanel> ().activateReadPanel (note);
			Destroy (gameObject);
		}

	}

    /*******************************************************************
	 * Picks up the note
     * @param note String for the note that is picked up
	 ******************************************************************/
    public void setNote(string note){
		this.note = note;
	}
}
