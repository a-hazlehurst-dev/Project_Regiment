using System;
using System.Collections.Generic;


public class CharacterService
{
	private CharacterRepository _charRepository;
	private CharacterBuilder _characerBuilder;
	private CharacterPrototypes _characterPrototypes;

	public  void Init(){
		_charRepository = new CharacterRepository();
		_characterPrototypes = new CharacterPrototypes (_charRepository, _characterPrototypes);
		_characerBuilder = new CharacterBuilder ();
	}
}

public class CharacterRepository{

	private List<Character> _characters;

	public CharacterRepository(){
		_characters = new List<Character> ();
	}
}

public class CharacterPrototypes{
	private Dictionary<string,Character> _characterPrototypes;

	public CharacterPrototypes(){
		_characterPrototypes = new Dictionary<string, Character> ();
	}
}

public class CharacterBuilder{

	private CharacterRepository _characterRepository;
	private CharacterPrototypes _characterPrototypes;

	public CharacterBuilder(CharacterRepository characterRepository, CharacterPrototypes characterPrototypes){
		_characterPrototypes = characterPrototypes;
		_characterRepository = characterRepository;
	}
}






