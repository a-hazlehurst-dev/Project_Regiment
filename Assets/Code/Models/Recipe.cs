using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Recipe 
{

    public string RecipeType { get; set; }
    public string DisplayName { get; set; }
    public List<string> ApplicableFurnitures { get; set; }
    public List<MaterialRequirement> MaterialRequirements { get; set; }
    public List<RequiredSkill> RequiredSkills { get; set; }
    public List<MaterialOutput> MaterialOutputs { get; set; }

    public Recipe()
    {
        ApplicableFurnitures = new List<string>();
        MaterialRequirements = new List<MaterialRequirement>();
        MaterialOutputs = new List<MaterialOutput>();
        RequiredSkills = new List<RequiredSkill>();
    }

    public float BaseTimeToComplete { get; set; }

    public void ReadXmlPrototype(XmlReader reader)
    {

        RecipeType = reader.GetAttribute("Type");

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
                    ApplicableFurnitures = GetApplicableFurnitures(recipeTag);
                    break;
                case "Requirements":
                    MaterialRequirements = GetRequirements(recipeTag);
                    break;
                case "RequiredSkills":
                    RequiredSkills = GetRequiredSkills(recipeTag);
                    break;
                case "Outputs":
                    MaterialOutputs = GetMaterialOutputs(recipeTag);
                    break;
                case "BaseCompletionTime":
                    recipeTag.Read();
                    BaseTimeToComplete = reader.ReadContentAsFloat();
                    break;
            }
        }
    }

    private List<string> GetApplicableFurnitures(XmlReader xmlrReader)
    {

        var reader = xmlrReader.ReadSubtree();

        var list = new List<string>();
        while (reader.Read())
        {
            if (reader.Name == "ApplicableFurniture" && reader.IsStartElement())
            {
                reader.Read();
                var x = reader.ReadContentAsString();
                list.Add(x);
            }
        }

        return list;
    }
    private List<MaterialRequirement> GetRequirements(XmlReader xmlrReader)
    {


        var reader = xmlrReader.ReadSubtree();

        var list = new List<MaterialRequirement>();
        while (reader.Read())
        {
            if (reader.Name == "Requirement" && reader.IsStartElement())
            {
                var quantity = int.Parse(reader.GetAttribute("qty"));
                var type = reader.GetAttribute("type");
                var specific = reader.GetAttribute("specific");

                var mat = new MaterialRequirement { Quantity = quantity, Type = type, Specific = specific };

                list.Add(mat);
            }
        }
        return list;

    }
    private List<RequiredSkill> GetRequiredSkills(XmlReader xmlrReader)
    {



        var reader = xmlrReader.ReadSubtree();

        var list = new List<RequiredSkill>();
        while (reader.Read())
        {
            if (reader.Name == "Skill" && reader.IsStartElement())
            {
                var type = reader.GetAttribute("type");
                var value = float.Parse(reader.GetAttribute("value"));

                var mat = new RequiredSkill { Type = type, Value = value };

                list.Add(mat);
            }
        }
        return list;

    }
    private List<MaterialOutput> GetMaterialOutputs(XmlReader xmlrReader)
    {

        Debug.Log("reading Material Output");
        var reader = xmlrReader.ReadSubtree();

        var list = new List<MaterialOutput>();
        while (reader.Read())
        {
            if (reader.Name == "Output" && reader.IsStartElement())
            {
                var type = reader.GetAttribute("type");
                var quanitity = int.Parse(reader.GetAttribute("qty"));

                var mat = new MaterialOutput { Type = type, Quantity = quanitity };

                list.Add(mat);
            }
        }
        return list;

    }

}
public class MaterialRequirement
{
	public string Type { get; set; } //the name of the item we want
	public string Specific {get;set;}
	public int Quantity { get; set; } // the amount of the item we need to make 1 unit of product.
}


public class RequiredSkill{

	public string Type { get;set;}
	public float Value {get;set;}
}

public class MaterialOutput
{
	public string Type {get;set;} //name of the item that will be produced;
	public int Quantity {get;set;} // basic number of items createed.
}