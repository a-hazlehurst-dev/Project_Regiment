using System;

public class JobTask
{
    private Action<JobTask> _cbTaskCompleted;

    public MasterJob ParentJob { get; set; }
    public Inventory RequiredInventory { get; set; }

    private void IsCompleted()
    {
        if (_cbTaskCompleted != null)
        {
            _cbTaskCompleted(this);
        }

    }

    #region CallBacks

    public void Register_OnTask_Completed(Action<MasterJob, JobTask> task)
    {
        _cbTaskCompleted += task;
    }

    public void UnRegister_OnTask_Completed(Action<MasterJob, JobTask> task)
    {
        _cbTaskCompleted -= task;
    }

    #endregion
}

