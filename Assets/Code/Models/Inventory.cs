﻿using System;
using UnityEngine;
using MoonSharp.Interpreter;
using System.Collections.Generic;

[MoonSharpUserData]
public class Inventory: ISelectableItem

{

    public string objectType = "clay";
    public int maxStackSize = 50;
    public int StackSize
    {
        get { return _stackSize; }
        set
        {  if (_stackSize != value)
            {
                _stackSize = value;
                if(cbInventoryChanged != null)
                {
                    cbInventoryChanged(this);
                }
              
   
            }
        }
    }

    private Action<Inventory> cbInventoryChanged;

    public Tile Tile;
    public Character Character;
    private int _stackSize = 1;

    public Inventory() {

    }
    public Inventory(string objectType, int maxStackSize, int currentStackSize  )
    {
        this.objectType = objectType;
        this.maxStackSize = maxStackSize;
        this.StackSize = currentStackSize;
    }

	protected Inventory(Inventory other){
		objectType 		= other.objectType;
		maxStackSize 	= other.maxStackSize;
		StackSize 		= other.StackSize;
	}

	public virtual Inventory Clone(){
		return new Inventory (this);
	}

    public void RegisterOnInventoryChangedCallback(Action<Inventory> cb)
    {

        cbInventoryChanged += cb;
    }

    public void UnRegisterOnInventoryChangedCallback(Action<Inventory> cb)
    {
        cbInventoryChanged -= cb;
    }

    public string Getname()
    {
        return objectType;
    }

    public string GetDescription()
    {
        return "This is an inventory";
    }

    public string GetHitPointsToString()
    {
        return "1/1";
    }

    public List<Recipe> Recipes()
    {
        return null;
    }

    public List<string> Buttons()
    {
        return new List<string>();
    }
}
