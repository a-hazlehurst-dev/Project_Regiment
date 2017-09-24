using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pnlSelectionManagerScript : MonoBehaviour {

    public Dropdown dropdown;
    public Text Summary;
    public Furniture furn;
    public List<Recipe> Recipes;

    public Recipe SelectedRecipe;
    private List<Recipe> recipes;

    private JobOrder _order;

    
	// Use this for initialization
	void Start () {

         recipes = GameManager.Instance.RecipeService.RecipePrototypes.GetAll();

        List<Dropdown.OptionData> data = new List<Dropdown.OptionData>();
        data.Add(new Dropdown.OptionData("None"));  
       
        foreach(var recipe in recipes)
        {
            if (recipe.ApplicableFurnitures.Contains("FURN_SMELTER"))
            {
                data.Add(new Dropdown.OptionData(recipe.DisplayName));
            }
        }

        dropdown.AddOptions(data);
        dropdown.onValueChanged.AddListener(delegate { OnDropDownSelected(dropdown); });
        _order = new JobOrder();
	}

    public void OnDropDownSelected(Dropdown target)
    {
        Debug.Log(target.value);
        _order.ItemOrdered = recipe.ApplicableFurnitures[target.value - 1];


        //we've selected the type of item we want to produce
    }

	
	// Update is called once per frame
	void Update () {
	}

    public void IncrementOrderQuantity(int count)
    {
        _order.Quantity += count;
        Summary.text = _order.Summary;
    }
    public void DecrementOrderQuantity (int count)
    {
        _order.Quantity -= count;
        Summary.text = _order.Summary;
    }

    
}


public class JobOrder
{
    public int Quantity { get; set; }
    public string ItemOrdered { get; set; }
    public int IsConfirmed { get; set; }
    public string Summary { get; set; }

    public void UpdateSummary()
    {
        Summary = "Ordering " + Quantity + " of " + ItemOrdered;
    }

}