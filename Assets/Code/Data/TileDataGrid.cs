﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class TileDataGrid : IXmlSerializable{

	private FurnitureService _furnitureService;
	private RoomService _roomService;
    private CharacterService _characterService;
	public Tile[,] GridMap {get; protected set;}

	//public Dictionary<string, Furniture> FurnitureObjectPrototypes;
	public int GridHeight { get ; protected set; }
	public int GridWidth {get; protected set;}
	public float TileWidth { get; protected set; }
	public float TileHeight { get; protected set; }

    public int treeCount = 0;


	public TileDataGrid(int gridheight, int gridWidth, float tileHeight,float tileWidth, FurnitureService furnitureService, RoomService roomService, CharacterService characterService)
	{
		_furnitureService = furnitureService;
		_roomService = roomService;
        _characterService = characterService;
        CreateGrid (gridWidth, gridheight, tileWidth, tileHeight);	
	}

	public TileDataGrid(FurnitureService furnitureService, RoomService roomService, CharacterService characterService)
    {
        _furnitureService = furnitureService;
		_roomService = roomService;
        _characterService = characterService;
      
    }


	private void CreateGrid(int width, int height, float tileWidth,float tileHeight )
	{
		GridHeight = height;
		GridWidth = width;

		GridMap = new Tile[GridWidth, GridHeight];

		for (int x = 0; x < GridWidth; x++) 
		{
			for (int y = 0; y < GridHeight; y++) 
			{
				GridMap [x, y] = new Tile (x, y,0);
				GridMap [x, y].Room = _roomService.Get("outside");
			}
		}
	}

	public Tile GetTileAt (int x, int y)
	{
        if (x >= GridWidth || x < 0 || y >= GridHeight || y < 0) {
			
			return null;
		}

		return GridMap [x, y];
	}




	/// <summary>
	/// SAVING & LOADING XML STUFF
	/// </summary>

	public TileDataGrid(){
		
	}

	public XmlSchema GetSchema(){
		return null;
	}

	public void WriteXml (XmlWriter writer){
		writer.WriteAttributeString ("Width", GridWidth.ToString());
		writer.WriteAttributeString("Height", GridHeight.ToString());

		writer.WriteStartElement ("Rooms");
		foreach (var room in _roomService.FindRooms()) {
            
            if(room == GameManager.Instance.GetOutsideRoom())
            {
                continue;
            }
			writer.WriteStartElement ("Room");
			room.WriteXml (writer);
			writer.WriteEndElement ();
		}
		writer.WriteEndElement ();

		writer.WriteStartElement ("Tiles");
		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++) {
				writer.WriteStartElement ("Tile");
				GridMap [x, y].WriteXml (writer);
				writer.WriteEndElement ();
                
			}
		}

		writer.WriteEndElement ();

		writer.WriteStartElement ("Furnitures");

		foreach (var furn in _furnitureService.FindAll()) {
			
			writer.WriteStartElement ("Furniture");
			furn.WriteXml (writer);
			writer.WriteEndElement ();
		}

		writer.WriteEndElement ();

        writer.WriteStartElement("Characters");

        foreach (var c in _characterService.FindAll())
        {

            writer.WriteStartElement("Character");
            c.WriteXml(writer);
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
    }

	public void ReadXml (XmlReader reader){
        return;
	}

    public void LoadSetup(XmlReader reader)
    {
        GridWidth = int.Parse(reader.GetAttribute("Width"));
        GridHeight = int.Parse(reader.GetAttribute("Height"));

        CreateGrid(GridWidth, GridHeight, 64, 64);
    }

    public void LoadTiles(XmlReader reader)
    {

		if (reader.ReadToDescendant ("Tile")) {

			do {

				var x = int.Parse (reader.GetAttribute ("X"));
				var y = int.Parse (reader.GetAttribute ("Y"));

				GridMap [x, y].ReadXml (reader);
			} while(reader.ReadToNextSibling ("Tile"));
		}
    }

    public void LoadFurniture(XmlReader reader)
    {
		if (reader.ReadToDescendant ("Furniture")) {

			do {

				var x = int.Parse(reader.GetAttribute("X"));
				var y = int.Parse(reader.GetAttribute("Y"));

				var furn = _furnitureService.CreateFurniture(reader.GetAttribute("objectType"), GridMap[x, y], false);
				furn.ReadXml(reader);
			} while(reader.ReadToNextSibling ("Furniture"));

			//flood fill prior to loading rooms
			//foreach (var furn in _furnitureService.FindAll()) {
			//	Room.DoRoomFloodFill (furn.Tile, true);
			//}

		}
    }

    public void LoadCharacter(XmlReader reader)
    {
        if (reader.ReadToDescendant("Character"))
        {

            do
            {
                var x = int.Parse(reader.GetAttribute("X"));
                var y = int.Parse(reader.GetAttribute("Y"));

                if(_characterService == null) { Debug.Log("Null"); }
                var c = _characterService.Create(GetTileAt(x, y));
                 c.ReadXml(reader);
            } while (reader.ReadToNextSibling("Character"));


        }
    }

    public void LoadRooms(XmlReader reader)
	{
		if (reader.ReadToDescendant ("Room")) {

			do {

			

				Room r = new Room("test");

				_roomService.AddRoom(r);
				//rooms will have same ids due to order by which they were saved, loaded.
				r.ReadXml(reader);
			} while(reader.ReadToNextSibling ("Room"));

		}
	}

	

	

}
