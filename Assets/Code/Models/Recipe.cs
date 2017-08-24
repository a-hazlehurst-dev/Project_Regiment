using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Recipe : MonoBehaviour {


	public string RecipeType { get; set; }
	public string DisplayName { get; set; }
	public List<string> ApplicableFurnitures { get; set; }
	public List<MaterialRequirement> MaterialRequirements {get;set;}
	public List<RequiredSkill> RequiredSkills { get; set; }
	public List<MaterialOutput> MaterialOutputs { get; set; }


	public float BaseTimeToComplete { get; set; }

	public Recipe(){
		ApplicableFurnitures = new List<string> ();
		MaterialRequirements = new List<MaterialRequirement> ();
		MaterialOutputs = new List<MaterialOutput> ();
		RequiredSkills = new List<RequiredSkill> ();
	}


	public void ReadXmlPrototype (XmlReader reader){
		Debug.Log("ReadXML Prototypes: Recipes");
		RecipeType = reader.GetAttribute ("Type");

		var recipeTag = reader.ReadSubtree();

		while (recipeTag.Read())
		{
			switch (recipeTag.Name)
			{
			case "Name":
				recipeTag.Read();
				DisplayName = reader.ReadContentAsString();
				break;
			case "ApplicableFurnitures":
				ApplicableFurnitures = GetApplicableFurnitures (recipeTag);
				break;
			case "Requirements":
				MaterialRequirements = GetRequirements (recipeTag);
				break;
			case "RequiredSkills":
				RequiredSkills = GetRequiredSkills (recipeTag);
				break;
			case "Outputs":
				MaterialOutput = GetMaterialOutputs (recipeTag);
				break;
			case "BaseCompletionTime":
				recipeTag.Read();
				BaseTimeToComplete = reader.ReadContentAsFloat(recipeTag);
				break;
			}
		}
	}

	private List<string> GetApplicableFurnitures(XmlReader xmlrReader){
		return new List<string> ();
	}

	private List<MaterialRequirement> GetRequirements(XmlReader xmlrReader){
		return new List<string> ();
	}
	private List<RequiredSkill> GetRequiredSkills(XmlReader xmlrReader){
		return new List<RequiredSkill> ();
	}
	private List<MaterialOutput> GetMaterialOutputs(XmlReader xmlrReader){
		return new List<MaterialOutput> ();
	}
	

}

public class MaterialRequirement
{
	public string ObjectType { get; set; } //the name of the item we want
	public int Quantity { get; set; } // the amount of the item we need to make 1 unit of product.
}


public class RequiredSkill{

	public string SkillType { get;set;}
	public float Value {get;set;}
}

public class MaterialOutput
{
	public string ObjectType {get;set;} //name of the item that will be produced;
	public int Quantity {get;set;} // basic number of items createed.
}