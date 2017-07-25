using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room
{
	int id;

	Dictionary<string, float> Environemnt;

	public string Name { get; set; }
	List<Tile> _tiles;

	public Room (int id, string name){
		this.id = id;
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




	public static void DoRoomFloodFill(Tile sourceTile){

        var oldRoom = sourceTile.Room; 

		//check if the sourcetile had a room. (will always be the case)
		if (oldRoom != null) {
			//the source tile had a room, so this room has now potentially changed.
			//

			foreach (var t in sourceTile.GetNeighbours()) {
				FloodFillRoom (t, oldRoom);
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
		}
	}

	public static void DoFloodFillOnRemove(Tile sourceTile){
		FloodFillRoom (sourceTile, null);
	}


	protected static void FloodFillRoom(Tile tile, Room oldRoom){
       
        if (tile == null) {
			//tile does not exist, this is probable a neighbour thats out of bounds
			return;
		}

		if (tile.Room != oldRoom) {
			Debug.Log ("doo i need this?");
			// this tile was already processed by a flood fill, cant flood
			return;
		}
		if (tile.Furniture != null && tile.Furniture.RoomEnclosure) {
			//has wall or door. cant flood
			return;
		}

        Room newRoom = new Room (1, "Room");

		Queue<Tile> TilesToCheck = new Queue<Tile> ();

		bool isConnectedToOutside = false;

		TilesToCheck.Enqueue (tile);

		while (TilesToCheck.Count > 0) {
			
			Tile t = TilesToCheck.Dequeue ();
			//I want t (t) to be part of the new room
			if (t.Room != newRoom) {

				/// we the checked tile is equal to the old room. (old room changed, so check its still 

				newRoom.AssignTile (t);
				Tile[] tn = t.GetNeighbours ();

				foreach (var t2 in tn) {
					if (t2 == null) {

						isConnectedToOutside = true;
						//newRoom.ResetRoomTilesToOutside ();
						//return;
					}
					//if the neighbour, is not off the grid, is in the same room as the original tile, && the the tile is not a structure. queue it.
					if (t2!=null &&t2.Room != newRoom && (t2.Furniture == null || t2.Furniture.RoomEnclosure == false)) {
						TilesToCheck.Enqueue (t2);
					}
				}
			}

		}
		if (isConnectedToOutside) {
			newRoom.ResetRoomTilesToOutside ();

		}

		if (oldRoom != null) {
			//splitting the room into 2 or more, so copy old gas ratios.
			newRoom.CopyEnvironment (oldRoom);
		} else {
			//mergin 1 or more rooms together, so we need to figure out the total
			//calc the temp difference between the merged rooms.
		}
       
        GameManager.Instance.AddRoom(newRoom);
	}

	void CopyEnvironment(Room other){
		foreach (string n in other.Environemnt.Keys) {
			this.Environemnt [n] = other.Environemnt [n];
		}
	}
}


