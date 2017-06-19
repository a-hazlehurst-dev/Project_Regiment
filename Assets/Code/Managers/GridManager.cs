using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Assets.Code.Helper;

public class GridManager : MonoBehaviour 
{
	private Dictionary<FurnitureItem, GameObject> _furnitureGameObjectMap;

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
		_furnitureGameObjectMap = new Dictionary<FurnitureItem, GameObject> ();
		if (Grid == null) 
		{
			Grid = new Grid (GridHeight, GridWidth, TileHeight, TileWidth);
		}

		gridHolder = new GameObject ("Grid").transform;

        RenderBase();
       

    }

	public void PlaceFurniture(string itemToBuild, Tile tile){
		Debug.Log ("Placing installed Object");
		if (Grid.FurnitureObjectPrototypes.ContainsKey (itemToBuild) == false) {
			Debug.LogError ("FurnitureObjectPrototypes does not contain prototype for key: " + itemToBuild);
			return;
		}

		var furnitureToInstall = FurnitureItem.PlaceFurniture (Grid.FurnitureObjectPrototypes [itemToBuild], tile);

		//create graphics for installed object.
		RenderFurnitureItem(furnitureToInstall);

	}

	public void RenderFurnitureItem(FurnitureItem furnitureToInstall){
		GameObject furnitureToRender = null;

		var furniture = _spriteManager.furnitureObjects ["wall"]; //FIXME wall does not exist.

		GameObject instance = Instantiate(furniture, new Vector3(furnitureToInstall.Tile.X, furnitureToInstall.Tile.Y, 0), Quaternion.identity) as GameObject;

		instance.name = "wall(" + furnitureToInstall.Tile.X+ ", " + furnitureToInstall.Tile.Y + ")";
		instance.transform.SetParent(gridHolder);

		furnitureToInstall.RegisterOnChangedCallback ( OnFurnitureChanged);
	}

	private void OnFurnitureChanged(FurnitureItem changedItem){
		Debug.LogError ("OnFurnitureChanged: Not implemented.");
	}

    private void RenderBase()
    {
        for (int x = 0; x < Grid.GridWidth; x++)
        { 
            for (int y = 0; y < Grid.GridHeight; y++)
            {
				var tile = GetTileAt (x, y);
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
                instance.transform.SetParent(gridHolder);

				tile.RegisterFloorTypeChangedCb ( (tile_data) => { OnTileTypeChanged(tile_data, instance);});
            }
        }
		

    }

    public Tile[] GetTileNeighbours(Tile centreTile)
    {
        Debug.Log("Getting neighbours of tile ");
        int x = centreTile.X;
        int y = centreTile.Y;
        Tile[] neighbours = new Tile[9];
        neighbours[0] = GetTileAt(x - 1, y - 1);
        neighbours[1] = GetTileAt(x    , y - 1);
        neighbours[2] = GetTileAt(x + 1, y - 1);
        neighbours[3] = GetTileAt(x - 1, y );
        neighbours[4] = centreTile;
        neighbours[5] = GetTileAt(x + 1, y );
        neighbours[6] = GetTileAt(x - 1, y +1);
        neighbours[7] = GetTileAt(x    , y +1);
        neighbours[8] = GetTileAt(x + 1, y +1);

        return neighbours;

    }

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go){
		if (_spriteManager.grassFloorTiles.Length < (int)tile_data.Floor) {
			Debug.LogError ("GridManager.OnTileTypeChanged cannot find floor type with index: " + tile_data.Floor);
		} 
		else {


            var tileNeighbours = GetTileNeighbours(tile_data);
            var floorType = TileRenderHelper.TileToRender(tileNeighbours);
            if(tile_data.Floor == Tile.FloorType.Grass)
            {
                tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.grassFloorTiles[floorType].GetComponent<SpriteRenderer>().sprite;
            }
            else if (tile_data.Floor == Tile.FloorType.Mud)
            {
                tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.mudFloorTiles[floorType].GetComponent<SpriteRenderer>().sprite;
            }
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
