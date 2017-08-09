using System.Collections.Generic;

public class CharacterRepository
{

    private List<Character> _characters;

    public CharacterRepository()
    {
        _characters = new List<Character>();
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
