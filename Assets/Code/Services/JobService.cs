using System;
using System.Collections.Generic;
using Assets.Code.Data;

public class JobService
{
    private JobPrototypes _jobPrototypes;
    private JobRepository _jobRepository;
    private Action<Job> _cbJobCreated;

    public void Init()
    {
        _jobRepository = new JobRepository();
        _jobPrototypes = new JobPrototypes();
        _jobPrototypes.Init();
    }

    public void Add(Job j)
    {
       if(_jobRepository.Add(j))
        {
            if (_cbJobCreated != null)
            {
                _cbJobCreated(j);
            }
            if (j.CanRepeat == false)
            {
               // GameManager.Instance.MessageListControl.AddMessage(new NotificationMessage { CooldownMax = 5f, Cooldown = 5f, Message = "Created job for " + j.JobObjectType, Parent = GameManager.Instance.MessageListControl });
            }
        }
    }

    public Job GetAndRemoveOldestJob()
    {
        var count = _jobRepository.FindAll().Count;

        if (count > 0)
        {
            var job = _jobRepository.FindAll()[count - 1];

            _jobRepository.Remove(job);

            return job;
        }

        return null;
       
    }

    public void Remove(Job j)
    {
        _jobRepository.Remove(j);
             
    }

    public List<Job> FindAll()
    {
        return _jobRepository.FindAll();
    }

    #region callbacks

    public void Register_Job_Created(Action<Job> cbJobCreated)
    {
        _cbJobCreated += cbJobCreated;
    }

    public void UnRegister_Job_Created(Action<Job> cbJobCreated)
    {
        _cbJobCreated -= cbJobCreated;
      
    }

    #endregion

}
