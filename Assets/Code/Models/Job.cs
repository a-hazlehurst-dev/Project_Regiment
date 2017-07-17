using System;
using System.Collections.Generic;
using System.Linq;

public class Job  {

    // this holds info for a queued up job, placing furniture, moving inventory, working at location, maybe fighting.

    public Tile Tile;
    float _timeToComplete = .2f;


    Action<Job> _cbCJobCompleted;
	Action<Job> _cbJobCancelled;

    public Dictionary<string, Inventory> _inventoryRequirements;

	//FIXME:  hard coded a parameter for furniture. Do not like
	public string JobObjectType { get; protected set;}

	public Job(Tile tile, string jobObjectType, Action<Job> cbJobCompleted, float timeToComplete ,  Inventory[] inventoryRequirements)
	{
		Tile = tile;
		_timeToComplete = timeToComplete;
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
        this._timeToComplete = other._timeToComplete;
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

	public void UnRegisterJobCompletedCallback(Action<Job> cb){
		_cbCJobCompleted -= cb;
	}

	public void UnRegisterJobCancelledCallback(Action<Job> cb){
		_cbJobCancelled -= cb;
	}

	public void DoWork(float workTime){
		_timeToComplete -= workTime;
		if (_timeToComplete <=0) {
			if (_cbCJobCompleted != null) {
				_cbCJobCompleted(this);
			}
		}
	}
	public void CancelJob(){
		if (_cbJobCancelled != null) 
			_cbJobCancelled(this);
	}

	public bool HasAllMaterial(){
		foreach (var inv in _inventoryRequirements.Values) {
			if (inv.maxStackSize > inv.stackSize) {
				return false;
			}
		}
		return true;
	}

	public bool DesireInventoryType(Inventory inv){
		if (_inventoryRequirements.ContainsKey (inv.objectType) == false) {
			return false;// we dont want this.
		}

		if (_inventoryRequirements [inv.objectType].stackSize >= _inventoryRequirements [inv.objectType].maxStackSize) {
			//already have enough of this material.
			return  false;
		}

		return true; //we need this stuff.
	}

	public Inventory GetFirstDesiredInventory(){
		return _inventoryRequirements.Values.FirstOrDefault (x => x.maxStackSize > x.stackSize); 
	}
}

