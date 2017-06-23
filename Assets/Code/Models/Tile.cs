
using System;

public class Tile 
{
	public enum FloorType { Grass =0, Mud=1, Water=2}

	Action<Tile> cbTileFloorChanged;
	FloorType _type =  FloorType.Grass;
	public Furniture InstalledFurniture { get; protected set; }

	public int X { get; protected set; }
	public int Y { get; protected set; } 
	public FloorType Floor 
	{ 
		get { return _type; } 
		set 
		{ 
			FloorType old = _type;
			_type = value; 
			if (cbTileFloorChanged != null && old != _type) {
				cbTileFloorChanged (this);
			}
		}
	}

	public Job PendingFurnitureJob;


	public Inventory StockpileItem;

	public bool PlaceObject(Furniture itemInstance){
		if (itemInstance == null) {
            InstalledFurniture = null;
			return true;
		}

		if (InstalledFurniture != null) {
			return false;
		}

        InstalledFurniture = itemInstance;
		return true;
	}

	public Tile(int x, int y, FloorType floorType)
	{
		X = x;
		Y = y;
		Floor = floorType;

	}

	public void RegisterFloorTypeChangedCb(Action<Tile> callBack)
	{
		cbTileFloorChanged += callBack;
	}
	public void UnRegisterFloorTypeChangedCb(Action<Tile> callBack)
	{
		cbTileFloorChanged -= callBack;
	}

	public bool IsNeighbour(Tile tile, bool diagOK = false){
		if (this.X == tile.X && (this.Y == tile.Y + 1 || this.Y == tile.Y - 1))
			return true;

		if (this.Y == tile.Y && (this.X == tile.X + 1 || this.X == tile.Y - X))
			return true;

		if (diagOK) {

			if (this.X == tile.X +1 && (this.Y == tile.Y+1 || this.Y == tile.Y-1) )
				return true;

			if (this.X == tile.X -1 && (this.Y == tile.Y+1 || this.Y == tile.Y-1) )
				return true;
			

		}
		return false;
	}
		
}
