using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GridManager : MonoBehaviour 
{
	public Grid Grid;
	public int GridHeight =10;
	public int GridWidth =10;
	public float TileWidth = 64;
	public float TileHeight = 64;

	
	private Transform gridHolder;

	public void GridSetup(SpriteManager spriteManager)
	{

		if (Grid == null) 
		{
			Grid = new Grid (GridHeight, GridWidth, TileHeight, TileWidth);
		}

		gridHolder = new GameObject ("Grid").transform;
        var yes = 0;
        var no = 0;
        var count = 0;

        
        for (int y = Grid.GridHeight-1; y >=0 ; y--)
        { 
            for (int x = 0; x <= Grid.GridWidth-1; x++)
            {
                count++;
               
                var tile = Grid.GridMap[x,y];
             
                GameObject toInstanciate = spriteManager.floorTiles[tile.Floor];
                GameObject instance = Instantiate(toInstanciate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(gridHolder);

                //if (y <1||y> Grid.GridHeight-2) continue;
                //if (x < 1 || x > Grid.GridWidth-1) continue;

                
                if (!tile.Tree)
                {
                    no++;
                    continue;
                }
                else
                {
                    yes++;
                    //            GameObject toEmbelishWith = spriteManager.floorEmbelishmentTiles [UnityEngine.Random.Range (0, spriteManager.floorEmbelishmentTiles.Length)];
                    //GameObject instanceOfEmbelish = Instantiate (toEmbelishWith, new Vector3 (x , y, 0), Quaternion.identity) as GameObject;
                    //instanceOfEmbelish.transform.SetParent (gridHolder);

                    GameObject toNaturaliseWith = spriteManager.naturalTiles[UnityEngine.Random.Range(0, spriteManager.naturalTiles.Length)];
                    GameObject instanciateNatural = Instantiate(toNaturaliseWith, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    instanciateNatural.transform.SetParent(gridHolder);
                }
               
    
			}
		}
        Debug.Log("Rendering: Y" + yes + ", N" + no + ", Count : " +count);
        Debug.Log("Tree count: " + Grid.treeCount);

    }
		

}
