public class CharacterBuilder
{

    private CharacterRepository _characterRepository;
    private CharacterPrototypes _characterPrototypes;

    public CharacterBuilder(CharacterRepository characterRepository, CharacterPrototypes characterPrototypes)
    {
        _characterPrototypes = characterPrototypes;
        _characterRepository = characterRepository;
    }

    public Character Create(Tile tile)
    {
        return new Character(tile);
    }
}

