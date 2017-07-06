using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FurnitureController : MonoBehaviour 
{

	//responsible for drawing furniture on int game.
	private Dictionary<Furniture, GameObject> _furnitureGameObjectMap;
	public Dictionary<string, Furniture> FurnitureObjectPrototypes;

    private SpriteManager _spriteManager;
	private Transform furnitureHolder;
	private FurnitureService _furnitureService;

	void Start(){
		

		furnitureHolder = new GameObject ("Furniture").transform;
	}

	public void InitialiseFurniture(SpriteManager spriteManager, FurnitureService furnitureService)
	{
        _furnitureGameObjectMap = new Dictionary<Furniture, GameObject>();
        _spriteManager = spriteManager;
		_furnitureService = furnitureService;
        _furnitureService.Register_OnFurniture_Created(OnFurnitureCreated);
	}


	public bool IsFurniturePlacementValid(string furnitureType, Tile t){
		return _furnitureService.IsValidPosition (furnitureType, t);
	}

	public Furniture PlaceFurniture(string itemToBuild, Tile tile){

		var furnToInstall = _furnitureService.CreateFurniture(itemToBuild, tile);
        if(furnToInstall == null) { return null; }
        
		OnFurnitureCreated(furnToInstall);

		GameManager.Instance.InvalidateTileGraph();

		foreach(var furn in _furnitureService.FindAll())
		{
			OnFurnitureCreated(furn);
		}

		return furnToInstall;

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

        if (_furnitureGameObjectMap.ContainsKey(furnitureToInstall))
        {
            return;
        }

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
