using System.Collections.Generic;

public class MasterJobRepository
{
    private List<MasterJob> _masterJobs;

    public MasterJobRepository()
    {
        _masterJobs = new List<MasterJob>();
    }

    public List<MasterJob> All()
    {
        return _masterJobs;
    }

    public void Add(MasterJob masterJob)
    {
        _masterJobs.Add(masterJob);
    }

    public void Remove(MasterJob masterJob)
    {
        _masterJobs.Remove(masterJob);
    }
}

