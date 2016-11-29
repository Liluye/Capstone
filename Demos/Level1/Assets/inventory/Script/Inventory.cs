using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * This script builds the inventory 
 */

public class Inventory : MonoBehaviour {

	GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	int slotAmount;
	public List<Item> items = new List<Item> ();
	public List<GameObject> slots = new List<GameObject> ();

	void Start()
	{
		database = GetComponent<ItemDatabase> ();

		slotAmount = 3;
		inventoryPanel = GameObject.Find ("InventoryPanel");
		slotPanel = inventoryPanel.transform.FindChild ("SlotPanel").gameObject;

		// Adds the number of slots dynamically by changing the slotAmount value
		for (int i = 0; i < slotAmount; i++) 
		{
			items.Add (new Item());
			slots.Add (Instantiate (inventorySlot));
			slots [i].GetComponent<Slot> ().id = i;
			slots [i].transform.SetParent (slotPanel.transform);
		}
			
		AddItem (3);
		AddItem (1);


		//Debug.Log (items [1].Title);
	}

	//Method to add item to the inventory
	public void AddItem(int id)
	{
		Item itemToAdd = database.FetchItemById (id);

		//Deals with stackable items
		if (itemToAdd.Stackable && CheckIfItemInInventory (itemToAdd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
					data.amount++;
					data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
					break;
				}
			}
		} 

		//Deals with non stackable items
		else {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == -1) 
				{
					items[i] = itemToAdd;
					GameObject itemObj = Instantiate (inventoryItem);
					itemObj.GetComponent<ItemData> ().item = itemToAdd;
					itemObj.GetComponent<ItemData> ().slot = i;
					itemObj.transform.SetParent (slots [i].transform);
					itemObj.transform.position = Vector2.zero;
					itemObj.GetComponent<Image> ().sprite = itemToAdd.Sprite;
					itemObj.name = itemToAdd.Title;

					break;
				}
			}
		}
	}

	bool CheckIfItemInInventory(Item item)
	{
		for (int i = 0; i < items.Count; i++) 
		{
			if (items [i].ID == item.ID) 
			{
				return true;
			}
		}
		return false;
	}
}
