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
	public InventorySpriteController InventorySpriteController { get; protected set; }
	public FurnitureService _furnitureService;
	private CharacterService _characterService;
	public InventoryService _inventoryService;
    public GameDrawMode GameDrawMode { get; set; }
	public RoomService RoomService;
    public JobService JobService { get; protected set; }
	private int optionAction;
	private static bool loadGameMode = false;

    

    public PathTileGraph TileGraph;// pathfinding graph for walkable tiles.

    //public JobQueue JobQueue;


	private int _drawMode = 1;
	private string _drawObject;

	void OnEnable(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

        
		Instance = this;
		_furnitureService = new FurnitureService ();
		_furnitureService.Init ();

        JobService = new JobService();
        JobService.Init();

		_characterService = new CharacterService ();
		_characterService.Init ();

		RoomService = new RoomService ();
		RoomService.Init ();

		_inventoryService = new InventoryService ();
		_inventoryService.Init ();

        GameDrawMode = GetComponent<GameDrawMode>();
        InventorySpriteController = GetComponent<InventorySpriteController> ();


		SpriteManager = GetComponent<SpriteManager>();
        SpriteManager.Init();

		//JobQueue = new JobQueue ();

		TileManager = GetComponent<TileManager> ();
		FurnitureController = GetComponent<FurnitureController> ();
		CharacterSpriteManager = GetComponent<CharacterSpriteManager> ();

		InventorySpriteController.Init (SpriteManager, _inventoryService);
			
		if (!loadGameMode) {
			InitGame ();
            _characterService.Create(TileDataGrid.GridMap[TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2]);
        } else {
			loadGameMode = false;
			CreateGameFromSaveFile ();
		}
		GameObject.Find("CameraDolly").transform.position = new Vector3 (TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight/2, -11);
        


    }


	public Dictionary<string ,List<Inventory>> GetInventories(){
		return _inventoryService._inventories;
	}
	public void InvalidateTileGraph(){
		TileGraph = null;
	}
	public List<Character> GetCharacters(){
		return _characterService.FindAll ();
	}
    public void AddRoom(Room rm)
    {

        RoomService.AddRoom(rm);
    }
    public Room GetOutsideRoom()
    {
        return RoomService.Get("outside");
    }

	public List<Room>FindRooms(){
		return RoomService.FindRooms ();
	}
    public void DeleteRoom(Room r)
    {
        Debug.Log("GameManager delete");
        if (r.Name == "outside")
        {
            Debug.LogError("Tried to delete the outside room!");
        }
        RoomService.Delete(r);
    }

    void Update(){
        var chars = _characterService.FindAll();

        foreach (var c in chars) {
			c.Update (Time.deltaTime);
		}

		foreach (var f in _furnitureService.FindAll()) {
			f.Update (Time.deltaTime);
		}

	}

	void InitGame(){

		TileDataGrid = new TileDataGrid (10,10,64,64,_furnitureService, RoomService, _characterService);
		TileManager.InitialiseTileMap(SpriteManager);
		FurnitureController.InitialiseFurniture (SpriteManager ,_furnitureService);

        // DEBUGGING REMOVE LATER
        // Create inventory item.
        //Inventory inv = new Inventory("clay", 50, 50);

        //var tile = TileDataGrid.GetTileAt(TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2 + 1);
        //_inventoryService.PlaceInventory(tile, inv);


        //inv = inv = new Inventory("clay", 50, 8);
        //tile = TileDataGrid.GetTileAt(TileDataGrid.GridWidth / 5, TileDataGrid.GridHeight / 2 + 1);
        //_inventoryService.PlaceInventory(tile, inv);


        //inv = inv = new Inventory("clay", 50, 22);

        //tile = TileDataGrid.GetTileAt(TileDataGrid.GridWidth / 2 - 1, TileDataGrid.GridHeight / 2 + 2);
        //_inventoryService.PlaceInventory(tile, inv);


        TileGraph = new PathTileGraph(TileDataGrid);
        CharacterSpriteManager.InitialiseCharacter (SpriteManager, _characterService);

    }

	void CreateGameFromSaveFile(){

		XmlSerializer xmlSerializer = new XmlSerializer (typeof (TileDataGrid));
	
        var xmlReader = XmlReader.Create(new StringReader(PlayerPrefs.GetString("SaveGame00")));
     
        FurnitureController.InitialiseFurniture (SpriteManager,_furnitureService);

        CharacterSpriteManager.InitialiseCharacter(SpriteManager, _characterService);

        TileDataGrid = new TileDataGrid(_furnitureService,RoomService, _characterService);
        
        while (xmlReader.Read() && xmlReader.IsStartElement())
        {

            if (xmlReader.Name == "TileDataGrid"){

                TileDataGrid.LoadSetup(xmlReader);
            }
			if (xmlReader.Name == "Rooms") {
			
				TileDataGrid.LoadRooms(xmlReader);
			
			}
            if(xmlReader.Name == "Tiles" )
            {
			
                TileDataGrid.LoadTiles(xmlReader);
            }
            if (xmlReader.Name == "Furnitures")
            {
		
                TileDataGrid.LoadFurniture(xmlReader);
            }
            if (xmlReader.Name == "Characters")
            {
       
                TileDataGrid.LoadCharacter(xmlReader);
            }

       }
       
        xmlReader.Close ();

		// DEBUGGING REMOVE LATER
		// Create inventory item.
		//Inventory inv = new Inventory("clay", 50, 50);
    
		//var tile = TileDataGrid.GetTileAt (TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2+1);
		//_inventoryService.PlaceInventory (tile,inv) ;
		

		//inv = inv = new Inventory("clay", 50,8);
  // 		tile = TileDataGrid.GetTileAt (TileDataGrid.GridWidth / 5, TileDataGrid.GridHeight / 2+1);
		//_inventoryService.PlaceInventory (tile,inv) ;
	

		//inv = inv = new Inventory("clay", 50, 22);
       
  //      tile = TileDataGrid.GetTileAt (TileDataGrid.GridWidth / 2-1, TileDataGrid.GridHeight / 2+2);
		//_inventoryService.PlaceInventory (tile,inv) ;


		TileManager.InitialiseTileMap(SpriteManager);

		TileGraph = new PathTileGraph(TileDataGrid);
		
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

		loadGameMode = true;
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);

	}


    public Tile GetTileAt(Vector3 coordinate)
    {
        int x = Mathf.FloorToInt(coordinate.x + .5f);
        int y = Mathf.FloorToInt(coordinate.y + .5f);
    
		return TileDataGrid.GetTileAt(x, y);
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
