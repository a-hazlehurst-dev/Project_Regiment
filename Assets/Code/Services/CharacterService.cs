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