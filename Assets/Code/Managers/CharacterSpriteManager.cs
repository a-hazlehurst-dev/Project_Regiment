using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteManager : MonoBehaviour {

	Dictionary<Character, GameObject> _characterGameObjectMap;
	SpriteManager _spriteManager;

	Transform characterHolder;
	GameManager Game  { get { return GameManager.Instance; } }

	// Use this for initialization
	void Start () {
		
		characterHolder = new GameObject ("CharacterHolder").transform;
	}

	public void InitialiseCharacter(SpriteManager spriteManager, TileDataGrid tileDataGrid, CharacterManager characterManager)
	{
		_characterGameObjectMap = new Dictionary<Character, GameObject> ();
		_spriteManager = spriteManager;
		characterManager.RegisterCharacterCreated (OnCharacterCreated);
		characterManager.CreateCharacter(tileDataGrid.GetTileAt (tileDataGrid.GridWidth/2, tileDataGrid.GridWidth/2),tileDataGrid);
	}

	public void OnCharacterCreated(Character character){

		Debug.Log ("OnCharacterCreated: was called");
		GameObject char_go = new GameObject ();

		_characterGameObjectMap.Add (character, char_go);

		char_go.name = "Character";
		char_go.transform.position = new Vector3 (character.X, character.Y, 0);
		var sr = char_go.AddComponent<SpriteRenderer> ();
		sr.sprite = _spriteManager.CharacterObjects ["basic_character"];
		sr.sortingLayerName = "Character";
		char_go.transform.SetParent ( characterHolder );

		character.RegisterOnCharacterChangedCallback (OnCharacterChanged);
	}

	public void OnCharacterChanged(Character character){
		if (!_characterGameObjectMap.ContainsKey (character)) {
			Debug.Log ("OnCharacterChanged: cannot find character.");
			return;
		}

		GameObject char_go = _characterGameObjectMap [character];

		//code to change images
		//var sr = char_go.GetComponent<SpriteRenderer> ();
		//sr.sprite = _spriteManager.CharacterObjects ["basic_character"];
		//sr.sortingLayerName = "Character";

		char_go.transform.position = new Vector3 (character.X, character.Y, 0);
		char_go.transform.SetParent ( characterHolder );
	}
}
