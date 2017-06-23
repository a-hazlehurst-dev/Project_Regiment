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
		_characterGameObjectMap = new Dictionary<Character, GameObject> ();
		characterHolder= new GameObject ("CharacterHolder").transform;

		Game.RegisterCharacterCreated (OnCharacterCreated);

		Game.CreateCharacter(Game.GetTileAt (new Vector3(Game.TileDataGrid.GridWidth/2, Game.TileDataGrid.GridWidth/2, 0)));

	}


	public void InitialiseCharacter(SpriteManager spriteManager)
	{
		_spriteManager = spriteManager;
	}

	public void OnCharacterCreated(Character character){

		Debug.Log ("OnCharacterCreated: was called");
		GameObject char_go = new GameObject ();

		_characterGameObjectMap.Add (character, char_go);

		char_go.name = "Character";
		char_go.transform.position = new Vector3 (character.CurrentTile.X, character.CurrentTile.Y, 0);
		var sr = char_go.AddComponent<SpriteRenderer> ();
		sr.sprite = _spriteManager.CharacterObjects ["basic_character"];
		sr.sortingLayerName = "active";
		char_go.transform.SetParent ( characterHolder );
	}
}
