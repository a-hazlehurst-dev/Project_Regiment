using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Assets.Code.Helper;

public class GridManager : MonoBehaviour 
{
	private Dictionary<Furniture, GameObject> _furnitureGameObjectMap;

	public Grid Grid;
	public int GridHeight;
	public int GridWidth;
	public float TileWidth;
	public float TileHeight;
    private SpriteManager _spriteManager;
	private Transform gridHolder;
	private Transform furnitureHolder;

	public void GridSetup(SpriteManager spriteManager)
	{
        _spriteManager = spriteManager;
		_furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		if (Grid == null) 
		{
			Grid = new Grid (GridHeight, GridWidth, TileHeight, TileWidth);
		}
		gridHolder = new GameObject ("Grid").transform;
		furnitureHolder = new GameObject ("Furniture").transform;

        RenderBase();
    }

	public void PlaceFurniture(string itemToBuild, Tile tile){
		if (Grid.FurnitureObjectPrototypes.ContainsKey (itemToBuild) == false) {
			Debug.LogError ("FurnitureObjectPrototypes does not contain prototype for key: " + itemToBuild);
			return;
		}

		var furnitureToInstall = Furniture.PlaceFurniture (Grid.FurnitureObjectPrototypes [itemToBuild], tile);
        if(furnitureToInstall == null) { return; }

        //create graphics for installed object.
        OnFurnitureCreated(furnitureToInstall);
	}

    Sprite GetGameObjectForInstallObject(Furniture furnitureItem)
    {
        if(furnitureItem.LinksToNeighbour == false)
        {
            return _spriteManager.furnitureObjects[furnitureItem.ObjectType];
        }

        string spriteName = furnitureItem.ObjectType + "_";

        Tile t;
        int x = furnitureItem.Tile.X;
        int y = furnitureItem.Tile.Y;

        t = GetTileAt(x, y + 1);
        if(t!=null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "N";
        }

        t = GetTileAt(x+1, y );
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "E";
        }

        t = GetTileAt(x, y - 1);
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "S";
        }
        t = GetTileAt(x-1, y);
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "W";
        }

       
		if (!_spriteManager.furnitureObjects.ContainsKey (spriteName)) {
			Debug.LogError ("furnitureObjects: Cannot find sprite called:" + spriteName);
			return null;
		}

        return _spriteManager.furnitureObjects[spriteName];
    }

	public void OnFurnitureCreated(Furniture furnitureToInstall){
		
		GameObject furnitureToRender = new GameObject("wall: x: "+ furnitureToInstall.Tile.X + ", y" +furnitureToInstall.Tile.Y);

		furnitureToRender.AddComponent<SpriteRenderer> ().sortingLayerName = "active";
		furnitureToRender.GetComponent<SpriteRenderer>().sprite = GetGameObjectForInstallObject(furnitureToInstall) ; //FIXME wall does not exist.
		furnitureToRender.transform.position = new Vector3(furnitureToInstall.Tile.X, furnitureToInstall.Tile.Y, 0);

		_furnitureGameObjectMap.Add(furnitureToInstall, furnitureToRender);

		furnitureToRender.transform.SetParent (furnitureHolder);
		   
		furnitureToInstall.RegisterOnChangedCallback ( OnFurnitureChanged);
	}

	public void OnFurnitureChanged(Furniture furn){
   
        if (_furnitureGameObjectMap.ContainsKey(furn) == false)
        {
            Debug.LogError("OnFurnitureChanged: trying to change furniture not in our map.");
            return;
        }

        GameObject furn_go = _furnitureGameObjectMap[furn];
		furn_go.GetComponent<SpriteRenderer>().sprite = GetGameObjectForInstallObject(furn);

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
