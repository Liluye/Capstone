using System;
using System.Collections.Generic;
using System.Reflection;

using DarkRift.ConfigTools;
using DarkRift.Storage;
using DarkRift;

using DataPacketLib;


namespace ClientConnector{

	public class ClientConnector : Plugin {

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
			ConnectionService.onData += OnDataReceived;
		}

		public void OnDataReceived(ConnectionService con, ref NetworkMessage msg){
			DataPacket packet;
			string query;

			Interface.Log("Received data from " + msg.senderID.ToString ());
			msg.DecodeData ();
			Interface.Log ("Tag: " + msg.tag);
			Interface.Log ("Subject: " + msg.subject);
			Interface.Log ("Data: " + msg.data);

			//if the msg is "RequestData" then query server and send data
			if(msg.data.Equals ("RequestData")){
				query = "SELECT * FROM item ORDER BY RAND() LIMIT 5;";
				DatabaseRow [] itemResults = DarkRiftServer.database.ExecuteQuery (query);
				query = "SELECT * FROM note ORDER BY RAND() LIMIT 5;";
				DatabaseRow [] noteResults = DarkRiftServer.database.ExecuteQuery (query);

				//cycle through array and send packet for each item
				for(int i = 0; i < itemResults.Length; i++){
					if(itemResults[i] != null){
						Interface.Log ("itemResults: " + itemResults[i]["type"]);
						packet = new DataPacket(Convert.ToByte(itemResults[i]["type"]), (float)itemResults[i]["locationX"],
							(float)itemResults[i]["locationY"]);
						Interface.Log (packet.itemNum + ", " + packet.x + ", " + packet.y);
						if (con.SendReply (msg.tag, msg.subject, packet)) {
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
						con.SendReply (msg.tag, msg.subject, packet);
					}
				}
			}

			//if the msg is a dataPacket from client put it into the database
			if(msg.data.GetType().Equals(typeof(DataPacket))){
				packet = (DataPacket) msg.data;
				//if the packet is an item put in item table
				if(packet.isItem == 1){
					query = "INSERT INTO item (type, x, y)" +
						"VALUES (" + (int)packet.itemNum + ", " + packet.x + ", " + packet.y + ");";
					//if the packet is a note put in note table
				}else if (packet.isItem == 0){
					query = "INSERT INTO note (message, x, y)" +
						"VALUES (" + DarkRiftServer.database.EscapeString(packet.note) + ", " + 
						packet.x + ", " + packet.y + ");";
				}else{
					Interface.LogError ("ERROR: there is a problem with the packet could not be added to DB.");
				}

			}
		}
	}
}

