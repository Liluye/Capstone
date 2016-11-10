using System;

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

		public ClientConnector() {
			ConnectionService.onData += OnDataReceived;
		}

		public void OnDataReceived(ConnectionService con, ref NetworkMessage msg){
			Interface.Log("Received data from " + msg.senderID.ToString ());
			msg.DecodeData ();
			Interface.Log ("Data: " + msg.data);
			con.SendReply (msg.tag, msg.subject, "Hi Client, you are connected!");

		}
	}
}

