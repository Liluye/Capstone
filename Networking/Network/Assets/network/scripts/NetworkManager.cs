using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DarkRift;
using DataPacketLib;

/*
* NetworkManager handles network connections to the server
* and packet management.
*/
public class NetworkManager : MonoBehaviour
{
	//Reset router to use static ip and port forwarding for server
	public int clientID;
	string IP = "68.37.92.209";
	int port = 4296;

	//lists of DataPackets from DB
	private List<DataPacket> itemData = new List<DataPacket> ();
	private List<DataPacket> noteData = new List<DataPacket> ();
	//list of DataPackets to be sent to DB
	private List<DataPacket> notes = new List<DataPacket> ();

	void Start(){
		//connect to the DarkRift server at IP and port
		DarkRiftAPI.Connect(IP, port);

		//assign onDataRecieved function to the onData event so it
		//runs every time data is recieved from the server.
		DarkRiftAPI.onData += onDataReceived;

		//Send the request for item and note data to spawn on the level
		//Server can only reply to a specific client so the client has to 
		//send the first message.
		if (DarkRiftAPI.isConnected){
			Debug.Log ("Sending data request.");
			DarkRiftAPI.SendMessageToServer(0, (ushort)clientID, "RequestData");
		} else {
			Debug.Log ("Failed to connect to DarkRift Server!");
		}
	}

	/*
	 * Function is attached to the onData server event and will trigger every
	 * time data is recieved.
	 */		
	void onDataReceived (byte tag, ushort subject, object data){
		Debug.Log ("Data Received!!!");
		Debug.Log ("data: " + data.ToString());

		//if the data recieved is a DataPacket, add to list
		if (data.GetType ().Equals (typeof(DataPacket))) {
			DataPacket packet = (DataPacket) data;

			//sort the DataPackets by item and note, the spawning methods will be different
			if (packet.isItem == 1) {
				Debug.Log ("Message " + tag + ": " + packet.itemNum);
				itemData.Add (packet);
			} else {
				Debug.Log ("Message " + tag + ": " + packet.note);
				noteData.Add (packet);
			}
		}

		//if the data recieved is the msg "Done", then spawn the items/notes
		if (data.ToString ().Equals ("Done")) {
			//disconnect from the server
			DarkRiftAPI.Disconnect ();
			//call SpawnItem script for each item
			foreach (DataPacket packet in itemData) {
				GameObject.Find ("spawner").GetComponent<SpawnItem> ()
					.spawning (packet.itemNum, new Vector2 (packet.x, packet.y));
			}

			//TODO need eqivalent script to spawn notes
		}
	}


	void OnApplicationQuit(){

		//connect to the server
		DarkRiftAPI.Connect (IP, port);

		List<GameObject> goList = new List<GameObject> ();
		bool skip = false;

		//grab all the GameObjects in the level
		GameObject[] temp = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

		//find the GameObjects that represent items and put them in the goList
		foreach (GameObject go in temp){

			//if the gameObject has both ItemData and a SpriteRenderer then it's an item on the floor 
			//inventory doesn't use the SpriteRenderer
			if(go.GetComponent<ItemData>() != null && go.GetComponent<SpriteRenderer>() != null){
				//compare to the items in the itemData list that were sent from the server
				foreach(DataPacket item in itemData){
					//if the position and id are the same as item sent from database then duplicate, don't send back
					if (item.x == go.transform.position.x && item.y == go.transform.position.y
						&& item.itemNum == go.GetComponent<ItemData>().item.ID) {
						//set skip to true and break out of item comparison
						skip = true;
						break;
					}
				}
				//if skip is true don't put in list for database, and check next gameobject
				if (skip) {
					skip = false;
					continue;
				}
				goList.Add(go);
				//Debug.Log ("putting in LIST: " + go.name);
			}
		}

		//for every item in the goList, get the item data and location
		foreach(GameObject go in goList){
			ItemData idata = go.GetComponent<ItemData>();
			if (idata == null) {
				Debug.Log ("ERROR!!! idata is null");
			}
			Debug.Log ("item going to database: " + idata.item.Title);
			//create a new DataPacket with the item data
			DataPacket dp = new DataPacket ((byte)idata.item.ID, go.transform.position.x, go.transform.position.y);
			//send the item data to the server
			DarkRiftAPI.SendMessageToServer (1, (ushort)clientID, dp);
		}

		//notes list filled as notes are written by function addNote, just send them to DB.
		foreach (DataPacket note in notes) {
			Debug.Log ("note going to database: " + note.note);
			DarkRiftAPI.SendMessageToServer (2, (ushort)clientID, note);
		}

		//disconnect from server
		DarkRiftAPI.Disconnect();
	}

	//Create new DataPacket for note and add to notes list
	public void addNote (string note, Vector2 location){
		DataPacket dp = new DataPacket (note, location.x, location.y);
		notes.Add (dp);
	}



}

