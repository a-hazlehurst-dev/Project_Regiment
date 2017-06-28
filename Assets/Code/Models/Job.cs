using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {

	// this holds info for a queued up job, placing furniture, moving inventory, working at location, maybe fighting.

	public Tile Tile { get; protected set; }
	float _timeToComplete = 1f;

	Action<Job> _cbCJobCompleted;
	Action<Job> _cbJobCancelled;

	//FIXME:  hard coded a parameter for furniture. Do not like
	public string JobObjectType { get; protected set;}

	public Job(Tile tile, string jobObjectType, Action<Job> cbJobCompleted, float timeToComplete = .5f)
	{
		Tile = tile;
		_timeToComplete = timeToComplete;
		_cbCJobCompleted += cbJobCompleted;
		JobObjectType = jobObjectType;
	}

	public void RegisterJobCompletedCallback(Action<Job> cb){
		_cbCJobCompleted += cb;
	}

	public void RegisterJobCancelledCallback(Action<Job> cb){
		_cbJobCancelled += cb;
	}

	public void UnRegisterJobCompletedCallback(Action<Job> cb){
		_cbCJobCompleted -= cb;
	}

	public void UnRegisterJobCancelledCallback(Action<Job> cb){
		_cbJobCancelled -= cb;
	}

	public void DoWork(float workTime){
		_timeToComplete -= workTime;
		if (_timeToComplete <=0) {
			if (_cbCJobCompleted != null) {
				_cbCJobCompleted(this);
			}
		}
	}
	public void CancelJob(){
		if (_cbJobCancelled != null) 
			_cbJobCancelled(this);
	}
}

