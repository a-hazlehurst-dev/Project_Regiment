using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuViewModel : MonoBehaviour {

	public GameObject btnNewGame;
	public GameObject btnSaveGame;
	public GameObject btnLoadGame;
	private GameManager Game { get { return GameManager.Instance; } }


	public void OnNewGame_Click(){
		Game.SetGameOptions (1);
	}

	public void OnSaveGame_Click(){
		Game.SetGameOptions (2);
		
	}

	public void OnLoadGame_Click(){
		Game.SetGameOptions (3);
		
	}
}
