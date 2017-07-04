using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Furniture  : IXmlSerializable{

	public  Tile Tile { get; protected set; }					//base tile of object( what you place ) object can be bigger than 1 tile.
	public string ObjectType { get; protected set; }
	public float MovementCost { get; protected set; }
	private int _width = 1;
	private int _height = 1;
    public bool LinksToNeighbour { get; protected set; }

	Action<Furniture> cbOnChanged;
	private Func<Tile, bool> funcPositionValidation;

	public Furniture(){
	}

	static public Furniture CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height =1, bool linksToNeighbour = false){
		Furniture item = new Furniture ();

		item.ObjectType = objectType;
		item.MovementCost= movementCost;
		item._width = width;
		item._height = height;
        item.LinksToNeighbour = linksToNeighbour;

		item.funcPositionValidation = item.IsValidPosition;

        return item;
	}


	static public Furniture PlaceFurniture(Furniture prototype,  Tile tile)
	{
		Furniture item = new Furniture ();

		item.ObjectType = prototype.ObjectType;
		item.MovementCost = prototype.MovementCost;
		item._width = prototype._width;
		item._height = prototype._height;

        item.LinksToNeighbour = prototype.LinksToNeighbour;

		item.Tile = tile;
		if (tile.PlaceObject (item)==false)  {
			//if we couldnt place the object.
			//it was likely already occupied
			//do NOT return the object;
			return null;
		}

        if (item.LinksToNeighbour)
        {
            //inform neighbours that they have a new tile        
            int x = tile.X;
            int y = tile.Y;

			var t = GameManager.Instance.TileDataGrid.GetTileAt(x, y + 1);
			if (t != null && t.InstalledFurniture != null &&  t.InstalledFurniture.cbOnChanged!=null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture); //we have northern neighbour with same object as us, so change it with callback;
            }

			t = GameManager.Instance.TileDataGrid.GetTileAt(x +1, y);
			if (t != null && t.InstalledFurniture != null &&  t.InstalledFurniture.cbOnChanged !=null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture);
            }

			t = GameManager.Instance.TileDataGrid.GetTileAt(x, y- 1);
			if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.cbOnChanged !=null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture);
            }
			t = GameManager.Instance.TileDataGrid.GetTileAt(x-1, y );
			if (t != null && t.InstalledFurniture != null  && t.InstalledFurniture.cbOnChanged !=null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture);
            }

        }

		return item;
	}
		
	public void RegisterOnChangedCallback(Action<Furniture> callBackFunc){
		cbOnChanged += callBackFunc;
	}
	public void UnRegisterOnChangedCallback(Action<Furniture> callBackFunc){
		cbOnChanged -= callBackFunc;
	}

	public bool __IsValidPosition(Tile t){
		return funcPositionValidation (t);
	}

	public bool IsValidPosition(Tile t){
		if (t.InstalledFurniture != null) {
			return false;
		}

		return true;
	}

	public XmlSchema GetSchema(){
		return null;
	}

	public void WriteXml (XmlWriter writer){
		
		writer.WriteAttributeString ("X", Tile.X.ToString());
		writer.WriteAttributeString ("Y", Tile.Y.ToString());

		writer.WriteAttributeString ("objectType", ObjectType);
		writer.WriteAttributeString ("movementCost",MovementCost.ToString());

	}

	public void ReadXml (XmlReader reader){
		Debug.Log ("Reading Furniture");
		MovementCost = int.Parse (reader.GetAttribute ("movementCost"));
	}

}

