﻿using System;
using UnityEngine;

namespace DataPacketLib{
    /*
     * The DataPacket is an object to be sent between the DarkRift
     * server and the client. It is built as a .dll file and used by
     * both the server and Unity.
     */
	[Serializable]
	public class DataPacket{
        //items and notes are idenfied by message tags elsewhere

        //if it is not an item then note is valid
        public string note { get; set; }
        //if it is an item then the itemNum is valid
        public byte itemNum { get; set; }
        //location of item/note in level for the x axis
        public float x { get; set; }
        //location of item/note in level for the y axis
        public float y { get; set; }
       

		//Constructor for Item DataPacket
		public DataPacket (byte itemNum, float x, float y){
			this.itemNum = itemNum;
			this.x = x;
			this.y = y;	
		}

		//Constructor for Note DataPacket
		public DataPacket (string note, float x, float y){
			this.note = note;
			this.x = x;
			this.y = y;
		}
	}
}
