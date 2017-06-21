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
	}

    Sprite GetGameObjectForFurniture(Furniture furnitureItem)
    {
        if(furnitureItem.LinksToNeighbour == false)
        {
            return _spriteManager.furnitureObjects[furnitureItem.ObjectType];
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

       
		if (!_spriteManager.furnitureObjects.ContainsKey (spriteName)) {
			Debug.LogError ("furnitureObjects: Cannot find sprite called:" + spriteName);
			return null;
		}

        return _spriteManager.furnitureObjects[spriteName];
    }

	public void OnFurnitureCreated(Furniture furnitureToInstall){
		
		GameObject furnitureToRender = new GameObject("wall: x: "+ furnitureToInstall.Tile.X + ", y" +furnitureToInstall.Tile.Y);

		furnitureToRender.AddComponent<SpriteRenderer> ().sortingLayerName = "active";
		furnitureToRender.GetComponent<SpriteRenderer>().sprite = GetGameObjectForFurniture(furnitureToInstall) ; //FIXME wall does not exist.
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
		furn_go.GetComponent<SpriteRenderer>().sprite = GetGameObjectForFurniture(furn);

    }

 
}
