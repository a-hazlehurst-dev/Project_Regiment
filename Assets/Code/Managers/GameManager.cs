using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Code.Services.Pathfinding;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;


public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; protected set;}
	public FurnitureManager FurnitureManager { get; protected set; }
	public TileManager TileManager { get; protected set; }
	public SpriteManager SpriteManager { get; protected set;}
	public TileDataGrid TileDataGrid { get; protected set; }
	public CharacterSpriteManager CharacterSpriteManager { get; protected set; }
	public XmlGame xmlGame;
	public CharacterManager CharacterManager { get; protected set; }
	private int optionAction;
   


    public PathTileGraph TileGraph;// pathfinding graph for walkable tiles.

    public JobQueue JobQueue;


	private int _drawMode = 1;
	private string _drawObject;

	void OnEnable(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

		Instance = this;
		xmlGame = new XmlGame ();

		CharacterManager = new CharacterManager ();
		JobQueue = new JobQueue ();
		SpriteManager = GetComponent<SpriteManager>();
		TileManager = GetComponent<TileManager> ();
		FurnitureManager = GetComponent<FurnitureManager> ();
		CharacterSpriteManager = GetComponent<CharacterSpriteManager> ();
		SpriteManager.Initialise ();

		if (!XmlGame.loadGameMode) {
			InitGame ();
		} else {
			XmlGame.loadGameMode = false;
			TileDataGrid = xmlGame.CreateGameFromSaveFile (SpriteManager, TileDataGrid);

			TileManager.InitialiseTileMap (SpriteManager, TileDataGrid);
			TileGraph = new PathTileGraph (TileDataGrid);
			CharacterSpriteManager.InitialiseCharacter (SpriteManager, TileDataGrid, CharacterManager);
		}		
		GameObject.Find ("CameraDolly").transform.position = new Vector3 (TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2, -11);

	}



	public void SetGameOptions(int optionAction){
		this.optionAction = optionAction;
		switch (this.optionAction) {
		case 1:
			xmlGame.NewGame();
			break;
		case 2:
			xmlGame.SaveGame(TileDataGrid);
			break;
		case 3:
			xmlGame.LoadGame();
			break;
		}
	}
	public void InvalidateTileGraph(){
		TileGraph = null;
	}



	void Update(){
		CharacterManager.Update (Time.deltaTime);
	}


	void InitGame(){

		TileDataGrid = new TileDataGrid (100,100,64,64);
		TileManager.InitialiseTileMap(SpriteManager,TileDataGrid);
        FurnitureManager.InitialiseFurniture (SpriteManager);
        TileGraph = new PathTileGraph(TileDataGrid);
		CharacterSpriteManager.InitialiseCharacter (SpriteManager,TileDataGrid, CharacterManager);

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
