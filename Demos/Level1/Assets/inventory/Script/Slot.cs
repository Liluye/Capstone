using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler {

	private Inventory inv;
	public int id;

	void Start()
	{
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
	}

	public void OnDrop (PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();

		if(inv.items[id].ID == -1)
		{
			inv.items [droppedItem.slot] = new Item ();
			droppedItem.slot = id;
		}
	}



}
