using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDataGrid {

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
		if (x > GridWidth || x < 0 || y > GridHeight || y < 0) {
			Debug.LogError ("Tile ( " + x + ", " + y + ") does not exist");
			return null;
		}
		return GridMap [x, y];
	}

	private void CreateFurnitureObjectPrototypes()
	{
		FurnitureObjectPrototypes = new Dictionary<string, Furniture> ();

		FurnitureObjectPrototypes.Add ("wall", Furniture.CreatePrototype ("wall", 0, 1, 1, true));
	}





		

}
