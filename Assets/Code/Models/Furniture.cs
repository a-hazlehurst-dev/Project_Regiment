using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;

public class Furniture  : IXmlSerializable{

	public Dictionary<string, float> furnParameters;
	public Action<Furniture, float> updateActions;

	public Func<Furniture, Enterability> isEnterable;

	public void Update(float deltaTime){
		if (updateActions != null) {
			updateActions (this, deltaTime);
		}
	}


	public  Tile Tile { get; protected set; }					//base tile of object( what you place ) object can be bigger than 1 tile.
	public string ObjectType { get; protected set; }
	public float MovementCost { get; protected set; }
	private int _width = 1;
	private int _height = 1;
    public bool LinksToNeighbour { get; protected set; }


	public Action<Furniture> cbOnChanged;
	private Func<Tile, bool> funcPositionValidation;

	//For Serialization
	public Furniture(){
		furnParameters = new Dictionary<string, float> ();
	
	}

	//Copy Constructors
	protected Furniture(Furniture other){
		this.ObjectType = other.ObjectType;
		this.MovementCost = other.MovementCost;
		this._width = other._width;
		this._height = other._height;

		this.LinksToNeighbour = other.LinksToNeighbour;
		this.furnParameters = new Dictionary<string, float> (other.furnParameters);

		if (other.updateActions != null) {
			this.updateActions = (Action<Furniture,float>)other.updateActions.Clone ();
		}

		this.isEnterable = other.isEnterable;
	}
		

	// create furniture, only used for prototypes
	public Furniture (string objectType, float movementCost = 1f, int width = 1, int height =1, bool linksToNeighbour = false){

		this.ObjectType = objectType;
		this.MovementCost= movementCost;
		this._width = width;
		this._height = height;
		this.LinksToNeighbour = linksToNeighbour;
		this.funcPositionValidation = this.IsValidPosition;
		this.furnParameters = new Dictionary<string, float> ();
	}

	virtual public Furniture Clone(){
		return new Furniture(this);
	}


	static public Furniture PlaceFurniture(Furniture prototype,  Tile tile)
	{
		Furniture item = prototype.Clone();

		item.Tile = tile;
		if (tile.PlaceObject (item)==false)  {
			//if we couldnt place the object.
			//it was likely already occupied
			//do NOT return the object;
			return null;
		}

        if (item.LinksToNeighbour)
        {
			InformNeightboursOfChange (tile, item);

        }

		return item;
	}
	private static void InformNeightboursOfChange(Tile tile, Furniture item){
		//used to update neighbour graphics
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
		//writer.WriteAttributeString ("movementCost",MovementCost.ToString());

		foreach (string k in furnParameters.Keys) {
			writer.WriteStartElement ("Param");
			writer.WriteAttributeString ("name", k);
			writer.WriteAttributeString ("value", furnParameters[k].ToString());
			writer.WriteEndElement ();

		}

	}

	public void ReadXml (XmlReader reader){
		Debug.Log ("Reading Furniture");
		//MovementCost = int.Parse (reader.GetAttribute ("movementCost"));

		if (reader.ReadToDescendant ("Param")) {
			do {
				string k = reader.GetAttribute ("name");
				float v = float.Parse (reader.GetAttribute ("value"));
				furnParameters [k] = v;

			} while(reader.ReadToNextSibling ("Param"));
		}
	}

}

