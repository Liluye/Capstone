using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/*
 *	This class to to keep track of the slots 
 *	it handles dragging and dropping items
 */

public class Slot : MonoBehaviour, IDropHandler {

	private Inventory inv;
	public int id;
    private Image image;
    private GameObject player;

	void Start()
	{
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
        image = GetComponent<Image>();
        if (id == 0) {
            image.color = Color.green;
        }
        else
        {
            image.color = Color.grey;
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

	public void OnDrop (PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();

        //Handles the item if it is dropped in an empty slot
        if (inv.items [id].ID == -1) 
		{
			inv.items [droppedItem.slot] = new Item ();
			inv.items [id] = droppedItem.item;
			droppedItem.slot = id;
        }

		//Handles the item if it is dropped in a slot that has an item already in it
		else 
		{
			Transform item = this.transform.GetChild (0);
			item.GetComponent<ItemData> ().slot = droppedItem.slot;
			item.transform.SetParent (inv.slots [droppedItem.slot].transform);
			item.transform.position = inv.slots [droppedItem.slot].transform.position;
            
            droppedItem.slot = id;
			droppedItem.transform.SetParent (this.transform);
			droppedItem.transform.position = this.transform.position;

			inv.items [droppedItem.slot] = item.GetComponent<ItemData> ().item;
			inv.items [id] = droppedItem.item;
            changeItem(inv.items[droppedItem.slot].Slug);
        }
        
    }

    private void changeItem(String itemName)
    {
        switch(itemName)
        {
            case "grappling_hook":
                {
                    player.SendMessage("SetItem", 2);
                    break;
                }

            case "bomb":
                {
                    player.SendMessage("SetItem", 1);
                    break;
                }

            case "boomerang":
                {
                    //Extra param is required when passing the value 0 through SendMessage function
                    player.SendMessage("SetItem", 0, SendMessageOptions.RequireReceiver);
                    break;
                }
        }
    }
}
