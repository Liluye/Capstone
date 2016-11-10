using System;
	
namespace DataPacket{
	public class DataPacket{
		
		public bool isItem { get; set;}
		public int itemNum { get; set;}
		public float x { get; set;}
		public float y { get; set;}
		public string note { get; set;}
		public int msgTotal { get; set;}
		public int msgNum { get; set;}


		//Constructor for Item DataPacket
		public DataPacket (int itemNum, float x, float y, int msgNum, int msgTotal){
			this.isItem = true;
			this.itemNum = itemNum;
			this.x = x;
			this.y = y;
			this.msgNum = msgNum;
			this.msgTotal = msgTotal;
		}

		//Constructor for Note DataPacket
		public DataPacket(string note, float x, float y, int msgNum, int msgTotal){
			this.isItem = false;
			this.note = note;
			this.x = x;
			this.y = y;
			this.msgNum = msgNum;
			this.msgTotal = msgTotal;
		}
	}
}
