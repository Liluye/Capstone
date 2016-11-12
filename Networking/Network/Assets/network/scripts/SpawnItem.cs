using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

	public GameObject itemObject;

	public void spawning(){
		GameObject go = GameObject.Find ("Main Camera");
		NetworkManager nm = go.GetComponent<NetworkManager> ();

		ItemDatabase idb = itemObject.AddComponent<ItemDatabase> ();
		Item itemToSpawn = idb.FetchItemById (nm.itemNum);
		GameObject itemobj = Instantiate (itemObject);
		itemobj.GetComponent<ItemData> ().item = itemToSpawn;
		itemobj.transform.position = nm.location;
		SpriteRenderer renderer = itemobj.AddComponent<SpriteRenderer> ();
		Debug.Log ("Sprite: " + itemToSpawn.Title);
		renderer.sprite = itemToSpawn.Sprite;
		itemobj.name = itemToSpawn.Title;
	}
}
