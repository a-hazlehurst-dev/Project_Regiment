using System;

public class JobTask
{
    private Action<JobTask> _cbTaskCompleted;
    public string TaskType { get; set; }

    public Job ParentJob { get; set; }
    public Inventory RequiredInventory { get; set; }
    public int Priority { get; set; }

    private void IsCompleted()
    {
        if (_cbTaskCompleted != null)
        {
            _cbTaskCompleted(this);
        }

    }

    #region CallBacks

    public void Register_OnTask_Completed(Action<JobTask> task)
    {
        _cbTaskCompleted += task;
    }

    public void UnRegister_OnTask_Completed(Action<JobTask> task)
    {
        _cbTaskCompleted -= task;
    }

    #endregion
}

