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

		//TODO: callbacks when new job arrives.

		if (cbJobCreated != null) {
			cbJobCreated (job);
		}
	}

	public void RegisterJobCreatedCallBack(Action<Job> cb){
		cbJobCreated += cb;
	}

	public void UnRegisterJobCreatedCallBack(Action<Job> cb){
		cbJobCreated -= cb;
	}
}
