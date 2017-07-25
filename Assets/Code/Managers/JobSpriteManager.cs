using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteManager : MonoBehaviour {

	//this bare bones controller is going to barrow from furniture manager.

	FurnitureController _furnitureManager;
    
	private  Transform jobHolder;
	Dictionary<Job, GameObject>  _jobToGameObjectMap;
	// Use this for initialization
	void Start () {
		_jobToGameObjectMap = new Dictionary<Job, GameObject> ();
		_furnitureManager = GameObject.FindObjectOfType<FurnitureController> ();
		jobHolder = new GameObject ("Jobs").transform;
        GameManager.Instance.JobService.Register_Job_Created(OnJobCreated);

	}

	void OnJobCreated(Job job)
	{
        if(job.JobObjectType == null)
        {
            return;
        }

		GameObject job_go = new GameObject();


		if (_jobToGameObjectMap.ContainsKey (job)) {
			Debug.Log ("ON Job Created: attempting to create a job graphic, where graphic already exists (probable, re queue not job created!");
			return;
		}

		_jobToGameObjectMap.Add(job, job_go);

		job_go.name = "Job: " + job.JobObjectType + ": x: " + job.Tile.X + ", y" + job.Tile.Y;
        job_go.transform.position = new Vector3(job.Tile.X + ((job.FurniturePrototype.Width - 1) / 2f), job.Tile.Y + ((job.FurniturePrototype.Height - 1) / 2f), 0);
		

	
		SpriteRenderer sr = job_go.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = "Job";
		sr.sprite = _furnitureManager.GetSpriteForFurniture(job.JobObjectType); 
		sr.color = new Color (.2f, .2f, .2f, 0.5f); //transparent

        if (job.JobObjectType == "door")
        {
            var northTile = GameManager.Instance.TileDataGrid.GetTileAt(job.Tile.X, job.Tile.Y + 1);
            var southTile = GameManager.Instance.TileDataGrid.GetTileAt(job.Tile.X, job.Tile.Y - 1);

            if (northTile != null && southTile != null && northTile.Furniture != null && southTile.Furniture != null
                && northTile.Furniture.ObjectType == "wall" && southTile.Furniture.ObjectType == "wall")
            {
                job_go.transform.rotation = Quaternion.Euler(0, 0, 90);
            }

        }
        job_go.transform.SetParent(jobHolder, true);
        job.RegisterJobCompletedCallback (OnJobCompleted);
		job.RegisterJobCancelledCallback (OnJobCompleted);
	}

	void OnJobCompleted (Job job){

		GameObject job_go = _jobToGameObjectMap [job];


		job.UnRegisterJobCancelledCallback (OnJobCompleted);

        job.UnRegisterJobCompletedCallback (OnJobCompleted);
        Destroy (job_go);
	}
}
