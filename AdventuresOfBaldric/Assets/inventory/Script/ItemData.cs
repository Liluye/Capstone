/*****************************************************************
Script to handle dragging and dropping items

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    /** the item being dragged */
	public Item item;

    /** integer to deal with stacking */
	public int amount;

    /** the slot being dragged to */
	public int slot;

    /** the player's current inventory */
    private Inventory inv;

    /** the offset of the item */
	private Vector2 offset;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
	{
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
	}

    /*******************************************************************
	 * Method to determine what happens when an item begins to be
     * dragged
     * @param eventData PointerEventData collected from the mouse 
     * pointer
	 ******************************************************************/
    public void OnBeginDrag (PointerEventData eventData)
	{
		if (item != null) 
		{
			offset = eventData.position - new Vector2 (this.transform.position.x, this.transform.position.y);

			this.transform.SetParent (this.transform.parent.parent);
			this.transform.position = eventData.position - offset;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

    /*******************************************************************
	 * Method to determine what happens when an item is dragged
     * @param eventData PointerEventData collected from the mouse 
     * pointer
	 ******************************************************************/
    public void OnDrag (PointerEventData eventData)
	{
		if (item != null) 
		{
			this.transform.position = eventData.position - offset;
		}
	}

    /*******************************************************************
	 * Method to determine what happens when an item stops being
     * dragged
     * @param eventData PointerEventData collected from the mouse 
     * pointer
	 ******************************************************************/
    public void OnEndDrag (PointerEventData eventData)
	{
		this.transform.SetParent (inv.slots[slot].transform);
		this.transform.position = inv.slots [slot].transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

}
