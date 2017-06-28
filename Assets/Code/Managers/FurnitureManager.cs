using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FurnitureManager : MonoBehaviour 
{
	private Dictionary<Furniture, GameObject> _furnitureGameObjectMap;

    private SpriteManager _spriteManager;
	private Transform furnitureHolder;

	void Start(){
		_furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		furnitureHolder = new GameObject ("Furniture").transform;
	}

	public void InitialiseFurniture(SpriteManager spriteManager)
	{
		_spriteManager = spriteManager;
	}


	public void PlaceFurniture(string itemToBuild, Tile tile){

		if (GameManager.Instance.TileDataGrid.FurnitureObjectPrototypes.ContainsKey (itemToBuild) == false) {
			Debug.LogError ("FurnitureObjectPrototypes does not contain prototype for key: " + itemToBuild);
			return;
		}

		var furnitureToInstall = Furniture.PlaceFurniture (GameManager.Instance.TileDataGrid.FurnitureObjectPrototypes [itemToBuild], tile);
        if(furnitureToInstall == null) { return; }
        
        OnFurnitureCreated(furnitureToInstall);
		GameManager.Instance.InvalidateTileGraph();
	}

    public Sprite GetSpriteForFurniture(Furniture furnitureItem)
    {
        if(furnitureItem.LinksToNeighbour == false)
        {
			return _spriteManager.FurnitureObjects[furnitureItem.ObjectType];
        }

        string spriteName = furnitureItem.ObjectType + "_";

        Tile t;
        int x = furnitureItem.Tile.X;
        int y = furnitureItem.Tile.Y;

		t = GameManager.Instance.TileDataGrid.GetTileAt(x, y + 1);
        if(t!=null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "N";
        }

		t = GameManager.Instance.TileDataGrid.GetTileAt(x+1, y );
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "E";
        }

		t = GameManager.Instance.TileDataGrid.GetTileAt(x, y - 1);
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "S";
        }
		t = GameManager.Instance.TileDataGrid.GetTileAt(x-1, y);
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furnitureItem.ObjectType)
        {
            spriteName += "W";
        }

       
		if (!_spriteManager.FurnitureObjects.ContainsKey (spriteName)) {
			Debug.LogError ("furnitureObjects: Cannot find sprite called:" + spriteName);
			return null;
		}

		return _spriteManager.FurnitureObjects[spriteName];
    }

	public Sprite GetSpriteForFurniture(string objectType)
	{
		if (_spriteManager.FurnitureObjects.ContainsKey (objectType)) {
			return _spriteManager.FurnitureObjects[objectType];
		}
		if (_spriteManager.FurnitureObjects.ContainsKey (objectType+"_")) {
			return _spriteManager.FurnitureObjects [objectType + "_"];

			}
		Debug.LogError ("GetSpriteForFurniture: Cannot find sprite called:" + objectType);

		return null;

	}

	public void OnFurnitureCreated(Furniture furnitureToInstall){
		
		GameObject furnitureToRender = new GameObject("wall: x: "+ furnitureToInstall.Tile.X + ", y" +furnitureToInstall.Tile.Y);

		_furnitureGameObjectMap.Add(furnitureToInstall, furnitureToRender);

		var sr = furnitureToRender.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = "Furniture";
		sr.sprite = GetSpriteForFurniture(furnitureToInstall) ; //FIXME wall does not exist.
		furnitureToRender.transform.position = new Vector3(furnitureToInstall.Tile.X, furnitureToInstall.Tile.Y, 0);

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
		var sr = furn_go.GetComponent<SpriteRenderer> ();
		sr.sprite =GetSpriteForFurniture(furn);
		sr.sortingLayerName = "Furniture";


    }

 
}
