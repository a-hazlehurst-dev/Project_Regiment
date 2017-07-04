using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TileManager : MonoBehaviour 
{
    private SpriteManager _spriteManager;
	private Transform gridHolder;

	// Creates the grid and acts a facade in front.
	public void InitialiseTileMap(SpriteManager spriteManager)
	{
        _spriteManager = spriteManager;

		gridHolder = new GameObject ("Grid").transform;

		CreateInitialTileMapGraphics();
    }

	//Maps the tile data to the tile graphics
    private void CreateInitialTileMapGraphics()
    {
		
		for (int x = 0; x < GameManager.Instance.TileDataGrid.GridWidth; x++)
        { 
			for (int y = 0; y < GameManager.Instance.TileDataGrid.GridHeight; y++)
            {
				var tile = GameManager.Instance.TileDataGrid.GetTileAt (x, y);
                GameObject toInstanciate = null;

                if(tile.Floor == Tile.FloorType.Grass)
                {
                    toInstanciate = _spriteManager.grassFloorTiles[4];
                }
                else if (tile.Floor == Tile.FloorType.Mud)
                {
                    toInstanciate = _spriteManager.mudFloorTiles[4];
                }

                GameObject instance = Instantiate(toInstanciate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
				instance.name = "tile_(" + tile.X + ", " + tile.Y + ")";
				var sr = instance.GetComponent<SpriteRenderer> ();
				sr.sortingLayerName = "Floor";
                instance.transform.SetParent(gridHolder);

				tile.RegisterFloorTypeChangedCb ( (tile_data) => { OnTileTypeChanged(tile_data, instance);});
            }
        }
    }
   
	//callback that response to a change of a tile.
	void OnTileTypeChanged(Tile tile_data, GameObject tile_go){
		if (_spriteManager.grassFloorTiles.Length < (int)tile_data.Floor) {
			Debug.LogError ("GridManager.OnTileTypeChanged cannot find floor type with index: " + tile_data.Floor);
		} 
		else {
            if(tile_data.Floor == Tile.FloorType.Grass)
            {
                tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.grassFloorTiles[4].GetComponent<SpriteRenderer>().sprite;
            }
            else if (tile_data.Floor == Tile.FloorType.Mud)
            {
                tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.mudFloorTiles[4].GetComponent<SpriteRenderer>().sprite;
            }
        }

		GameManager.Instance.InvalidateTileGraph ();
	}




}
