using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour {

    public GameObject[] grassFloorTiles;
    public GameObject[] mudFloorTiles;
    public GameObject[] floorEmbelishmentTiles;
    public GameObject[] naturalTiles;

	public GameObject wallSprite;

	public Dictionary<string, GameObject> furnitureObjects;

    public GameObject[] interactableTiles;
    public GameObject[] peasentSprites;

    public void Awake()
    {
		furnitureObjects = new Dictionary<string, GameObject> ();
        LoadResources();
    }

    public void LoadResources()
    {
		LoadFurniture ();
    }

	private void LoadFurniture(){
		furnitureObjects.Add ("wall", wallSprite);
	}
}
