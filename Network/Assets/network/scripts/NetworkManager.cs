using UnityEngine;
using System.Collections;

//First get access to the DarkRift namespace
using DarkRift;

public class NetworkManager : MonoBehaviour
{
	//Using local network for now.
	public string IP = "127.0.0.1";

	void Start()
	{
		DarkRiftAPI.Connect(IP);
	}

	//For now disconnect on app close, but want it to disconnect after data transfer.
	void OnApplicationQuit()
	{
		DarkRiftAPI.Disconnect();
	}
}