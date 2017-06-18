using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public GameObject[] grassFloorTiles;
    public GameObject[] mudFloorTiles;
    public GameObject[] floorEmbelishmentTiles;
    public GameObject[] naturalTiles;

    public GameObject[] interactableTiles;
    public GameObject[] peasentSprites;

    public void Awake()
    {
        LoadResources();
    }

    public void LoadResources()
    {

    }
}
