using System;
using System.Collections.Generic;

using DarkRift.ConfigTools;
using DarkRift.Storage;
using DarkRift;

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

		int datalen = 10;
		//DataPacket [] dataReceived = new DataPacket[datalen];
		//List<DataPacket> dataRecieved = new List<DataPacket>();


		public ClientConnector() {
			ConnectionService.onData += OnDataReceived;
		}

		public void OnDataReceived(ConnectionService con, ref NetworkMessage msg){
			DataPacket packet;
			string query;

			Interface.Log("Received data from " + msg.senderID.ToString ());
			msg.DecodeData ();
			Interface.Log ("Data: " + msg.data);
		
			//if the msg is "RequestData" then query server and send data
			if(msg.data.Equals ("RequestData")){
				query = "SELECT * FROM items ORDER BY RAND() LIMIT 5;";
				DatabaseRow [] itemResults = DarkRiftServer.database.ExecuteQuery (query);
				query = "SELECT * FROM notes ORDER BY RAND() LIMIT 5;";
				DatabaseRow [] noteResults = DarkRiftServer.database.ExecuteQuery (query);

				//cycle through array and send packet for each item
				for(int i = 0; i < itemResults.Length; i++){
					if(itemResults[i] != null){
						packet = new DataPacket(itemResults[i]["id"], );
						con.SendReply (msg.tag, msg.subject, packet);
					}
				}

				//cycle through array and send packet for each note
				for(int i = 0; i < noteResults.Length; i++){
					if(noteResults[i] != null){
						packet = new DataPacket(noteResults[i]["id"], );
						con.SendReply (msg.tag, msg.subject, packet);
					}
				}
			}

			//if the msg is a dataPacket from client put it into the database
			if(msg.data.Equals (DataPacket)){
				packet = (DataPacket) msg.data;
				//dataRecieved.Add(packet);
				//if the packet is an item put in item table
				if(packet.isItem){
					query = "INSERT INTO (id, x, y)" +
						"VALUES (" + packet.itemNum + ", " + packet.x + ", " + packet.y + ");";
				//if the packet is a note put in note table
				}else if (!packet.isItem){
					query = "INSERT INTO (note, x, y)" +
						"VALUES (" + packet.note + ", " + packet.x + ", " + packet.y + ");";
				}else{
					Interface.LogError ("ERROR: there is a problem with the packet could not be added to DB.");
				}

			}
		}
	}
}

