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
	//Using local network for now.
	public string IP = "127.0.0.1";
	public int clientID;

	private List<DataPacket> itemData = new List<DataPacket> ();
	private List<DataPacket> noteData = new List<DataPacket> ();

	void Start(){
		//connect to the DarkRift server at IP
		DarkRiftAPI.Connect(IP);

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
		DarkRiftAPI.Connect (IP);

		List<GameObject> goList = new List<GameObject> ();
		List<DataPacket> packetList = new List<DataPacket> ();

		//grab all the GameObjects in the level
		GameObject[] temp = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

		//find the GameObjects that represent items and put them in the goList
		foreach (GameObject go in temp){
			
			//TODO this way of selecting items may end up with inventory items as well!!!
			if(go.GetComponent<ItemData>() != null && go.GetComponent<SpriteRenderer>() != null){
				goList.Add(go);
				Debug.Log ("putting in LIST: " + go.name);
			}
		}
		//for every item in the goList, get the item data and location
		foreach(GameObject go in goList){
			ItemData idata = go.GetComponent<ItemData>();
			if (idata == null) {
				Debug.Log ("ERROR!!! idata is null");
			}
			Vector2 loc = go.transform.position;
			Debug.Log ("item going to database: " + idata.item.Title);
			//create a new DataPacket with the item data
			DataPacket dp = new DataPacket ((byte)idata.item.ID, loc.x, loc.y);
			//send the item data to the server
			DarkRiftAPI.SendMessageToServer (1, (ushort)clientID, dp);
		}

		//TODO need equivalent code to handle finding and sending notes

		//disconnect from server
		DarkRiftAPI.Disconnect();
	}




}

