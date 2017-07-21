﻿using System;
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

	public static void DoRoomFloodFill(Furniture sourceFurniture){
		// the room that the furniture was originally assigned too.
        var oldRoom = sourceFurniture.Tile.Room;

        //try building new room starting from north.
        foreach(var t in sourceFurniture.Tile.GetNeighbours())
        {
			FloodFillRoom (t, oldRoom);
        }

        sourceFurniture.Tile.Room = null;
        oldRoom._tiles.Remove(sourceFurniture.Tile);

		if (oldRoom.IsOutside()==false) {

            if (oldRoom._tiles.Count > 0)
            {
                Debug.LogError("Attempting to delete room with tiles assigned to it.");
            }
			//odl room should not have any more tiles in it.
			GameManager.Instance.DeleteRoom (oldRoom);
		}
	}

	protected static void FloodFillRoom(Tile tile, Room oldRoom){
       
        if (tile == null) {
			//empty space, cant flood
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

        Room newRoom = new Room (1, "Room");

		Queue<Tile> TilesToCheck = new Queue<Tile> ();

		TilesToCheck.Enqueue (tile);

		while (TilesToCheck.Count > 0) {
			
			Tile t = TilesToCheck.Dequeue ();
			if (t.Room == oldRoom) {
				newRoom.AssignTile (t);
				Tile[] tn = t.GetNeighbours ();

				foreach (var t2 in tn) {
					if (t2 == null) {
						newRoom.ResetRoomTilesToOutside ();
						return;
					}
					//if the neighbour, is not off the grid, is in the same room as the original tile, && the the tile is not a structure. queue it.
					if (t2 != null && t2.Room == oldRoom && (t2.Furniture == null || t2.Furniture.RoomEnclosure == false)) {
						TilesToCheck.Enqueue (t2);
					}
				}
			}

		}

		newRoom.CopyEnvironment (oldRoom);
       
        GameManager.Instance.AddRoom(newRoom);
	}

	void CopyEnvironment(Room other){
		foreach (string n in other.Environemnt.Keys) {
			this.Environemnt [n] = other.Environemnt [n];
		}
	}
}


