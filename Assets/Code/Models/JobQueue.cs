using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JobQueue  {

	private Queue<Job> _jobQueue;

	Action<Job> cbJobCreated;


	public JobQueue(){
		_jobQueue = new Queue<Job>();
	}

	public void Enqueue(Job job){

        if(job.TimeToComplete < 0)
        {
            // job has a negative time, its not meant to be queued
            job.DoWork(0);
            return;
        }
		_jobQueue.Enqueue (job);
		//Debug.Log ("job added to queue");

		if (cbJobCreated != null) {
			
			cbJobCreated (job);
		}
	}

	public Job DeQueue(){
		if (_jobQueue.Count == 0) {
			return null;
		}
		return _jobQueue.Dequeue ();
	}

    public void Remove(Job j)
    {
        //TODO: is there a more effective solution.
        List<Job> jobs = new List<Job>(_jobQueue);

        if(jobs.Contains(j) == false)
        {
           // Debug.LogError("Job Queue:: => Remove: trying to dequeue job thats not there.");
           //most likely not on queue as character was working here.
        }
        jobs.Remove(j);
        _jobQueue = new Queue<Job>(jobs);
    }

	public void RegisterJobCreatedCallBack(Action<Job> cb){
		cbJobCreated += cb;
	}

	public void UnRegisterJobCreatedCallBack(Action<Job> cb){
		cbJobCreated -= cb;
	}
}
