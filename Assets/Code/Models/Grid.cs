using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

	public Tile[,] GridMap {get; protected set;}

	public Dictionary<string, FurnitureItem> FurnitureObjectPrototypes;
	public int GridHeight { get ; protected set; }
	public int GridWidth {get; protected set;}
	public float TileWidth { get; protected set; }
	public float TileHeight { get; protected set; }

    public int treeCount = 0;


	public Grid(int gridheight, int gridWidth, float tileHeight,float tileWidth )
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

	private void CreateFurnitureObjectPrototypes()
	{
		FurnitureObjectPrototypes = new Dictionary<string, FurnitureItem> ();

		FurnitureObjectPrototypes.Add ("wall", FurnitureItem.CreatePrototype ("Wall", 0, 1, 1));
	}





		

}
