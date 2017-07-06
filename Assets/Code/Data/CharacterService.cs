using System;
using System.Collections.Generic;


public class CharacterService
{
	private CharacterRepository _charRepository;
	private CharacterBuilder _characterBuilder;
	private CharacterPrototypes _characterPrototypes;

    private Action<Character> cbOnCharacterCreated;

	public  void Init(){
		_charRepository = new CharacterRepository();
		_characterPrototypes = new CharacterPrototypes ();
        _characterBuilder = new CharacterBuilder (_charRepository, _characterPrototypes);
	}
    public Character Create(Tile tile)
    {
        var character = _characterBuilder.Create(tile);
        _charRepository.Add(character);
        if (cbOnCharacterCreated != null)
        {
            cbOnCharacterCreated(character);
        }

        return character;
    }

    public List<Character> FindAll()
    {

        return _charRepository.FindAll();
    }

    public void Register_OnCharacter_Created(Action<Character> cbOnCreated)
    {
        cbOnCharacterCreated += cbOnCreated;
    }
    public void UnRegister_OnCharacter_Created(Action<Character> cbOnCreated)
    {
        cbOnCharacterCreated -= cbOnCreated;
    }

}

public class CharacterRepository{

	private List<Character> _characters;

	public CharacterRepository(){
		_characters = new List<Character> ();
	}

    public void Add(Character character)
    {
        _characters.Add(character);
    }

    public List<Character> FindAll()
    {
        return _characters;
    }
}

public class CharacterPrototypes{
	private Dictionary<string,Character> _characterPrototypes;

	public CharacterPrototypes(){
		_characterPrototypes = new Dictionary<string, Character> ();
	}

    public void Initialise()
    {

    }
}

public class CharacterBuilder{

	private CharacterRepository _characterRepository;
	private CharacterPrototypes _characterPrototypes;

	public CharacterBuilder(CharacterRepository characterRepository, CharacterPrototypes characterPrototypes){
		_characterPrototypes = characterPrototypes;
		_characterRepository = characterRepository;
	}

    public Character Create(Tile tile)
    {
        return new Character(tile);
    }
}






