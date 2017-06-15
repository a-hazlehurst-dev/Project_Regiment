using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

	public Tile[,] GridMap {get; protected set;}
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
            
                var rndTree = Random.Range(0, 50);
                var rndEmbelishment = Random.Range(0, 10);
                var tree = false;
                var embelish = false;
                if(rndTree <= 1)
                {
                    //request a tree
                    tree = true;
                    treeCount++;
                }
                if(rndEmbelishment <=  4)
                {
                    //request embelishment
                    embelish = true;
                }

				GridMap [x, y] = new Tile (x, y, 0,tree,embelish);
			}
		}
			
	}




		

}
