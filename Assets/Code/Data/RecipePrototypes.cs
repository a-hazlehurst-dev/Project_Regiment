using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class RecipePrototypes
{
    private Dictionary<string, Recipe> _recipePrototypes;

	public RecipePrototypes()
    {
		_recipePrototypes = new Dictionary<string, Recipe>();
    }

    public void Init()
    {
		LoadPrototypesFromXml ();
    }


	private void LoadPrototypesFromXml(){
		//loads furn xml data
		var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Data");
		filePath = System.IO.Path.Combine(filePath, "Recipes.xml");
		var recipeXml = File.ReadAllText(filePath);

		var reader = XmlTextReader.Create(new StringReader(recipeXml));

		int recipePrototypesCount = 0;

		if (reader.ReadToDescendant("Recipes"))
		{
			if (reader.ReadToDescendant("Recipe"))
			{
				recipePrototypesCount++;

				var recipe = new Recipe();
				recipe.ReadXmlPrototype(reader);


			}
			else
			{
				Debug.Log("Could not find a Recipe in Recipe.xml");
			}

		}
		else
		{
			Debug.Log("Could not find Recipe in Recipe.xml");
		}

		Debug.Log("Recipe prototypes created "  + recipePrototypesCount);
	}

	public void Add(Recipe recipe){
		_recipePrototypes.Add (recipe.RecipeType, recipe);
	}

	public void Remove(Recipe recipe)
	{
		if (!_recipePrototypes.ContainsKey (recipe.RecipeType)) {
			Debug.Log ("could not remove recipe prtototype " + recipe.RecipeType);
			return;
		}
		_recipePrototypes.Remove (recipe.RecipeType);
	}


}


