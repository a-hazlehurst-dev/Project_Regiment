using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour {

    public GameObject[] grassFloorTiles;
    public GameObject[] mudFloorTiles;
    public GameObject[] floorEmbelishmentTiles;
    public GameObject[] naturalTiles;


	public Dictionary<string, Sprite> furnitureObjects;

    public GameObject[] interactableTiles;
    public GameObject[] peasentSprites;

    public void Awake()
    {
		furnitureObjects = new Dictionary<string, Sprite> ();
        LoadResources();
    }

    public void LoadResources()
    {
		LoadFurniture ();
    }

	private void LoadFurniture(){

        var wallObjects = Resources.LoadAll<Sprite>("Images/wall/");

        foreach(var go in wallObjects)
        {
            furnitureObjects.Add(go.name, go);
        }
	}
}
