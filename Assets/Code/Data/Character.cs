
using UnityEngine;
using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

public class Character: IXmlSerializable
{

	public Job Job { get; protected set;}
	private Tile _destTile;
	private Tile _nextTileInPath;  // next tile in path finding sequence;
	private PathAStar pathAStar;
	private float _speed = 5; //tiles per second.
	private float _movementPercentage;// goes from 0 to 1, when moveing to dest tile. 1 being destination
	private Action<Character> cbCharacterChanged;


	public float X { 
		get
		{ 
			if(_nextTileInPath == null){return CurrentTile.X;}
			return Mathf.Lerp (CurrentTile.X, _nextTileInPath.X, _movementPercentage); 
		}
	}

	public float Y { 
		get
		{ 
			if(_nextTileInPath == null){ return CurrentTile.Y;}
			return Mathf.Lerp (CurrentTile.Y, _nextTileInPath.Y, _movementPercentage); 
		}
	}

	public Tile DestinationTile 
	{ 
		get { return _destTile; }
		set
		{ 
			if (_destTile != value) {
				_destTile = value;
				pathAStar = null; // new destination will always invalidate path.
			}
		}
	}

	public Inventory Inventory { get; set; } // what im carrying. (not hear / or equipment)
	public Tile CurrentTile { get; protected set; }

	public Character(Tile currentTile){
		CurrentTile = DestinationTile = _nextTileInPath = currentTile; 
	}
	public void SetTileAsCurrent(){
		DestinationTile = CurrentTile;
	}
	private void DropoffJobMaterials(){
		GameManager.Instance.InventoryService.PlaceInventory (Job, Inventory);
		Job.DoWork(0);

		if (Inventory.StackSize == 0) {
			Inventory = null;
		} else {
			Debug.Log ("Character is still carrying inventory, which wshould not be.");
			Inventory = null;
		}
	}

	private void DumpUnwantedMaterials(){
		//carrying something that the job doesnt want.
		//dump at feet. (or werever is closest);
		//TODO; go to nearest empty tile and dump it.
		if (GameManager.Instance.InventoryService.PlaceInventory (CurrentTile, Inventory) == false) {
			Debug.LogError ("Tried to dump invemntory into invalid tile " + CurrentTile.X + ", " + CurrentTile.Y);
			//FIXME: this will loose inventory perminantly.
			Inventory = null;
		}
	}






	private void GoToJobLocationAndWork(float deltaTime){
		DestinationTile = Job.Tile;

		if (CurrentTile == DestinationTile ) {
			Job.DoWork (deltaTime);
		}
	}





	private void Update_DoJob(float deltaTime)
	{
		if(!CharacterDoJobAction.DoIHaveAJob(this)) {return;}
		if(!CharacterDoJobAction.DoesJobMeetRequirements ()) { return; }
		GoToJobLocationAndWork (deltaTime);
	}


	public void GetNewJob(){
        Job = GameManager.Instance.JobService.GetAndRemoveOldestJob();
		//myJob = GameManager.Instance.JobQueue.DeQueue ();
		if (Job == null) {
			return;
		}
		DestinationTile = Job.Tile;

		Job.Register_JobStopped_Callback(OnJobStopped);

		//immediately check if job tile is reachable.

		pathAStar = new PathAStar (GameManager.Instance.TileDataGrid, CurrentTile, DestinationTile);
		if (pathAStar.Length () == 0) {
			Debug.LogError ("PathASTAR returned no path to target job tile..");
			Job.CancelJob();
			AbandonJob ();
			DestinationTile = CurrentTile;
		}
	}
	public void AbandonJob(){
		_nextTileInPath = DestinationTile = CurrentTile;
        GameManager.Instance.JobService.Add(Job);
		Job = null;

	}

	void Update_DoMovement(float deltaTime){

		if (CurrentTile == DestinationTile) 
		{
			pathAStar = null;
			return;
		} //already at destination.
       

		if (_nextTileInPath == null || _nextTileInPath == CurrentTile) {
			//get next tile from path finder.
			if (pathAStar == null || pathAStar.Length()==0) {
				pathAStar = new PathAStar (GameManager.Instance.TileDataGrid, CurrentTile, DestinationTile);
				if (pathAStar.Length () == 0) {
					
					Debug.LogError ("PathASTAR returned no path to destination.");
					//FIXME: job should be reenqueued.
					Job.CancelJob();
					AbandonJob ();
					return;
				
				}
				//ignore the first tile.
				_nextTileInPath = pathAStar.Dequeue();
			}
	
			//grab the tuke were about to enter.
			_nextTileInPath = pathAStar.Dequeue();

			if (_nextTileInPath == CurrentTile) {
				Debug.LogError ("UpdateMovement:+ next tile is currTile?");
			}
		}

		//should have valid next tile.

		float distToTravel = Mathf.Sqrt(
			Mathf.Pow (CurrentTile.X - _nextTileInPath.X, 2) + 
			Mathf.Pow (CurrentTile.Y - _nextTileInPath.Y, 2));

		if (_nextTileInPath.IsEnterable() == Enterability.Never) {
			// did a wall get built? reset pathing. invalidate the path.
			Debug.Log ("FIXME: character, pathed through 0 movemnt tile. (Unwalkable)");
			_nextTileInPath = null;
			pathAStar = null;
			return;
		} else if(_nextTileInPath.IsEnterable() == Enterability.Wait){
            //cant enter now but can soon enter, could be entering a door.
            // dont bail on the path, but slow down movement.
            return;
		}


		//distance travelled this update;
		float distThisFrame = (_speed/ _nextTileInPath.MovementCost) * deltaTime;

		//percentage distance to destination.
		float percThisFrame = distThisFrame / distToTravel ;

		//increment that to movement percentage
		_movementPercentage += percThisFrame;

		if (_movementPercentage >= 1) {
			CurrentTile = _nextTileInPath;
			_movementPercentage = 0;

			//FIXME? retain any overshot movement?;
		}
	}

	public void Update(float deltaTime){

		Update_DoJob (deltaTime);

		Update_DoMovement (deltaTime);


		if (cbCharacterChanged != null) {
			cbCharacterChanged (this);
		}
	}


	public void RegisterOnCharacterChangedCallback(Action<Character> cb){

		cbCharacterChanged += cb;
	}

	public void UnRegisterOnCharacterChangedCallback(Action<Character> cb){
		cbCharacterChanged -= cb;
	}


	public void OnJobStopped(Job j){
		// job was completed (if non repeating) or was cancelled.
		if (j != Job) {
			Debug.LogError ("Character being told about job thats not his. you forgot to unregister old job");
		}
		j.UnRegister_JobStopped_Callback(OnJobStopped);

		Job = null;
	}

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        return;
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("X", CurrentTile.X.ToString());
        writer.WriteAttributeString("Y", CurrentTile.Y.ToString());

     
    }
}
