using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; protected set;}
	public FurnitureManager FurnitureManager { get; protected set; }
	public TileManager TileManager { get; protected set; }
	public SpriteManager SpriteManager { get; protected set;}
	public TileDataGrid TileDataGrid { get; protected set; }

	public JobQueue JobQueue;


	private int _drawMode = 1;
	private string _drawObject;

	void OnEnable(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

		Instance = this;
		JobQueue = new JobQueue ();
		SpriteManager = GetComponent<SpriteManager>();
		TileManager = GetComponent<TileManager> ();
		FurnitureManager = GetComponent<FurnitureManager> ();
	
		InitGame();
	}

	void InitGame(){
		TileDataGrid = new TileDataGrid (100,100,64,64);
		TileManager.InitialiseTileMap(SpriteManager, 100,100, 64,64);
		FurnitureManager.InitialiseFurniture (SpriteManager);

	}

    public Tile GetTileAt(Vector3 coordinate)
    {
        int x = Mathf.FloorToInt(coordinate.x);
        int y = Mathf.FloorToInt(coordinate.y);
    
		return TileDataGrid.GetTileAt(x, y);
    }

    public void SetDrawMode(int mode, string type)
    {
		_drawMode = mode;
		_drawObject = type;
    }

    public int GetDrawMode()
    {
        return _drawMode;
    }

	public string GetDrawObjectMode(){
		return _drawObject;
	}

}
