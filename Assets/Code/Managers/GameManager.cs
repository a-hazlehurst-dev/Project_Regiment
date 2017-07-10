using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Code.Services.Pathfinding;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; protected set;}
	public FurnitureController FurnitureController { get; protected set; }
	public TileManager TileManager { get; protected set; }
	public SpriteManager SpriteManager { get; protected set;}
	public TileDataGrid TileDataGrid { get; protected set; }
	public CharacterSpriteManager CharacterSpriteManager { get; protected set; }
	private FurnitureService _furnitureService;
	private CharacterService _characterService;
	private RoomService _roomService;
	private int optionAction;
	private static bool loadGameMode = false;
   


    public PathTileGraph TileGraph;// pathfinding graph for walkable tiles.

    public JobQueue JobQueue;


	private int _drawMode = 1;
	private string _drawObject;

	void OnEnable(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

		Instance = this;
		_furnitureService = new FurnitureService ();
		_furnitureService.Init ();

		_characterService = new CharacterService ();
		_characterService.Init ();

		_roomService = new RoomService ();
		_roomService.Init ();

		SpriteManager = GetComponent<SpriteManager>();
        SpriteManager.Init();

		JobQueue = new JobQueue ();

		TileManager = GetComponent<TileManager> ();
		FurnitureController = GetComponent<FurnitureController> ();
		CharacterSpriteManager = GetComponent<CharacterSpriteManager> ();
			
		if (!loadGameMode) {
			InitGame ();
		} else {
			loadGameMode = false;
			CreateGameFromSaveFile ();
		}
		GameObject.Find("CameraDolly").transform.position = new Vector3 (TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight/2, -11);
        _characterService.Create(TileDataGrid.GridMap[TileDataGrid.GridWidth/2,TileDataGrid.GridHeight/2]);


    }
	public void InvalidateTileGraph(){
		TileGraph = null;
	}



	void Update(){
		
		foreach (var c in _characterService.FindAll()) {
			c.Update (Time.deltaTime);
		}

		foreach (var f in _furnitureService.FindAll()) {
			f.Update (Time.deltaTime);
		}

	}

	void InitGame(){

		TileDataGrid = new TileDataGrid (10,10,64,64,_furnitureService, _roomService);
		TileManager.InitialiseTileMap(SpriteManager);
		FurnitureController.InitialiseFurniture (SpriteManager ,_furnitureService);
        TileGraph = new PathTileGraph(TileDataGrid);
        CharacterSpriteManager.InitialiseCharacter (SpriteManager, _characterService);

    }

	void CreateGameFromSaveFile(){

		XmlSerializer xmlSerializer = new XmlSerializer (typeof (TileDataGrid));
	
        var xmlReader = XmlReader.Create(new StringReader(PlayerPrefs.GetString("SaveGame00")));
     
        FurnitureController.InitialiseFurniture (SpriteManager,_furnitureService);
       
		TileDataGrid = new TileDataGrid(_furnitureService,_roomService);
        
        while (xmlReader.Read() && xmlReader.IsStartElement())
        {
            if (xmlReader.Name == "TileDataGrid"){
                TileDataGrid.LoadSetup(xmlReader);
            }
            if(xmlReader.Name == "Tiles" )
            {
                TileDataGrid.LoadTiles(xmlReader);
            }
            if (xmlReader.Name == "Furnitures")
            {
                TileDataGrid.LoadFurniture(xmlReader);
            }


        }
       
        xmlReader.Close ();

		TileManager.InitialiseTileMap(SpriteManager);

		TileGraph = new PathTileGraph(TileDataGrid);
		CharacterSpriteManager.InitialiseCharacter (SpriteManager,_characterService);
	}

	public Room GetOutsideRoom(){
		return _roomService.Get ("outside");
	}
	public void DeleteRoom(Room r){
		if (r.Name == "outside") {
			Debug.LogError ("Tried to delete the outside room!");
		}
		_roomService.Delete(r);
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

		PlayerPrefs.SetString ("SaveGame00", writer.ToString());


	}

	void LoadGame(){
		Debug.Log ("Loading...");
		loadGameMode = true;
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);

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
