using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteRenderer : MonoBehaviour {

	Dictionary<Character, GameObject> _characterGameObjectMap;
	SpriteManager _spriteManager;
    CharacterService _characterService;

	Transform characterHolder;

	// Use this for initialization
	void Start () {
		
	}


	public void InitialiseCharacter(SpriteManager spriteManager, CharacterService characterService)
	{
		_spriteManager = spriteManager;

        _characterService = characterService;

        _characterGameObjectMap = new Dictionary<Character, GameObject>();

        characterHolder = new GameObject("CharacterHolder").transform;

        _characterService.Register_OnCharacter_Created(OnCharacterCreated);
    }

	public void OnCharacterCreated(Character character){


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

		char_go.transform.position = new Vector3 (character.X, character.Y, 0);
		char_go.transform.SetParent ( characterHolder );
	}
}
