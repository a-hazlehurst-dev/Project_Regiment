using System;
using UnityEngine;
using MoonSharp.Interpreter;


[MoonSharpUserData]
public class Inventory {

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


}
