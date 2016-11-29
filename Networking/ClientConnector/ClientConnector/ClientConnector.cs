using System;
using System.Collections.Generic;
using System.Reflection;

using DarkRift.ConfigTools;
using DarkRift.Storage;
using DarkRift;

using DataPacketLib;


namespace ClientConnector{

	public class ClientConnector : Plugin {
		
		/*
		 * The following declarations are required for plugin
		 * identification by the DarkRift server.
		 */
		public override string name {
			get {
				return "ClientConnector plugin";
			}
		}

		public override string version {
			get {
				return "0.1";
			}
		}

		public override Command[] commands {
			get {
				return new Command[0];
			}
		}

		public override string author {
			get {
				return "Liluye";
			}
		}


		public override string supportEmail {
			get {
				return "april.lautenbach@gmail.com";
			}
		}


		public ClientConnector() {
			//Set the onData event to call the OnDataReived function
			//ConnectionService handles all connections to the server.
			ConnectionService.onData += OnDataReceived;
		}

		/*
		 * This function handles all data events on the server. In this game the
		 * server does more than just pass information between clients so it needs
		 * to be handled by a plugin.
		 */
		public void OnDataReceived(ConnectionService con, ref NetworkMessage msg){
			DataPacket packet;
			string query;

			//Logging to make sure the packets are arriving with the right data.
			Interface.Log("Received data from " + msg.senderID.ToString ());
			//data arrives encoded and must be run through this function before it can be
			//accessed.
			msg.DecodeData ();
			Interface.Log ("Tag: " + msg.tag);
			Interface.Log ("Subject: " + msg.subject);
			Interface.Log ("Data: " + msg.data);

			//if the msg is "RequestData" then query server and send data
			if(msg.tag == 0){
				//Item query to get at most 5 random records from the item table
				query = "SELECT * FROM item ORDER BY RAND() LIMIT 3;";
				DatabaseRow [] itemResults = DarkRiftServer.database.ExecuteQuery (query);
				//Note query to get at most 5 random records from the note table.
				query = "SELECT * FROM note ORDER BY RAND() LIMIT 3;";
				DatabaseRow [] noteResults = DarkRiftServer.database.ExecuteQuery (query);

				//cycle through array and send packet for each item
				for(int i = 0; i < itemResults.Length; i++){
					if(itemResults[i] != null){
						packet = new DataPacket(Convert.ToByte(itemResults[i]["type"]), (float)itemResults[i]["locationX"],
							(float)itemResults[i]["locationY"]);
						Interface.Log (packet.itemNum + ", " + packet.x + ", " + packet.y);
						if (con.SendReply (1, msg.subject, packet)) {
							Interface.Log ("Packet SENT!");
						} else {
							Interface.LogError("ERROR: Packet did not send.");
						}
					}
				}

				//cycle through array and send packet for each note
				for(int i = 0; i < noteResults.Length; i++){
					if(noteResults[i] != null){
						packet = new DataPacket((string)noteResults[i]["message"], (float)noteResults[i]["locationX"],
							(float)noteResults[i]["locationY"]);
						Interface.Log (packet.note + ", " + packet.x + ", " + packet.y);
						if (con.SendReply (2, msg.subject, packet)) {
							Interface.Log ("Packet SENT!");
						} else {
							Interface.LogError("ERROR: Packet did not send.");
						}
					}
				}

				//Once all the data is sent to the client indicate that it is finished
				con.SendReply (0, msg.subject, "Done");
			

			//if the msg is a dataPacket from client put it into the database
			//The only time the client will send DataPackets is when the level is finished so the data
			//must be for the database.
			} else if (msg.tag == 1) {
				packet = (DataPacket)msg.data;
				query = "INSERT INTO item (type, locationX, locationY)" +
				"VALUES (" + (int)packet.itemNum + ", " + packet.x + ", " + packet.y + ");";
				Interface.Log ("adding item to database");
				DarkRiftServer.database.ExecuteQuery (query);

			} else if (msg.tag == 2) {
				//packet = (DataPacket)msg.data;
				string notepacket = (string)msg.data;
				string[] packetdata = notepacket.Split ('|');
				query = "INSERT INTO note (message, locationX, locationY)" +
					"VALUES (\"" + DarkRiftServer.database.EscapeString (packetdata[0]) + "\", " +
					float.Parse(packetdata[1]) + ", " + float.Parse(packetdata[2]) + ");";
				Interface.Log ("adding note to database");
				DarkRiftServer.database.ExecuteQuery (query);
			} else {
				Interface.LogError ("ERROR: there is a problem with the packet could not be added to DB.");
			}
		}
	}
}

