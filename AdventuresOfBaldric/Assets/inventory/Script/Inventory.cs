/*****************************************************************
Script to build the inventory.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    /** GameObject for the inventory panel */
	GameObject inventoryPanel;

    /** GameObject for the slots in the inventory */
	GameObject slotPanel;

    /** the database containing the inventory */
	ItemDatabase database;

    /** GameObject for a single slot */
	public GameObject inventorySlot;

    /** GameObject for a single item */
    public GameObject inventoryItem;

    /** the number of slots in the inventory */
	int slotAmount;

    /** list of items in the inventory */
	public List<Item> items = new List<Item> ();

    /** list of slots */
	public List<GameObject> slots = new List<GameObject> ();

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
	{
		database = GetComponent<ItemDatabase> ();

		slotAmount = 3;
		inventoryPanel = GameObject.Find ("InventoryPanel");
		slotPanel = inventoryPanel.transform.FindChild ("SlotPanel").gameObject;

		// adds the number of slots dynamically by changing the slotAmount value
		for (int i = 0; i < slotAmount; i++) 
		{
			items.Add (new Item());
			slots.Add (Instantiate (inventorySlot));
			slots [i].GetComponent<Slot> ().id = i;
			slots [i].transform.SetParent (slotPanel.transform);
		}

        AddItem (2);
        AddItem (3);
		AddItem (1);


		//Debug.Log (items [1].Title);
	}

    /*******************************************************************
	 * Method to add an item to the inventory
     * @param id Integer for the item ID in the database
	 ******************************************************************/
    public void AddItem(int id)
	{
		Item itemToAdd = database.FetchItemById (id);

		// deals with stackable items
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

		// deals with non stackable items
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

    /*******************************************************************
	 * Method to check if an item is in the inventory
     * @param item Item to search for
	 ******************************************************************/
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
