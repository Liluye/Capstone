using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DarkRift;
using DataPacketLib;

public class NetworkManager : MonoBehaviour
{
	//Using local network for now.
	public string IP = "127.0.0.1";
	public int clientID;

	private List<DataPacket> itemData = new List<DataPacket> ();
	private List<DataPacket> noteData = new List<DataPacket> ();

	void Start(){
		DarkRiftAPI.Connect(IP);

		DarkRiftAPI.onData += onDataReceived;

		if (DarkRiftAPI.isConnected){
			Debug.Log ("Sending data request.");
			DarkRiftAPI.SendMessageToServer(0, (ushort)clientID, "RequestData");
		} else {
			Debug.Log ("Failed to connect to DarkRift Server!");
		}
	}
		
	void onDataReceived (byte tag, ushort subject, object data){
		Debug.Log ("Data Received!!!");
		Debug.Log ("data: " + data.ToString());

		//if the data recieved is a DataPacket, add to list
		if (data.GetType ().Equals (typeof(DataPacket))) {
			DataPacket packet = (DataPacket) data;
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
			foreach (DataPacket packet in itemData) {
				GameObject.Find ("spawner").GetComponent<SpawnItem> ()
					.spawning (packet.itemNum, new Vector2 (packet.x, packet.y));
			}
		}
	}


	//For now disconnect on app close, but want it to disconnect after data transfer.
	void OnApplicationQuit(){
//		GatherItem gi = new GatherItem ();
//		DataPacket[] packets = gi.itemGatherer ();
//		Debug.Log ("packet: " + packets.Length);

		List<GameObject> goList = new List<GameObject> ();
		List<DataPacket> packetList = new List<DataPacket> ();

		GameObject[] temp = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

		foreach (GameObject go in temp){
			if(go.GetComponent<ItemData>() != null && go.GetComponent<SpriteRenderer>() != null){
				goList.Add(go);
				Debug.Log ("putting in LIST: " + go.name);
			}
		}
		foreach(GameObject go in goList){
			ItemData idata = go.GetComponent<ItemData>();
			if (idata == null) {
				Debug.Log ("ERROR!!! idata is null");
			}
			Vector2 loc = go.transform.position;
			Debug.Log ("item going to database: " + idata.item.Title);

			DataPacket dp = new DataPacket ((byte)idata.item.ID, loc.x, loc.y);
			DarkRiftAPI.SendMessageToServer (1, (ushort)clientID, dp);
		}


		DarkRiftAPI.Disconnect();
	}




}

