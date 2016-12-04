/*****************************************************************
Script to keep track of the slots and handling dragging
and dropping items.

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Slot : MonoBehaviour, IDropHandler {

    /** the player's current inventory */
	private Inventory inv;

    /** active slot id */
	public int id;

    /** the image of the item */
    private Image image;

    /** game objects associated with the player */
    private GameObject player;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
	{
        // determine coloring of the slots
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

    /*******************************************************************
	 * Method to handle dropping items
     * @param eventData PointerEventData collected from the mouse 
     * pointer
	 ******************************************************************/
    public void OnDrop (PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();

        // handles the item if it is dropped in an empty slot
        if (inv.items [id].ID == -1) 
		{
			inv.items [droppedItem.slot] = new Item ();
			inv.items [id] = droppedItem.item;
			droppedItem.slot = id;
        }

		// handles the item if it is dropped in a slot that has an item already in it
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

    /*******************************************************************
	 * Method used to change the current item
     * @param itemName String containing the item's name
	 ******************************************************************/
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
                    // extra param is required when passing the value 0 
                    // through SendMessage function
                    player.SendMessage("SetItem", 0, SendMessageOptions.RequireReceiver);
                    break;
                }
        }
    }
}
