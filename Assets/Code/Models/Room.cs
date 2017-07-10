using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	int id;
	float temperature;	
	float cleanliness;
	public string Name { get; set; }
	List<Tile> _tiles;

	public Room (int id, string name){
		this.id = id;
		this.Name = name;
		_tiles = new List<Tile> ();
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

	public void ResetRoomTilesToOutside(){
		for (int i = 0; i < _tiles.Count; i++) {
			_tiles [i].Room = GameManager.Instance.GetOutsideRoom ();
		}
		_tiles = new List<Tile> ();
	}

	public static void DoRoomFloodFill(Furniture sourceFurniture){
        Debug.Log("Creating Room");
        var oldRoom = sourceFurniture.Tile.Room;

        //try building new room starting from north.
        foreach(var t in sourceFurniture.Tile.GetNeighbours())
        {
            FloodFillRoom(t, oldRoom);
        }

        sourceFurniture.Tile.Room = null;
        oldRoom._tiles.Remove(sourceFurniture.Tile);

		if (oldRoom != GameManager.Instance.GetOutsideRoom ()) {

            if (oldRoom._tiles.Count > 0)
            {
                Debug.LogError("Attempting to delete room with tiles assigned to it.");
            }
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
		if (tile.InstalledFurniture != null && tile.InstalledFurniture.RoomEnclosure) {
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
                Tile[] tn = tile.GetNeighbours();
             
                foreach (var t2 in tn)
                {
                    //if (t2.Room != null && t2.Room == GameManager.Instance.GetOutsideRoom())
                    //{
                    //    Debug.Log("exposed to outside.");
                    //    t2.Room.ResetRoomTilesToOutside();
                    //    return;
                    //}
                    if (t2.Room != null && t2.Room == oldRoom && t2.InstalledFurniture != null )
                    {
                        if (t2.InstalledFurniture.RoomEnclosure == false)
                        {
                            TilesToCheck.Enqueue(t2);
                            Debug.Log("Adding new room");
                        }
                    }

                }
			}
		}
       
        GameManager.Instance.AddRoom(newRoom);


	}
}


