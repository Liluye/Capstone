/*****************************************************************
Script to spawn items within the level when it loads.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

    /** game object that spawns items */
	public GameObject spawner;

    /*******************************************************************
    * This function takes in item information then clones the spawner
	* GameObject. The appropriate values are set to the clone.
    * @param itemNum Integer if for the item
    * @param location location to place the item
    ******************************************************************/
    public void spawnItem(int itemNum, Vector2 location){

		// access the ItemDatabase script on the spawner and use it to get
		// item information from JSON file.
		ItemDatabase idb = spawner.GetComponent<ItemDatabase> ();
		Item itemToSpawn = idb.FetchItemById (itemNum);

		// create clone of spawner and add the item information
		GameObject itemobj = Instantiate (spawner);
		itemobj.GetComponent<ItemData> ().item = itemToSpawn;
		itemobj.transform.position = location;
		SpriteRenderer renderer = itemobj.AddComponent<SpriteRenderer> ();
		Debug.Log ("spawning sprite: " + itemToSpawn.Title);
		renderer.sprite = itemToSpawn.Sprite;
		itemobj.name = itemToSpawn.Title;

	}
}
