using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

	public GameObject itemObject;

	public void spawning(int itemNum, Vector2 location){
		ItemDatabase idb = itemObject.GetComponent<ItemDatabase> ();

		Item itemToSpawn = idb.FetchItemById (itemNum);
		GameObject itemobj = Instantiate (itemObject);
		itemobj.GetComponent<ItemData> ().item = itemToSpawn;
		itemobj.transform.position = location;
		SpriteRenderer renderer = itemobj.AddComponent<SpriteRenderer> ();
		Debug.Log ("spawning sprite: " + itemToSpawn.Title);
		renderer.sprite = itemToSpawn.Sprite;
		itemobj.name = itemToSpawn.Title;
	}
}
