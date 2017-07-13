﻿using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour {

    public GameObject[] grassFloorTiles;
    public GameObject[] mudFloorTiles;
    public GameObject[] floorEmbelishmentTiles;
    public GameObject[] naturalTiles;


	public Dictionary<string, Sprite> FurnitureObjects;
	public Dictionary<string, Sprite> CharacterObjects;
	public Dictionary<string, Sprite> InventoryObjects;

    public GameObject[] interactableTiles;
    public GameObject[] peasentSprites;


    public void Awake()
    {
		
    }

    public void Init()
    {
        FurnitureObjects = new Dictionary<string, Sprite>();
        CharacterObjects = new Dictionary<string, Sprite>();
		InventoryObjects = new Dictionary<string, Sprite> ();
        LoadResources();

    }

    public void LoadResources()
    {
		LoadFurniture ();
		LoadCharacters ();
		LoadInventory ();
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
