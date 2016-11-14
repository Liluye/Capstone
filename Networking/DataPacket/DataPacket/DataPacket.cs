using System;
using UnityEngine;

namespace DataPacketLib{

	[Serializable]
	public class DataPacket{

        public byte isItem { get; set; }
        public byte itemNum { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public string note { get; set; }

		//Constructor for Item DataPacket
		public DataPacket (byte itemNum, float x, float y){
			this.isItem = 1;
			this.itemNum = itemNum;
			this.x = x;
			this.y = y;	
            this.note = "";
		}

		//Constructor for Note DataPacket
		public DataPacket (string note, float x, float y){
			this.isItem = 0;
			this.note = note;
			this.x = x;
			this.y = y;
		}
	}
}
