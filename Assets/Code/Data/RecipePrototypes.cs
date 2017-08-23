using System.Collections.Generic;
using UnityEngine;

public class RecipePrototypes
{
    private Dictionary<string, Recipe> _recipePrototypes;

	public RecipePrototypes()
    {
		_recipePrototypes = new Dictionary<string, Recipe>();
    }

    public void Init()
    {
		
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


