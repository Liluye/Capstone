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
	public int itemNum;
	public Vector2 location;
	public string note;

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
				itemNum = packet.itemNum;
				location = new Vector2 (packet.x, packet.y);
				GameObject.Find ("spawner").GetComponent<SpawnItem> ().spawning ();
			}
		}
	}


	//For now disconnect on app close, but want it to disconnect after data transfer.
	void OnApplicationQuit()
	{
		DarkRiftAPI.Disconnect();
	}




}

