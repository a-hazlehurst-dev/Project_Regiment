using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class TileDataGrid : IXmlSerializable{

	public Tile[,] GridMap {get; protected set;}

	public Dictionary<string, Furniture> FurnitureObjectPrototypes;
	public int GridHeight { get ; protected set; }
	public int GridWidth {get; protected set;}
	public float TileWidth { get; protected set; }
	public float TileHeight { get; protected set; }

    public int treeCount = 0;


	public TileDataGrid(int gridheight, int gridWidth, float tileHeight,float tileWidth )
	{
		GridHeight = gridheight;
		GridWidth = gridWidth;
		TileHeight = tileHeight;
		TileWidth = tileWidth;

		CreateGrid ();	
	}

	private void CreateGrid()
	{
		GridMap = new Tile[GridWidth, GridHeight];

		for (int x = 0; x < GridWidth; x++) 
		{
			for (int y = 0; y < GridHeight; y++) 
			{
				GridMap [x, y] = new Tile (x, y,0);
			}
		}

		CreateFurnitureObjectPrototypes ();

	}

	public Tile GetTileAt (int x, int y)
	{
        //Debug.Log("Finding ( " + x + ", " + y + ") does not exist");
        if (x >= GridWidth || x < 0 || y >= GridHeight || y < 0) {
			
			return null;
		}

		return GridMap [x, y];
	}

	private void CreateFurnitureObjectPrototypes()
	{
		FurnitureObjectPrototypes = new Dictionary<string, Furniture> ();

		FurnitureObjectPrototypes.Add ("wall", Furniture.CreatePrototype ("wall", 0, 1, 1, true));
	}

	public bool IsFurniturePlacementValid(string furnitureType, Tile t){
		return FurnitureObjectPrototypes [furnitureType].IsValidPosition (t);
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
	}

	public void ReadXml (XmlReader reader){


	}

		

}
