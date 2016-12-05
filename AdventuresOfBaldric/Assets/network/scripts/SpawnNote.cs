using UnityEngine;
using System.Collections;

public class SpawnNote : MonoBehaviour {

	public GameObject spawner;
	public Sprite scroll;

	/*
	 * This function takes in item information then clones the spawner
	 * GameObject. The appropriate values are set to the clone.
	 */
	public void spawnNote(string note, Vector2 location){
		
		//create clone of spawner and add the item information
		GameObject noteobj = Instantiate (spawner);
		Destroy (noteobj.GetComponent("ItemData"));
		Destroy (noteobj.GetComponent("ItemDatabase"));
		noteobj.transform.position = location;
		Vector2 scale = new Vector2 (0.1f, 0.1f);
		noteobj.transform.localScale = scale;
		SpriteRenderer renderer = noteobj.AddComponent<SpriteRenderer> ();
		Debug.Log ("spawning sprite: Note Scroll");
		renderer.sprite = scroll;
		noteobj.name = "Note Scroll";

		noteobj.AddComponent<BoxCollider2D> ();
		noteobj.GetComponent<BoxCollider2D> ().isTrigger = true;
		noteobj.AddComponent<pickupNote> ();
		noteobj.GetComponent<pickupNote> ().setNote (note);
        noteobj.GetComponent<SpriteRenderer>().sortingOrder = 4;
        
	}
}
