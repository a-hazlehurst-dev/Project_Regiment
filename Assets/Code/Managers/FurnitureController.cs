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

    public Sprite GetSpriteForFurniture(Furniture furn)
    {
        var spriteName = furn.ObjectType;
        if (furn.LinksToNeighbour == false)
        {
            if (furn.ObjectType == "door")
            {
                if (furn.furnParameters["openness"] < 0.1f)
                {
                    spriteName = "door_";
                }
                else if (furn.furnParameters["openness"] < 0.5f)
                {
                    spriteName = "door_2";
                }
                else if (furn.furnParameters["openness"] < 0.9f)
                {
                    spriteName = "door_3";
                }
                else
                {
                    spriteName = "door_4";
                }
                
            }
            return _spriteManager.FurnitureObjects[spriteName];
        }

        spriteName = furn.ObjectType + "_";

        Tile t;
        int x = furn.Tile.X;
        int y = furn.Tile.Y;

		t = GameManager.Instance.TileDataGrid.GetTileAt(x, y + 1);
        if(t!=null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furn.ObjectType)
        {
            spriteName += "N";
        }

		t = GameManager.Instance.TileDataGrid.GetTileAt(x+1, y );
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furn.ObjectType)
        {
            spriteName += "E";
        }

		t = GameManager.Instance.TileDataGrid.GetTileAt(x, y - 1);
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furn.ObjectType)
        {
            spriteName += "S";
        }
		t = GameManager.Instance.TileDataGrid.GetTileAt(x-1, y);
        if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == furn.ObjectType)
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

		GameObject furnitureToRender = new GameObject(furnitureToInstall.ObjectType+ " "+ furnitureToInstall.Tile.X + ", y" +furnitureToInstall.Tile.Y);

       _furnitureGameObjectMap.Add(furnitureToInstall, furnitureToRender);

		var sr = furnitureToRender.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = "Furniture";
		sr.sprite = GetSpriteForFurniture(furnitureToInstall) ; //FIXME wall does not exist.
		furnitureToRender.transform.position = new Vector3(furnitureToInstall.Tile.X, furnitureToInstall.Tile.Y, 0);

		furnitureToRender.transform.SetParent (furnitureHolder);

        if (furnitureToInstall.ObjectType == "door")
        {
            var northTile = GameManager.Instance.TileDataGrid.GetTileAt(furnitureToInstall.Tile.X, furnitureToInstall.Tile.Y + 1);
            var southTile = GameManager.Instance.TileDataGrid.GetTileAt(furnitureToInstall.Tile.X, furnitureToInstall.Tile.Y - 1);

            if (northTile != null && southTile != null && northTile.InstalledFurniture != null && southTile.InstalledFurniture != null
                && northTile.InstalledFurniture.ObjectType == "wall" && southTile.InstalledFurniture.ObjectType == "wall")
            {

                furnitureToRender.transform.rotation = Quaternion.Euler(0, 0, 90);
                furnitureToRender.transform.Translate(1, 0, 0, Space.World);// ugly hack for bottom left anchor
            }

        }


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
