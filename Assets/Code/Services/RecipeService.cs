using System;

public class RecipeService
	{
		public RecipePrototypes RecipePrototypes  { get; set;}


		public RecipeService ()
		{
			RecipePrototypes = new RecipePrototypes ();

		}

		public void Init(){
			RecipePrototypes.Init ();
		}


}

