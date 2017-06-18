using System;
using UnityEngine;

public class GameManager : MonoBehaviour {



	public static GameManager Instance { get; protected set;}

	public GridManager  GridManager { get; protected set;}
	public SpriteManager SpriteManager { get; protected set;}
	public PeasantManager PeasantManager{ get; protected set;}

    private Tile.FloorType _drawMode = Tile.FloorType.Grass;

	void Awake(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

		Instance = this;
		SpriteManager = GetComponent<SpriteManager>();
        PeasantManager = GetComponent<PeasantManager>();
        GridManager = GetComponent<GridManager> ();
	
		InitGame();
	}

    public Tile GetTileAtWorldCoordinate(Vector3 coordinate)
    {
        int x = Mathf.FloorToInt(coordinate.x);
        int y = Mathf.FloorToInt(coordinate.y);
    
        return GridManager.GetTileAt(x, y);
    }

    public void SetDrawMode(Tile.FloorType newtype)
    {
        _drawMode = newtype;
    }

    public Tile.FloorType GetDrawMode()
    {
        return _drawMode;
    }

    void InitGame(){
		GridManager.GridSetup (SpriteManager);

    }

    void Update()
    {
        
    }

	
}
