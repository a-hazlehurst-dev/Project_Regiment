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

	public void RegisterJobCreatedCallBack(Action<Job> cb){
		cbJobCreated += cb;
	}

	public void UnRegisterJobCreatedCallBack(Action<Job> cb){
		cbJobCreated -= cb;
	}
}
