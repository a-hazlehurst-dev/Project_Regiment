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
    private SpriteManager _spriteManager;
	
	private Transform gridHolder;

	public void GridSetup(SpriteManager spriteManager)
	{
        _spriteManager = spriteManager;
		if (Grid == null) 
		{
			Grid = new Grid (GridHeight, GridWidth, TileHeight, TileWidth);
		}

		gridHolder = new GameObject ("Grid").transform;

        RenderBase();
       

    }

    private void RenderBase()
    {
        for (int y = Grid.GridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x <= Grid.GridWidth - 1; x++)
            {
                var tile = Grid.GridMap[x, y];

                GameObject toInstanciate = _spriteManager.floorTiles[tile.Floor];
                GameObject instance = Instantiate(toInstanciate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(gridHolder);
            }
        }
    }
}
