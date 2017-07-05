using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class TileDataGrid : IXmlSerializable{

	public Tile[,] GridMap {get; protected set;}

	//public Dictionary<string, Furniture> FurnitureObjectPrototypes;
	public int GridHeight { get ; protected set; }
	public int GridWidth {get; protected set;}
	public float TileWidth { get; protected set; }
	public float TileHeight { get; protected set; }

    public int treeCount = 0;


	public TileDataGrid(int gridheight, int gridWidth, float tileHeight,float tileWidth )
	{

		CreateGrid (gridWidth, gridheight, tileWidth, tileHeight);	
	}


	private void CreateGrid(int width, int height, float tileWidth,float tileHeight )
	{
		GridHeight = height;
		GridWidth = width;

		GridMap = new Tile[GridWidth, GridHeight];

		for (int x = 0; x < GridWidth; x++) 
		{
			for (int y = 0; y < GridHeight; y++) 
			{
				GridMap [x, y] = new Tile (x, y,0);
			}
		}

		//GameManager.Instance.FurnitureManager.CreateFurnitureObjectPrototypes ();

	}

	public Tile GetTileAt (int x, int y)
	{
        //Debug.Log("Finding ( " + x + ", " + y + ") does not exist");
        if (x >= GridWidth || x < 0 || y >= GridHeight || y < 0) {
			
			return null;
		}

		return GridMap [x, y];
	}




	/// <summary>
	/// SAVING & LOADING XML STUFF
	/// </summary>

	public TileDataGrid(){
		
	}

	public XmlSchema GetSchema(){
		return null;
	}

	public void WriteXml (XmlWriter writer){
		writer.WriteAttributeString ("Width", GridWidth.ToString());
		writer.WriteAttributeString("Height", GridHeight.ToString());

		writer.WriteStartElement ("Tiles");
		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
				writer.WriteStartElement ("Tile");
				GridMap [x, y].WriteXml (writer);
				writer.WriteEndElement ();
			}
		}
		writer.WriteEndElement ();

		writer.WriteStartElement ("Furnitures");

		foreach (var furn in GameManager.Instance.FurnitureManager.Furnitures) {
			Debug.Log ("saving furnitures.");
			writer.WriteStartElement ("Furniture");
			furn.WriteXml (writer);
			writer.WriteEndElement ();
		}
			
		writer.WriteEndElement ();
	}

	public TileDataGrid ReadSetup(XmlReader reader){
		GridWidth = int.Parse (reader.GetAttribute ("Width"));
		GridHeight = int.Parse(reader.GetAttribute ("Height"));

		CreateGrid (GridWidth,GridHeight,64,64);

		return this;
	}

	public void ReadXml (XmlReader reader){

		var tileDataGrid = ReadSetup (reader);

		while (reader.Read ()) {
			switch (reader.Name) {
			case "Tiles":
				ReadXML_Tiles (reader);
				break;
			case "Furnitures":
				ReadXml_Furnitures (reader,tileDataGrid);
				break;
			}
		}

	
	}

	public void ReadXML_Tiles(XmlReader reader){
		while (reader.Read ()) {
			if (reader.Name != "Tile") {
				return;
			}

			var x  = int.Parse (reader.GetAttribute ("X"));
			var y = int.Parse (reader.GetAttribute ("Y"));
		
			GridMap [x, y].ReadXml (reader);
		}
	
	}

	public void ReadXml_Furnitures(XmlReader reader, TileDataGrid tileDataGrid){
		while (reader.Read ()) {
			if (reader.Name != "Furniture") {
				return;
			}
			var x  = int.Parse (reader.GetAttribute ("X"));
			var y = int.Parse (reader.GetAttribute ("Y"));

			var furn = GameManager.Instance.FurnitureManager.PlaceFurniture (reader.GetAttribute ("objectType"), GridMap [x, y], tileDataGrid);
			furn.ReadXml (reader);

		}
	}
		

}
