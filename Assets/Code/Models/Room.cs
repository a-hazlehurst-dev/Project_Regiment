using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Room : IXmlSerializable
{
	public int Id 
	{
		get {return GameManager.Instance.RoomService.GetRoomID (this);}
	}

	Dictionary<string, float> Environemnt;

	public string Name { get; set; }
	List<Tile> _tiles;
    public int NumberOfTiles()
    {
        return _tiles.Count;
    }

	public Room (string name){
		
		this.Name = name;
		_tiles = new List<Tile> ();
		Environemnt = new Dictionary<string, float> ();
	}

	public bool IsOutside(){
		return this == GameManager.Instance.GetOutsideRoom ();
	}

	public void AssignTile(Tile tile){
		
		if(_tiles.Contains(tile)){
            //already in this room
			return;
		}
        if(tile.Room != null)
        {
            //belongs to some other room
            tile.Room._tiles.Remove(tile);
        }

		tile.Room = this;

		_tiles.Add (tile);
	}

	public void ChangeEnvironment(string name, float amount){
		if (Environemnt.ContainsKey (name)) {
			Environemnt [name] += amount;
		} else {
			Environemnt [name] = amount;
		}

	}

	public float GetEnviromenntAmount(string name){
		if (Environemnt.ContainsKey (name)) {
			return Environemnt [name];
		}
		return 0;
	}

	public string[] GetEnvironmentNames(){
		return Environemnt.Keys.ToArray ();
	}

	public void ResetRoomTilesToOutside(){
		for (int i = 0; i < _tiles.Count; i++) {
			_tiles [i].Room = GameManager.Instance.GetOutsideRoom ();
		}
		_tiles = new List<Tile> ();
	}




	public static void DoRoomFloodFill(Tile sourceTile, bool onlyIfOutside =false){

        var oldRoom = sourceTile.Room; 

		//check if the sourcetile had a room. (will always be the case)
		if (oldRoom != null) {
			//the source tile had a room, so this room has now potentially changed.
			//

			foreach (var t in sourceTile.GetNeighbours()) {
		
					if (t.Room != null && (onlyIfOutside == false || t.Room.IsOutside ())) {
						FloodFillRoom (t, oldRoom);
					}
		
			}
		

			sourceTile.Room = null;

			oldRoom._tiles.Remove (sourceTile);// if the old room exists, remove the source tile from it.

			if (oldRoom.IsOutside () == false) {// if not removing from outside room

				if (oldRoom._tiles.Count > 0) {
					Debug.LogError ("Attempting to delete room with tiles assigned to it.");
				}
				//delete room, as it has no tiles.
				GameManager.Instance.DeleteRoom (oldRoom);
			}
		} else {

			//old room is null, source tile was likely a wall. (may not be the case) the wall was deconstructed.
			//
			FloodFillRoom (sourceTile, null);
          
        }

        Debug.Log(GameManager.Instance.RoomService.FindRooms().Count());

    }

	public static void DoFloodFillOnRemove(Tile sourceTile){
		DoRoomFloodFill (sourceTile);
	}


	protected static void FloodFillRoom(Tile tile, Room oldRoom){
       
        if (tile == null) {
			//tile does not exist, this is probable a neighbour thats out of bounds
			return;
		}

		if (tile.Room != oldRoom) {
			// this tile was already processed by a flood fill, cant flood
			return;
		}
		if (tile.Furniture != null && tile.Furniture.RoomEnclosure) {
			//has wall or door. cant flood
			return;
		}

        Room newRoom = new Room ( "Room");

		Queue<Tile> TilesToCheck = new Queue<Tile> ();

		bool isConnectedToOutside = false;

		TilesToCheck.Enqueue (tile);
		int tileschecked = 0;

		while (TilesToCheck.Count > 0) {

			tileschecked++;
			Tile t = TilesToCheck.Dequeue ();
			//I want t (t) to be part of the new room
			if (t.Room != newRoom) {
				newRoom.AssignTile (t);
				Tile[] tn = t.GetNeighbours ();

				foreach (var t2 in tn) {
					if (t2 == null ) {

						isConnectedToOutside = true;
						//newRoom.ResetRoomTilesToOutside ();
						//return;
					}
					//if the neighbour, is not off the grid, is in the same room as the original tile, && the the tile is not a structure. queue it.
					else {

						if( t2.Room != newRoom && (t2.Furniture == null || t2.Furniture.RoomEnclosure == false))
						{
							TilesToCheck.Enqueue (t2);
						}
					}
				}
			}

		}

		Debug.Log (tileschecked);
		if (isConnectedToOutside) {

			newRoom.ResetRoomTilesToOutside ();
            
			return;

		}

		if (oldRoom != null) {
			//splitting the room into 2 or more, so copy old gas ratios.
			newRoom.CopyEnvironment (oldRoom);
		} else {
			//mergin 1 or more rooms together, so we need to figure out the total
			//calc the temp difference between the merged rooms.
		}

		GameManager.Instance.RoomService.AddRoom(newRoom);
	}

	void CopyEnvironment(Room other){
		foreach (string n in other.Environemnt.Keys) {
			this.Environemnt [n] = other.Environemnt [n];
		}
	}

	public XmlSchema GetSchema(){
		return null;
	}

	public void WriteXml (XmlWriter writer){

		//write environment
		foreach (string k in Environemnt.Keys) {
			writer.WriteStartElement ("Param");
			writer.WriteAttributeString ("name", k);
			writer.WriteAttributeString ("value", Environemnt[k].ToString());
			writer.WriteEndElement ();

		}

	}


	public void ReadXml (XmlReader reader){
		if (reader.ReadToDescendant ("Param")) {
			do {
				string k = reader.GetAttribute ("name");
				float v = float.Parse (reader.GetAttribute ("value"));
				Environemnt [k] = v;

			} while(reader.ReadToNextSibling ("Param"));
		}
	}
}


