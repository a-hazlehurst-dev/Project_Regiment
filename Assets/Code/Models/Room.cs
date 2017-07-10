using System;
using System.Collections.Generic;

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
			return;
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

		var oldRoom = sourceFurniture.Tile.Room;

		//try building new room starting from north.
		FloodFillRoom (sourceFurniture.Tile.North (), oldRoom);
		FloodFillRoom (sourceFurniture.Tile.East (), oldRoom);
		FloodFillRoom (sourceFurniture.Tile.South (), oldRoom);
		FloodFillRoom (sourceFurniture.Tile.West (), oldRoom);
			
		oldRoom._tiles = new List<Tile> ();

		if (oldRoom != GameManager.Instance.GetOutsideRoom ()) {
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
				TilesToCheck.Enqueue (t.North());
				TilesToCheck.Enqueue (t.South());
				TilesToCheck.Enqueue (t.East());
				TilesToCheck.Enqueue (t.West());


			}
		}


	}
}


