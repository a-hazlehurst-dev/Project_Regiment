using System.Collections.Generic;


public class JobRepository
{

    private List<Job> _jobs;

    public JobRepository()
    {
        _jobs = new List<Job>();
    }

    public bool Add(Job j)
    {
        if (!_jobs.Contains(j))
        {
            _jobs.Insert(0,j);
            return true;
        }
        return false;
    }

    public bool Remove(Job j)
    {
        if (_jobs.Contains(j)){
            _jobs.Remove(j);
            return true;
        }
        return false;
    }

    public List<Job> FindAll()
    {
        return _jobs;
    }



}
