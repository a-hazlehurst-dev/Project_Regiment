using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Job  {

    // this holds info for a queued up job, placing furniture, moving inventory, working at location, maybe fighting.

    public Tile Tile;
    public float TimeToComplete { get; protected set; }


    Action<Job> _cbCJobCompleted;
	Action<Job> _cbJobCancelled;
    Action<Job> _cbJobWorked;

    public bool AcceptsAnyInventoryType = false;

    public Dictionary<string, Inventory> _inventoryRequirements;

	//FIXME:  hard coded a parameter for furniture. Do not like
	public string JobObjectType { get; protected set;}

    public bool CanTakeFromStockpile = true;

	public Job(Tile tile, string jobObjectType, Action<Job> cbJobCompleted, float timeToComplete ,  Inventory[] inventoryRequirements)
	{
		Tile = tile;
		TimeToComplete = timeToComplete;
		_cbCJobCompleted += cbJobCompleted;
      

        JobObjectType = jobObjectType;

        _inventoryRequirements = new Dictionary<string, Inventory>( );
        if (inventoryRequirements != null)
        {
            foreach (var inv in inventoryRequirements)
            {
                _inventoryRequirements[inv.objectType] = inv.Clone();
            }
        }
       
	}

    protected Job(Job other)
    {
        this.Tile = other.Tile;
        this.TimeToComplete = other.TimeToComplete;
        this._cbCJobCompleted += other._cbCJobCompleted;
        this.JobObjectType = other.JobObjectType;

        this._inventoryRequirements = new Dictionary<string, Inventory>();
        if (other._inventoryRequirements != null)
        {
            foreach (var inv in other._inventoryRequirements.Values)
            {
                this._inventoryRequirements[inv.objectType] = inv.Clone();
            }
        }
    }
    virtual public Job Clone()
    {
        return new Job(this);
    }
	public void RegisterJobCompletedCallback(Action<Job> cb){
		_cbCJobCompleted += cb;
	}

	public void RegisterJobCancelledCallback(Action<Job> cb){
		_cbJobCancelled += cb;
	}

    public void RegisterJobWorkedCallback(Action<Job> cb)
    {
        _cbJobWorked += cb;
    }

    public void UnRegisterJobWorkedCallback(Action<Job> cb)
    {
        _cbJobWorked -= cb;
    }

    public void UnRegisterJobCompletedCallback(Action<Job> cb){
		_cbCJobCompleted -= cb;
	}

	public void UnRegisterJobCancelledCallback(Action<Job> cb){
		_cbJobCancelled -= cb;
	}

	public void DoWork(float workTime){

        if(HasAllMaterial() == false)
        {
            //job cant actually be worked, but still call cb, so sprites/animations can be updated.
            if (_cbJobWorked != null)
            {
                _cbJobWorked(this);
            }
           /// Debug.LogError("Tried to do work on a job, were the job has not got all its materials");
            return;
        }

        if (_cbJobWorked != null)
        {
            _cbJobWorked(this);
        }
		TimeToComplete -= workTime;
		if (TimeToComplete <=0) {
			if (_cbCJobCompleted != null) {
				_cbCJobCompleted(this);
			}
		}
	}
	public void CancelJob(){
		if (_cbJobCancelled != null) 
			_cbJobCancelled(this);

        GameManager.Instance.JobQueue.Remove(this);
	}

	public bool HasAllMaterial(){
		foreach (var inv in _inventoryRequirements.Values) {
			if (inv.maxStackSize > inv.StackSize) {
				return false;
			}
		}
		return true;
	}

	public int DesireInventoryType(Inventory inv){
        if (AcceptsAnyInventoryType)
        {
            return inv.maxStackSize;
        }
		if (_inventoryRequirements.ContainsKey (inv.objectType) == false) {
			return 0;// we dont want this.
		}

		if (_inventoryRequirements [inv.objectType].StackSize >= _inventoryRequirements [inv.objectType].maxStackSize) {
			//already have enough of this material.
			return  0;
		}

        return _inventoryRequirements[inv.objectType].maxStackSize - _inventoryRequirements[inv.objectType].StackSize; //we need this stuff.
	}

	public Inventory GetFirstDesiredInventory(){
		return _inventoryRequirements.Values.FirstOrDefault (x => x.maxStackSize > x.StackSize); 
	}
}

