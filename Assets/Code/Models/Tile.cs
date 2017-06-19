
using System;

public class Tile 
{
	public enum FloorType { Grass =0, Mud=1, Water=2}

	Action<Tile> cbTileFloorChanged;
	FloorType _type =  FloorType.Grass;
	protected FurnitureItem _installedFurniture;

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


	public FurnitureItem FurnitureItem;
	public StockpileItem StockpileItem;

	public bool PlaceObject(FurnitureItem itemInstance){
		if (itemInstance == null) {
			_installedFurniture = null;
			return true;
		}

		if (_installedFurniture != null) {
			return false;
		}

		_installedFurniture = itemInstance;
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
		
}
