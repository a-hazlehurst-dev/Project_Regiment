using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GridManager : MonoBehaviour 
{
	public Grid Grid;
	public int GridHeight;
	public int GridWidth;
	public float TileWidth;
	public float TileHeight;
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
        for (int x = 0; x < Grid.GridWidth; x++)
        { 
            for (int y = 0; y < Grid.GridHeight; y++)
            {
				var tile = GetTileAt (x, y);

				GameObject toInstanciate = _spriteManager.floorTiles[(int)tile.Floor];
                GameObject instance = Instantiate(toInstanciate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
				instance.name = "tile_(" + tile.X + ", " + tile.Y + ")";
                instance.transform.SetParent(gridHolder);

				tile.RegisterFloorTypeChangedCb ( (tile_data) => { OnTileTypeChanged(tile_data, instance);});
            }
        }
		GetTileAt (50,50).Floor = Tile.FloorType.Mud;

    }

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go){
		if (_spriteManager.floorTiles.Length < (int)tile_data.Floor) {
			Debug.LogError ("GridManager.OnTileTypeChanged cannot find floor type with index: " + tile_data.Floor);
		} 
		else {
			tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.floorTiles [(int)tile_data.Floor].GetComponent<SpriteRenderer>().sprite;


		}
	}


	public Tile GetTileAt (int x, int y)
	{

		if (x > GridWidth || x < 0 || y > GridHeight || y < 0) {
			Debug.LogError ("Tile ( " + x + ", " + y + ") does not exist");
			return null;
		}
		return Grid.GridMap [x, y];
	}


}
