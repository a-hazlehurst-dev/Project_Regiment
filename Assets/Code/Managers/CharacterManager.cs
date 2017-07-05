using System;
using System.Collections.Generic;


	public class CharacterManager
	{
		public List<Character> Characters {get;set;}

		Action<Character> cbCharacterCreated;

		public CharacterManager (){
			Characters = new List<Character> ();
		}


	public void CreateCharacter(Tile t, TileDataGrid tileDataGrid){
		Character c = new Character (tileDataGrid.GridMap [tileDataGrid.GridWidth / 2, tileDataGrid.GridHeight / 2]);
		if (cbCharacterCreated != null) {
			cbCharacterCreated (c);
		}
		Characters.Add (c);
	}

	public void Update(float deltaTime){
		foreach (var c in Characters) {
			c.Update (deltaTime);
		}
	}
	public void RegisterCharacterCreated(Action<Character> callBackFunction){
		cbCharacterCreated += callBackFunction;
	}

	public void UnRegisterCharacterCreated(Action<Character> callBackFunction){
		cbCharacterCreated -= callBackFunction;
	}
}



