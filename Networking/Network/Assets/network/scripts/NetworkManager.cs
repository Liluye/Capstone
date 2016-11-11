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

	List<DataPacket> dataReceived = new List<DataPacket>();

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

		if (data.GetType ().Equals (typeof(DataPacket))) {
			DataPacket datapacket = (DataPacket) data;
			if (datapacket.isItem == 0) {
				Debug.Log ("Message " + tag + ": " + datapacket.itemNum);
			} else {
				Debug.Log ("Message " + tag + ": " + datapacket.note);
			}

			dataReceived.Add (datapacket);
		}
	}

	void spawnData (){
		
	}

	//For now disconnect on app close, but want it to disconnect after data transfer.
	void OnApplicationQuit()
	{
		DarkRiftAPI.Disconnect();
	}



	void SerialiseData(string msg)
	{
		//Here is where we actually serialise things manually. To do this we need to add
		//any data we want to send to a DarkRiftWriter. and then send this as we would normally.
		//The advantage of custom serialisation is that you have a much smaller overhead than when
		//the default BinaryFormatter is used, typically about 50 bytes.
		using(DarkRiftWriter writer = new DarkRiftWriter())
		{
			//Next we write any data to the writer, as we never change the z pos there's no need to 
			//send it.
			writer.Write(msg);

			DarkRiftAPI.SendMessageToServer(0, (ushort)clientID, writer);
		}
	}

	void DeserialiseData(object data)
	{
		//Here we decode the stream, the data will arrive as a DarkRiftReader so we need to cast to it
		//and then read the data off in EXACTLY the same order we wrote it.
		if( data is DarkRiftReader )
		{
			//Cast in a using statement because we are using streams and therefore it 
			//is important that the memory is deallocated afterwards, you wont be able
			//to use this more than once though.
			using(DarkRiftReader reader = (DarkRiftReader)data)
			{
				
			}
		}
		else
		{
			Debug.LogError("Should have recieved a DarkRiftReciever but didn't! (Got: " + data.GetType() + ")");
			transform.position = transform.position;
		}
	}
}

