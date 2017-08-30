﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Code.Services.Pathfinding;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using MoonSharp.Interpreter;


[MoonSharpUserData]
public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; protected set;}

    public FurnitureService FurnitureService;
    public CharacterService CharacterService;
    public InventoryService InventoryService;
	public RecipeService RecipeService;

	public BaseTileRenderer BaseTileRenderer { get; protected set; }
	public SpriteManager SpriteManager { get; protected set;}
	public TileDataGrid TileDataGrid { get; protected set; }

    public FurnitureSpriteRenderer FurnitureSpriteRenderer { get; protected set; }
    public CharacterSpriteRenderer CharacterSpriteRenderer { get; protected set; }
	public InventorySpriteRenderer InventorySpriteRenderer { get; protected set; }
	
    public GameDrawMode GameDrawMode { get; set; }
	public RoomService RoomService;
    public JobService JobService { get; protected set; }
	private int optionAction;
	private static bool loadGameMode = false;

    public PathTileGraph TileGraph;// pathfinding graph for walkable tiles.

	private int _drawMode = 1;
	private string _drawObject;

	void OnEnable(){
		if (Instance != null)
        {
		}
        
		Instance = this;
		FurnitureService = new FurnitureService ();
		FurnitureService.Init ();

		RecipeService = new RecipeService ();
		RecipeService.Init ();

        JobService = new JobService();
        JobService.Init();

		CharacterService = new CharacterService ();
		CharacterService.Init ();

		RoomService = new RoomService ();
		RoomService.Init ();

		InventoryService = new InventoryService ();
		InventoryService.Init ();

        GameDrawMode = GetComponent<GameDrawMode>();
        InventorySpriteRenderer = GetComponent<InventorySpriteRenderer> ();

		SpriteManager = GetComponent<SpriteManager>();
        SpriteManager.Init();

		BaseTileRenderer = GetComponent<BaseTileRenderer> ();
		FurnitureSpriteRenderer = GetComponent<FurnitureSpriteRenderer> ();
		CharacterSpriteRenderer = GetComponent<CharacterSpriteRenderer> ();

		InventorySpriteRenderer.Init (SpriteManager, InventoryService);


			
		if (!loadGameMode) {
			InitGame ();
            CharacterService.Create(TileDataGrid.GridMap[TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2]);
        } else {
			loadGameMode = false;
			CreateGameFromSaveFile ();
		}
		GameObject.Find("CameraDolly").transform.position = new Vector3 (TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight/2, -11);
        


    }


	public Dictionary<string ,List<Inventory>> GetInventories(){
		return InventoryService._inventories;
	}
	public void InvalidateTileGraph(){
		TileGraph = null;
	}
	public List<Character> GetCharacters(){
		return CharacterService.FindAll ();
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
		if (CharacterService != null) {
			var chars = CharacterService.FindAll ();

			foreach (var c in chars) {
				c.Update (Time.deltaTime);
			}
		}

		foreach (var f in FurnitureService.FindAll()) {
			f.Update (Time.deltaTime);
		}

	}

	void InitGame(){

		TileDataGrid = new TileDataGrid (10,10,64,64,FurnitureService, RoomService, CharacterService);
		BaseTileRenderer.InitialiseTileMap(SpriteManager);
		FurnitureSpriteRenderer.InitialiseFurniture (SpriteManager ,FurnitureService);

        // DEBUGGING REMOVE LATER
        // Create inventory item.
        Inventory inv = new Inventory("metal_ore_copper", 50, 50);

        var tile = TileDataGrid.GetTileAt(TileDataGrid.GridWidth / 2, TileDataGrid.GridHeight / 2 + 1);
        InventoryService.PlaceInventory(tile, inv);

        inv = inv = new Inventory("metal_coal", 50, 8);
        tile = TileDataGrid.GetTileAt(TileDataGrid.GridWidth / 5, TileDataGrid.GridHeight / 2 + 1);
        InventoryService.PlaceInventory(tile, inv);

        //inv = inv = new Inventory("clay", 50, 22);

        tile = TileDataGrid.GetTileAt(TileDataGrid.GridWidth / 2 - 1, TileDataGrid.GridHeight / 2 + 2);
        InventoryService.PlaceInventory(tile, inv);

        TileGraph = new PathTileGraph(TileDataGrid);
        CharacterSpriteRenderer.InitialiseCharacter (SpriteManager, CharacterService);

    }

	void CreateGameFromSaveFile(){

		XmlSerializer xmlSerializer = new XmlSerializer (typeof (TileDataGrid));
	
        var xmlReader = XmlReader.Create(new StringReader(PlayerPrefs.GetString("SaveGame00")));
     
        FurnitureSpriteRenderer.InitialiseFurniture (SpriteManager,FurnitureService);

        CharacterSpriteRenderer.InitialiseCharacter(SpriteManager, CharacterService);

        TileDataGrid = new TileDataGrid(FurnitureService,RoomService, CharacterService);
        
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
            if (xmlReader.Name == "Jobs")
            {
                
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


		BaseTileRenderer.InitialiseTileMap(SpriteManager);

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
		if (TileDataGrid == null) {
			return null;
		}
    
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
