using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour {

    public Dictionary<string, Sprite> FloorTiles;
    public Dictionary<string, Sprite> FurnitureObjects;
	public Dictionary<string, Sprite> CharacterObjects;
	public Dictionary<string, Sprite> InventoryObjects;

    public void Init()
    {
        FloorTiles = new Dictionary<string, Sprite>();
        FurnitureObjects = new Dictionary<string, Sprite>();
        CharacterObjects = new Dictionary<string, Sprite>();
		InventoryObjects = new Dictionary<string, Sprite> ();
        LoadResources();

    }

    public void LoadResources()
    {
        LoadFloorTiles();
		LoadFurniture ();
		LoadCharacters ();
		LoadInventory ();
    }

    private void LoadFloorTiles()
    {
        var floorObjects = Resources.LoadAll<Sprite>("Images/grass/");

        foreach (var go in floorObjects)
        {
            FloorTiles.Add(go.name, go);
        }


    }

    private void LoadFurniture(){

        var wallObjects = Resources.LoadAll<Sprite>("Images/wall/");

        foreach(var go in wallObjects)
        {
			FurnitureObjects.Add(go.name, go);
        }

        var doorObjects = Resources.LoadAll<Sprite>("Images/door/");

        foreach (var go in doorObjects)
        {
            FurnitureObjects.Add(go.name, go);
        }

        var stockpileObjects = Resources.LoadAll<Sprite>("Images/StockPile/");

        foreach (var go in stockpileObjects)
        {
            FurnitureObjects.Add(go.name, go);
        }

        var itemObjects = Resources.LoadAll<Sprite>("Images/Items/");

        foreach (var go in itemObjects)
        {
            FurnitureObjects.Add(go.name, go);
        }
    }

	private void LoadInventory(){
		var inventoryObjects = Resources.LoadAll<Sprite>("Images/Inventory/"); 

		foreach (var go in inventoryObjects)
		{
			InventoryObjects.Add(go.name, go);
		}
	}

	private void LoadCharacters(){
		var charObjects = Resources.LoadAll<Sprite>("Images/characters/");

		foreach(var go in charObjects)
		{
			CharacterObjects.Add(go.name, go);
		}
	}

	public Sprite GetCharacterPrototype(string objectType){
		if (!CharacterObjects.ContainsKey (objectType)) {
			Debug.LogError ("sprite manager could not find Character type with key, " + objectType);
		}
		return FurnitureObjects [objectType];
	}

	public Sprite GetFuniturePrototype(string objectType){

		if (!FurnitureObjects.ContainsKey (objectType)) {
			Debug.LogError ("sprite manager could not find furniture type with key, " + objectType);
		}
		return FurnitureObjects [objectType];
	}
}
