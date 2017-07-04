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

	private int optionAction;
	private static bool loadGameMode = false;
   

	Action<Character> cbCharacterCreated;
	List<Character> characters ;
    public PathTileGraph TileGraph;// pathfinding graph for walkable tiles.

    public JobQueue JobQueue;


	private int _drawMode = 1;
	private string _drawObject;

	void OnEnable(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

		Instance = this;
			
		if (!loadGameMode) {
			InitGame ();
		} else {
			loadGameMode = false;
			LoadGameFromFile ();
		}

	}
	public void InvalidateTileGraph(){
		TileGraph = null;
	}



	void Update(){
		
		foreach (var c in characters) {
			c.Update (Time.deltaTime);
		}

	}

	public Character CreateCharacter(Tile t){
		Character c = new Character (TileDataGrid.GridMap [TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2]);
		if (cbCharacterCreated != null) {
			cbCharacterCreated (c);
		}
		characters.Add (c);

		return c;
	}

	void InitGame(){
		characters = new List<Character> ();
		JobQueue = new JobQueue ();
		SpriteManager = GetComponent<SpriteManager>();
		TileManager = GetComponent<TileManager> ();
		FurnitureManager = GetComponent<FurnitureManager> ();
		CharacterSpriteManager = GetComponent<CharacterSpriteManager> ();

		TileDataGrid = new TileDataGrid (100,100,64,64);
		TileManager.InitialiseTileMap(SpriteManager, 100,100, 64,64);
        FurnitureManager.InitialiseFurniture (SpriteManager);
        TileGraph = new PathTileGraph(TileDataGrid);
        CharacterSpriteManager.InitialiseCharacter (SpriteManager);
        

    }

	void NewGame(){
		Debug.Log ("Restarting....");
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	void SaveGame(){
		Debug.Log ("Saving....");

		XmlSerializer xmlSerializer = new XmlSerializer (typeof (TileDataGrid));
		TextWriter writer = new StringWriter ();
		xmlSerializer.Serialize (writer, TileDataGrid);

		Debug.Log (writer.ToString ());


	}

	void LoadGame(){
		Debug.Log ("Loading...");
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	void LoadGameFromFile(){
		loadGameMode = true;
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


	public void RegisterCharacterCreated(Action<Character> callBackFunction){
		cbCharacterCreated += callBackFunction;
	}

	public void UnRegisterCharacterCreated(Action<Character> callBackFunction){
		cbCharacterCreated -= callBackFunction;
	}

	public void SetGameOptions(int optionAction){
		this.optionAction = optionAction;
		switch (this.optionAction) {
		case 1:
			NewGame();
			break;
		case 2:
			SaveGame();
			break;
		case 3:
			LoadGame();
			break;
		}
	}
}
