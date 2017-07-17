
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public enum FloorType { Grass =0, Mud=1, Water=2}
public enum Enterability { Ok, Never, Wait}

public class Tile : IXmlSerializable
{
	

    public float MovementCost
    {
        get
        {
            float movement = 0f;
            
            if (Floor == FloorType.Grass)
            {
                movement += 1;
            }
            else if (Floor == FloorType.Mud)
            {
                movement+= 1.2f;
            }

            if (InstalledFurniture != null)
            {
                movement *= InstalledFurniture.MovementCost;

            }

            return movement;
        }
      
    }

	public Tile North(){
		return GameManager.Instance.TileDataGrid.GetTileAt (X, Y + 1);
	}

	public Tile East(){
		return GameManager.Instance.TileDataGrid.GetTileAt (X+1, Y );
	}

	public Tile South(){
		return GameManager.Instance.TileDataGrid.GetTileAt (X, Y - 1);
	}

	public Tile West(){
		return GameManager.Instance.TileDataGrid.GetTileAt (X-1, Y);
	}


	 Action<Tile> cbTileFloorChanged;
	FloorType _type =  FloorType.Grass;
	public Furniture InstalledFurniture { get; protected set; }
	public Room Room;

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


	public Inventory inventory { get;  set; }



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

	public Tile[] GetNeighbours(bool diagOK = false)
    {
        Tile[] ns;

        if (!diagOK)
        {
            ns = new Tile[4];  //N , E , S , W
        }
        else
        {
            ns = new Tile[8];// N E S W NE SE SW NW
        }

    
        ns[0] = GameManager.Instance.TileDataGrid.GetTileAt(X, Y +1 ); //north
        ns[1] = GameManager.Instance.TileDataGrid.GetTileAt(X + 1, Y);//East
        ns[2] = GameManager.Instance.TileDataGrid.GetTileAt(X, Y - 1);//south
        ns[3] = GameManager.Instance.TileDataGrid.GetTileAt(X - 1, Y);//west

        if (diagOK)
        {
			ns[4] = GameManager.Instance.TileDataGrid.GetTileAt(X + 1, Y + 1); //NE
            ns[5] = GameManager.Instance.TileDataGrid.GetTileAt(X + 1, Y - 1); //SE
            ns[6] = GameManager.Instance.TileDataGrid.GetTileAt(X - 1, Y - 1); //SW
            ns[7] = GameManager.Instance.TileDataGrid.GetTileAt(X - 1, Y + 1); //NW
        }

            return ns;
    }

	public bool PlaceFurniture(Furniture itemInstance){
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

	public bool PlaceInventory(Inventory inv){
		if (inv == null) {
			inventory = null;
			return true;
		}

		if (inventory != null) {
			//TODO: this will be ok, if the object types match and inventory is not full.
			if (inventory.objectType != inv.objectType) {
				Debug.LogError ("Cant add inventory of different type.");
				return false;
			}

			int numToMove = inv.StackSize;
			if (inventory.StackSize + numToMove > inventory.maxStackSize) {
				numToMove = inventory.maxStackSize - inventory.StackSize;
			}

			inventory.StackSize += numToMove;
			inv.StackSize -= numToMove;
		
			return true;
		}

		inventory = inv.Clone ();
		inventory.Tile = this;

		inv.StackSize = 0;

		return true;
	}

	/// <summary>
	/// SAVING & LOADING XML STUFF
	/// </summary>



	public XmlSchema GetSchema(){
		return null;
	}

	public void WriteXml (XmlWriter writer){
		writer.WriteAttributeString ("X", X.ToString ());
		writer.WriteAttributeString ("Y", Y.ToString ());
		writer.WriteAttributeString("Type", ((int)Floor).ToString());
	}

	public void ReadXml (XmlReader reader){
		//X = int.Parse (reader.GetAttribute ("X"));
		//Y  = int.Parse (reader.GetAttribute ("Y"));

		Floor = (FloorType)int.Parse (reader.GetAttribute ("Type"));
	}




	public Enterability IsEnterable(){
		//returns true if this tyle can be entered now.
		if (MovementCost == 0) {
			return Enterability.Never;
		}

		//check furn return soon.
		if(InstalledFurniture!= null && InstalledFurniture.isEnterable!=null){
			return InstalledFurniture.isEnterable (InstalledFurniture);
		}

		return Enterability.Ok;
	}


		
}
