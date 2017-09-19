using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;


[MoonSharpUserData]
public class Job
{

    // this holds info for a queued up job, placing furniture, moving inventory, working at location, maybe fighting.

    public Tile Tile;

    public string JobType;
    public string Name;
    public string FurnitureType;
    public bool CanRepeat; 
   
    public List<JobTask> JobTasks { get; set; }
	public Dictionary<string, Inventory> _inventoryRequirements;
    public float TimeToComplete { get; protected set; }
    public bool CanTakeFromStockpile = true;
    //FIXME:  hard coded a parameter for furniture. Do not like
    public string JobObjectType { get; protected set; }
    protected float _jobTimeRequired;
    protected bool _jobRepeats = false;
    public bool AcceptsAnyInventoryType = false;
    public Furniture FurniturePrototype;
    public Furniture furnitureToOperate; // peice of furn that owns the h

    Action<Job> _cbCJobCompleted; // job was completed, shouldnwo build item or whatever
    List<string> _cbLuaJobCompleted;
    Action<Job> _cbJobStopped; // job was stopped, maybe non repeating or was cancelled.
    Action<Job> _cbJobWorked; // gets called each time work was performed, update ui?
    List<string> _cbLuaJobWorked;

    #region constructors

    public Job(Tile tile, string jobObjectType, Action<Job> cbJobCompleted, float timeToComplete,
        Inventory[] inventoryRequirements, bool jobRepeats = false)
    {
        Tile = tile;
        TimeToComplete = timeToComplete;
        _cbCJobCompleted += cbJobCompleted;
        _jobTimeRequired = this.TimeToComplete = timeToComplete;
        _jobRepeats = jobRepeats;

        JobObjectType = jobObjectType;
        _cbLuaJobWorked = new List<string>();
        _cbLuaJobCompleted = new List<string>();
        JobTasks = new List<JobTask>();

        _inventoryRequirements = new Dictionary<string, Inventory>();
        if (inventoryRequirements != null)
        {
            foreach (var inv in inventoryRequirements)
            {
                _inventoryRequirements[inv.objectType] = inv.Clone();
            }
        }

    }

    public Job()
    {
        _cbLuaJobWorked = new List<string>();
        _cbLuaJobCompleted = new List<string>();
        _inventoryRequirements = new Dictionary<string, Inventory>();
        JobTasks = new List<JobTask>();
    }



