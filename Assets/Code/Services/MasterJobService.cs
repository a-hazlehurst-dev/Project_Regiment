
using System;
using System.Collections.Generic;

public class MasterJobService
{
    private MasterJobRepository _masterJobRepository;

    private Action<MasterJob> _cbJobAdded;
    private Action<MasterJob> _cbJobRemoved;
    private Action<MasterJob, List<MasterJob>> _cbJobListChanged;

    public MasterJobService()
    {
        _masterJobRepository = new MasterJobRepository();
    }

    public void AddJob(MasterJob job)
    {
        _masterJobRepository.Add(job);
        if(_cbJobAdded != null)
        {
            _cbJobAdded(job);
        }
    }

    private void RemoveJob(MasterJob job)
    {
        _masterJobRepository.Remove(job);
        if (_cbJobRemoved != null)
        {
            _cbJobRemoved(job);
        }
    }

    public void CancelJob(MasterJob job)
    {
        RemoveJob(job);
    }


    #region CallBacks


    public void Register_OnJobAdded(Action<MasterJob> cb)
    {
        _cbJobAdded += cb;
    }

    public void UnRegister_OnJobAdded(Action<MasterJob> cb)
    {
        _cbJobAdded -= cb;
    }

    public void Register_OnJobRemoved(Action<MasterJob> cb)
    {
        _cbJobRemoved += cb;
    }

    public void UnRegister_OnJobRemoved(Action<MasterJob> cb)
    {
        _cbJobRemoved -= cb;
    }

    #endregion
}

