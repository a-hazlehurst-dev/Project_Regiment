using System;
using UnityEngine;

public class GameManager : MonoBehaviour {



	public static GameManager Instance { get; protected set;}

	public GridManager  GridManager { get; protected set;}
	public SpriteManager SpriteManager { get; protected set;}
	public PeasantManager PeasantManager{ get; protected set;}

	private int _drawMode = 1;
	private string _drawObject;

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

    void InitGame(){
		GridManager.GridSetup (SpriteManager);

    }

	
}
