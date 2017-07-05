using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Code.Services.Pathfinding;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;

public class XmlGame 
{

	public GameManager Game{ get { return GameManager.Instance; } }

	private int optionAction;
	public static bool loadGameMode = false;
   

	public TileDataGrid CreateGameFromSaveFile(SpriteManager spriteManager, TileDataGrid tileDataGrid){

		XmlSerializer xmlSerializer = new XmlSerializer (typeof (TileDataGrid));
		TextReader reader = new StringReader (PlayerPrefs.GetString("SaveGame00"));
		Game.FurnitureManager.InitialiseFurniture (spriteManager);

		tileDataGrid =(TileDataGrid)xmlSerializer.Deserialize (reader);

		reader.Close ();
		return tileDataGrid;
	}

	public void NewGame(){
		Debug.Log ("Restarting....");
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void SaveGame(TileDataGrid tileDataGrid){
		Debug.Log ("Saving....");

		XmlSerializer xmlSerializer = new XmlSerializer (typeof(TileDataGrid));
		TextWriter writer = new StringWriter ();
		xmlSerializer.Serialize (writer, tileDataGrid);

		Debug.Log (writer.ToString ());

		PlayerPrefs.SetString ("SaveGame00", writer.ToString ());
	}

	public void LoadGame(){
		Debug.Log ("Loading...");
		loadGameMode = true;
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);

	}



}
