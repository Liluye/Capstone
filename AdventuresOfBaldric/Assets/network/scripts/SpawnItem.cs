using UnityEngine;
using System.Collections;

/*
 * This is the script to spawn items within the level when it loads
 */

public class SpawnItem : MonoBehaviour {

	public GameObject spawner;

	/*
	 * This function takes in item information then clones the spawner
	 * GameObject. The appropriate values are set to the clone.
	 */
	public void spawnItem(int itemNum, Vector2 location){
		//access the ItemDatabase script on the spawner and use it to get
		//item information from JSON file.
		ItemDatabase idb = spawner.GetComponent<ItemDatabase> ();
		Item itemToSpawn = idb.FetchItemById (itemNum);

		//create clone of spawner and add the item information
		GameObject itemobj = Instantiate (spawner);
		itemobj.GetComponent<ItemData> ().item = itemToSpawn;
		itemobj.transform.position = location;
		SpriteRenderer renderer = itemobj.AddComponent<SpriteRenderer> ();
		Debug.Log ("spawning sprite: " + itemToSpawn.Title);
		renderer.sprite = itemToSpawn.Sprite;
		itemobj.name = itemToSpawn.Title;
	}
}
