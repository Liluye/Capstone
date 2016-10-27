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
					Interface.Log ("Item num: " + reader.ReadInt16());
					Interface.Log ("X position: " + reader.ReadSingle());
					Interface.Log ("Y position: " + reader.ReadSingle ());
				}
			}
			else
			{
				Interface.Log("Should have recieved a DarkRiftReciever but didn't! (Got: " + data.GetType() + ")");
			}
		}
	}
}