    protected Job(Job other)
    {
        this.Tile = other.Tile;
        this.TimeToComplete = other.TimeToComplete;
        this._cbCJobCompleted += other._cbCJobCompleted;
        this.JobObjectType = other.JobObjectType;
        this.JobTasks = other.JobTasks;

        _cbLuaJobWorked = new List<string>(other._cbLuaJobWorked);
        _cbLuaJobCompleted = new List<string>(other._cbLuaJobCompleted);

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


    #endregion

    #region CallBacks

    public void Register_JobCompleted_Callback(Action<Job> cb)
    {
        _cbCJobCompleted += cb;
    }

    public void UnRegister_JobCompleted_Callback(Action<Job> cb)
    {
        _cbCJobCompleted -= cb;
    }

    public void Register_JobCompleted_Callback(string cb)
    {
        _cbLuaJobCompleted.Add(cb);
    }

    public void UnRegister_JobCompleted_Callback(string cb)
    {
        _cbLuaJobCompleted.Remove(cb);
    }

    public void Register_JobStopped_Callback(Action<Job> cb)
    {
        _cbJobStopped += cb;
    }

    public void Register_JobWorked_Callback(Action<Job> cb)
    {
        _cbJobWorked += cb;
    }

    public void UnRegister_JobWorked_Callback(Action<Job> cb)
    {
        _cbJobWorked -= cb;
    }



    public void Register_JobWorked_Callback(string cb)
    {
        _cbLuaJobWorked.Add(cb);
    }

    public void UnRegister_JobWorked_Callback(string cb)
    {
        _cbLuaJobWorked.Add(cb);
    }




    public void UnRegister_JobStopped_Callback(Action<Job> cb)
    {
        _cbJobStopped -= cb;
    }

    #endregion

    #region JobActions

    public void DoWork(float workTime)
    {

        if (HasAllMaterial() == false)
        {
            //job cant actually be worked, but still call cb, so sprites/animations can be updated.
            if (_cbJobWorked != null)
            {
                _cbJobWorked(this);
            }

            if (_cbLuaJobWorked != null)
            {
                foreach (var luaFunction in _cbLuaJobWorked)
                {
                    Debug.Log(this.Tile.inventory);

                    FurnitureActions.CallFunction(luaFunction, this);
                }
            }
            /// Debug.LogError("Tried to do work on a job, were the job has not got all its materials");
            return;
        }

        TimeToComplete -= workTime;

        if (_cbJobWorked != null)
        {
            _cbJobWorked(this);
        }

        if (_cbLuaJobWorked != null)
        {
            foreach (var luaFunction in _cbLuaJobWorked)
            {
                Debug.Log("JobWorked: " + luaFunction);
                FurnitureActions.CallFunction(luaFunction, this);
            }
        }

        if (TimeToComplete <= 0)
        {
            //do what ever is needed when job cycle completes
            if (_cbCJobCompleted != null)
            {
                _cbCJobCompleted(this);
            }
            foreach (string luaFunc in _cbLuaJobCompleted)
            {
                if (luaFunc != null)
                {
                    FurnitureActions.CallFunction(luaFunc, this);
                }
            }


            if (_jobRepeats == false)
            {
                //let all know the job has been officially concluded
                if (_cbJobStopped != null)
                {
                    _cbJobStopped(this);
                }
            }
            else
            {
                //repeating job and must be reset.s
                TimeToComplete += _jobTimeRequired;
            }

        }
    }

    public void CancelJob()
    {
        if (_cbJobStopped != null)
            _cbJobStopped(this);

        GameManager.Instance.JobService.Remove(this);

    }

    #endregion

    #region JobRequirements

    public bool HasAllMaterial()
    {
        foreach (var inv in _inventoryRequirements.Values)
        {
            if (inv.maxStackSize > inv.StackSize)
            {
                return false;
            }
        }
        return true;
    }

    public int DesireInventoryType(Inventory inv)
    {
        if (AcceptsAnyInventoryType)
        {
            return inv.maxStackSize;
        }
        if (_inventoryRequirements.ContainsKey(inv.objectType) == false)
        {
            return 0; // we dont want this.
        }

        if (_inventoryRequirements[inv.objectType].StackSize >= _inventoryRequirements[inv.objectType].maxStackSize)
        {
            //already have enough of this material.
            return 0;
        }

        return _inventoryRequirements[inv.objectType].maxStackSize - _inventoryRequirements[inv.objectType].StackSize;
            //we need this stuff.
    }

    public Inventory GetFirstDesiredInventory()
    {
        //FIXME: cant always just return the first one, as their may not be enough material to collect the first type.
        //TODO: probably need to check see what items are available to drop off.
        //Maybe this should be changed so that an unstored item instead creates a job and says store me.

        return _inventoryRequirements.Values.FirstOrDefault(x => x.maxStackSize > x.StackSize);
    }

    #endregion

    public void ReadXmlPrototype(XmlReader jobReader)
    {

		JobType = JobObjectType = jobReader.GetAttribute ("JobType");;
        FurnitureType = jobReader.GetAttribute("furnitureType");
        CanRepeat = bool.Parse(jobReader.GetAttribute("CanRepeat"));

        XmlReader reader = jobReader.ReadSubtree();

        while (reader.Read())
        {
            switch (reader.Name)
            {
                case "Name":
                    reader.Read();
                    Name = reader.ReadContentAsString();
                    break;
                case "JobTasks":
                    XmlReader readTasks = jobReader.ReadSubtree();
                    while (readTasks.Read())
                    {
                        if (readTasks.Name == "JobTask" && readTasks.IsStartElement())
                        {
                            SetTask(readTasks);
                        }
                    }

                    break;
				case "Requirements":
					var requirementReader = jobReader.ReadSubtree();
					while (requirementReader.Read())
					{
					if (requirementReader.Name == "Requirement" && requirementReader.IsStartElement())
						{
							SetRequirements (requirementReader);
						}
					}

					break;
            }
        }

    }

	private void SetRequirements(XmlReader reader){

		var quantity = reader.GetAttribute("qty");
		var type = reader.GetAttribute("type");

		_inventoryRequirements.Add (type, new Inventory (type, 50, 0));

	}

    private void SetTask(XmlReader taskReader)
    {

        var taskType = taskReader.GetAttribute("TaskType");

        var priority = int.Parse(taskReader.GetAttribute("Priority"));

        JobTask task = new JobTask { ParentJob = this, TaskType = taskType, Priority = priority };

        this.JobTasks.Add(task);

    }
}
