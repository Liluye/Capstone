/*****************************************************************
Script to build the item database and store it as a list and 
hold data for the item

@author The Adventures of Baldric
@version Fall 2016
*****************************************************************/

using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

/*******************************************************************
* Builds the item database and stores it as a list
******************************************************************/
public class ItemDatabase : MonoBehaviour {

    /** list to store the item database */
	private List<Item> database = new List<Item>();

    /** data about the item */
	private JsonData itemData;

    /*******************************************************************
	 * Method used for initialization
	 ******************************************************************/
    void Start()
	{
		itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/inventory/StreamingAssets/Items.Json"));
		ConstructItemDatabase ();
		//Debug.Log (FetchItemById(0).Title);

	}

    /*******************************************************************
	 * Get the item from the database
     * @param id Integer for the item id
	 ******************************************************************/
    public Item FetchItemById(int id)
	{
		for (int i = 0; i < database.Count; i++) 
			if (database [i].ID == id)
				return database [i];
		return null;
	}

    /*******************************************************************
	 * Method to create the database
	 ******************************************************************/
    void ConstructItemDatabase()
	{
		for (int i = 0; i < itemData.Count; i++) 
		{
			database.Add (new Item ((int)itemData[i]["id"], itemData[i]["title"].ToString(), itemData[i]["description"].ToString(), 
				(bool)itemData[i]["stackable"], itemData[i]["slug"].ToString()));
		}
	}
}

/*******************************************************************
* Holds the data for an item
******************************************************************/
public class Item
{

    /*******************************************************************
	 * Getters and setters for item ID, title, description,
     * stackable state, slug, and sprite
	 ******************************************************************/
    public int ID { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }

    /*******************************************************************
	 * Constructor for an Item
     * @param id Integer ID for the item
     * @param title String with the item's name
     * @param description String with the item's description
     * @param stackable the item's ability to be stacked
     * @param slug String for the item's slug
	 ******************************************************************/
    public Item(int id, string title, string description, bool stackable, string slug)
	{
		this.ID = id;
		this.Title = title;
		this.Description = description;
		this.Stackable = stackable;
		this.Slug = slug;
		this.Sprite = Resources.Load<Sprite> ("sprites/items/" + slug);
	}

    /*******************************************************************
	 * Constructor for Item
	 ******************************************************************/
    public Item()
	{
		this.ID = -1;
	}
}
