using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GridManager  gridManager;
    public SpriteManager spriteManager;


	void Awake(){
        spriteManager = GetComponent<SpriteManager>();
        gridManager = GetComponent<GridManager> ();
		InitGame();
	}

	void InitGame(){
		gridManager.GridSetup (spriteManager);
	}

	void Update () {
		
	}
}
