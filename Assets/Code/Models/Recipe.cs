using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour {


	public string RecipeType { get; set; }
	public List<string> ApplicableFurniture{ get; set; }
	public List<MaterialRequirement> MaterialRequirent {get;set;}
	public List<SkillLevelRequirement> SkillLevelRequirement { get; set; }
	public List<RecipeOutput> MaterialOutput { get; set; }


	public float BaseTimeToComplete { get; set; }

	public Recipe(){
		ApplicableFurniture = new List<string> ();
		MaterialRequirent = new List<MaterialRequirement> ();
		SkillLevelRequirement = new List<RecipeOutput> ();
		SkillLevelRequirement = new List<SkillLevelRequirement> ();
	}

	

}

public class MaterialRequirement
{
	public string ObjectType { get; set; } //the name of the item we want
	public int Quantity { get; set; } // the amount of the item we need to make 1 unit of product.
}


public class SkillLevelRequirement{

	public string SkillType { get;set;}
	public float Value {get;set;}
}

public class RecipeOutput
{
	public string ObjectType {get;set;} //name of the item that will be produced;
	public int Quantity {get;set;} // basic number of items createed.
}