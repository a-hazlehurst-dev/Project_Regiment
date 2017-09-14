using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class Furniture : IXmlSerializable {

    private Func<Tile, bool> _funcPositionValidation;
    private List<Job> _jobs;
    protected Dictionary<string, float> _furnParameters;

    protected List<string> _updateActions;
    protected string _isEnterableAction;

    public Color Tint = Color.white;
    public Enterability IsEnterable()
    {

        if (string.IsNullOrEmpty(_isEnterableAction))
        {

            return Enterability.Ok;
        }

        DynValue result = FurnitureActions.CallFunction(_isEnterableAction, this);
        return (Enterability)result.Number;
    }
    public string Name
    {
        get
        {
            if (_name == null || _name.Length == 0)
            {
                return ObjectType;
            }
            return _name;
        }
        set { _name = value; }
    }
    private string _name = null;
    public Tile Tile { get; protected set; }
    public string ObjectType { get; protected set; }
    public float MovementCost { get; protected set; }
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public bool LinksToNeighbour { get; protected set; }
    public bool RoomEnclosure { get; protected set; }
   
    public Vector2 JobSpotOffset { get { return Vector2.zero; } set { } }
    public Vector2 JobSpawnSpotOffset  { get { return Vector2.zero; } set { } }

	public Action<Furniture> cbOnChanged;
	public Action<Furniture> cbOnRemoved;

    #region Constructors

    public Furniture()
    {
        _updateActions = new List<string>();
        _furnParameters = new Dictionary<string, float>();
        _jobs = new List<Job>();
        this._funcPositionValidation = this.DefaultIsPositionValid;
        this._isEnterableAction = "";

    }
    
    protected Furniture(Furniture other)
    {
        this.ObjectType = other.ObjectType;
        this.Name = other.Name;
        this.MovementCost = other.MovementCost;
        this.RoomEnclosure = other.RoomEnclosure;
        this.Width = other.Width;
        this.Height = other.Height;
        this.Tint = other.Tint;
        this.LinksToNeighbour = other.LinksToNeighbour;
        this.JobSpotOffset = other.JobSpotOffset;
        this.JobSpawnSpotOffset = other.JobSpawnSpotOffset;

        this._furnParameters = new Dictionary<string, float>(other._furnParameters);
        this._jobs = new List<Job>();

        if (other._updateActions != null)
        {
            this._updateActions = new List<string>(other._updateActions);
        }

        if (other._funcPositionValidation != null)
        {
            this._funcPositionValidation = (Func<Tile, bool>)other._funcPositionValidation.Clone();
        }

        this._isEnterableAction = other._isEnterableAction;
    }

    virtual public Furniture Clone()
    {
        return new Furniture(this);
    }
    #endregion

    public void Update(float deltaTime)
    {

        if (_updateActions != null)
        {
            //updateActions (this, deltaTime);
            FurnitureActions.CallFunctionsWithFurniture(_updateActions.ToArray(), this, deltaTime);
        }
    }

    public bool IsStockpile()
    {
        return ObjectType == "stockpile"; ;
    }

	public Tile GetJobSpotTile(){
		return GameManager.Instance.TileDataGrid.GetTileAt (Tile.X + (int)JobSpotOffset.x, Tile.Y + (int)JobSpotOffset.y);
	}

	public Tile GetSpawnSpotTile(){
		return GameManager.Instance.TileDataGrid.GetTileAt (Tile.X + (int)JobSpawnSpotOffset.x , Tile.Y + (int)JobSpawnSpotOffset.y);
	}

	public void Deconstruct(){

		Tile.UnPlaceFurniture (this);

		if (cbOnRemoved != null) {
			cbOnRemoved (this);
		}

		if (RoomEnclosure) {
			Room.DoFloodFillOnRemove (this.Tile);
		}

		GameManager.Instance.InvalidateTileGraph ();

	}

    static public Furniture PlaceFurniture(Furniture prototype,  Tile tile)
	{
		Furniture item = prototype.Clone();

		item.Tile = tile;
		if (tile.PlaceFurniture (item)==false)  {
			//if we couldnt place the object.
			//it was likely already occupied
			//do NOT return the object;
			return null;
		}

        if (item.LinksToNeighbour)
        {
			InformNeightboursOfChange (tile, item);

        }

		return item;
	}

	private static void InformNeightboursOfChange(Tile tile, Furniture item){
		//used to update neighbour graphics
		//inform neighbours that they have a new tile        
		int x = tile.X;
		int y = tile.Y;

		var t = GameManager.Instance.TileDataGrid.GetTileAt(x, y + 1);
		if (t != null && t.Furniture != null &&  t.Furniture.cbOnChanged!=null && t.Furniture.ObjectType == item.ObjectType)
		{
			t.Furniture.cbOnChanged(t.Furniture); //we have northern neighbour with same object as us, so change it with callback;
		}

		t = GameManager.Instance.TileDataGrid.GetTileAt(x +1, y);
		if (t != null && t.Furniture != null &&  t.Furniture.cbOnChanged !=null && t.Furniture.ObjectType == item.ObjectType)
		{
			t.Furniture.cbOnChanged(t.Furniture);
		}

		t = GameManager.Instance.TileDataGrid.GetTileAt(x, y- 1);
		if (t != null && t.Furniture != null && t.Furniture.cbOnChanged !=null && t.Furniture.ObjectType == item.ObjectType)
		{
			t.Furniture.cbOnChanged(t.Furniture);
		}
		t = GameManager.Instance.TileDataGrid.GetTileAt(x-1, y );
		if (t != null && t.Furniture != null  && t.Furniture.cbOnChanged !=null && t.Furniture.ObjectType == item.ObjectType)
		{
			t.Furniture.cbOnChanged(t.Furniture);
		}
	}

	public bool __IsValidPosition(Tile t){
		return _funcPositionValidation (t);
	}

	public bool DefaultIsPositionValid(Tile t){
     
        //foreach tile check they dont already have furniture.
        for (int x_off = t.X; x_off < (t.X + Width); x_off++)
        {
            for (int y_off = t.Y; y_off < (t.Y + Height); y_off++)
            {
                Tile t2 = GameManager.Instance.TileDataGrid.GetTileAt(x_off, y_off);

                if(t2 == null)
                {
                    return false;
                }

                if (t2.Furniture != null)
                {
                    return false;
                }
            }
        }

		return true;
	}

    #region Parameters
    public float GetParameter(string key, float default_val)
    {
        if (!_furnParameters.ContainsKey(key))
        {
            return default_val;

        }
        return _furnParameters[key];
    }

    public float GetParameter(string key)
    {
        return GetParameter(key, 0);
    }

    public void SetParameter(string key, float value)
    {
        _furnParameters[key] = value;
    }

    public void ChangeParameter(string key, float value)
    {
        if (!_furnParameters.ContainsKey(key))
        {
            _furnParameters[key] = value;
        }
        _furnParameters[key] += value;
    }
    #endregion
    
    #region Jobs

    public int JobCount()
    {
        return _jobs.Count;
    }

    public void AddJob(Job j)
    {
      
        j.furnitureToOperate = this;
        _jobs.Add(j);
        j.Register_JobStopped_Callback(OnJobStopped);
        GameManager.Instance.JobService.Add(j);
    }

    void OnJobStopped(Job j)
    {
        RemoveJob(j);
    }
    protected void RemoveJob(Job j)
    {
        j.UnRegister_JobStopped_Callback(OnJobStopped);
        _jobs.Remove(j);
        j.furnitureToOperate = null;
    }

    public void CancelJobs()
    {
        Job[] jobs_array = _jobs.ToArray();
        foreach (var job in jobs_array)
        {
            job.CancelJob();
        }
    }

    protected void ClearJobs()
    {
        Job[] jobs_array = _jobs.ToArray();
        foreach (var job in jobs_array)
        {
            RemoveJob(job);
        }
    }

    #endregion

    #region XmlReading

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXmlPrototype(XmlReader readerParent)
    {
        ObjectType = readerParent.GetAttribute("objectType");

        XmlReader xmlReader = readerParent.ReadSubtree();
        while (xmlReader.Read())
        {
            switch (xmlReader.Name)
            {
                case "Name":
                    xmlReader.Read();
                    Name = xmlReader.ReadContentAsString();
                    break;
                case "MovementCost":
                    xmlReader.Read();
                    MovementCost = xmlReader.ReadContentAsFloat();
                    break;
                case "Width":
                    xmlReader.Read();
                    Width = xmlReader.ReadContentAsInt();
                    break;
                case "Height":
                    xmlReader.Read();
                    Height = xmlReader.ReadContentAsInt();
                    break;
                case "LinksToNeighbour":
                    xmlReader.Read();
                    LinksToNeighbour = xmlReader.ReadContentAsBoolean();
                    break;
                case "EnclosesRoom":
                    xmlReader.Read();
                    RoomEnclosure = xmlReader.ReadContentAsBoolean();
                    break;
                case "BuildingJob":
                    float jobTime = float.Parse(xmlReader.GetAttribute("jobTime"));
                    List<Inventory> invs = new List<Inventory>();

                    XmlReader invReader = xmlReader.ReadSubtree();

                    while (invReader.Read())
                    {
                        if (invReader.Name == "Inventory")
                        {
                            invs.Add(
                                new Inventory(
                                    invReader.GetAttribute("objectType"),
                                    int.Parse(invReader.GetAttribute("amount")),
                                    0
                                ));
                        }
                    }

                    Job j = new Job(
                        null,
                        ObjectType,
                        FurnitureActions.JobComplete_FurnitureBuilding,
                        jobTime,
                        invs.ToArray());

                    GameManager.Instance.FurnitureService.furnPrototypes.RegisterJobFurniturePrototype(j, this);

                    break;

                case "OnUpdate":
                    string functionName = xmlReader.GetAttribute("FunctionName");
                    RegisterUpdateAction(functionName);
                    break;

                case "IsEnterable":

                    _isEnterableAction = xmlReader.GetAttribute("FunctionName");
                    break;
                case "Params":
                    ReadXmlParams(xmlReader);
                    break;

            }
        }
    }

    public void ReadXmlParams(XmlReader xmlReader)
    {
        XmlReader invReader = xmlReader.ReadSubtree();
        while (invReader.Read())
        {
            if (invReader.Name == "param")
            {
                SetParameter(invReader.GetAttribute("name"), int.Parse(invReader.GetAttribute("value")));
            }

        }
    }

    public void WriteXml(XmlWriter writer)
    {

        writer.WriteAttributeString("X", Tile.X.ToString());
        writer.WriteAttributeString("Y", Tile.Y.ToString());

        writer.WriteAttributeString("objectType", ObjectType);
        //writer.WriteAttributeString ("movementCost",MovementCost.ToString());

        foreach (string k in _furnParameters.Keys)
        {
            writer.WriteStartElement("Param");
            writer.WriteAttributeString("name", k);
            writer.WriteAttributeString("value", _furnParameters[k].ToString());
            writer.WriteEndElement();
        }
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.ReadToDescendant("Param"))
        {
            do
            {
                string k = reader.GetAttribute("name");
                float v = float.Parse(reader.GetAttribute("value"));
                _furnParameters[k] = v;

            } while (reader.ReadToNextSibling("Param"));
        }
    }

    #endregion

    #region Callbacks
    public void RegisterOnChangedCallback(Action<Furniture> callBackFunc){
		cbOnChanged += callBackFunc;
	}
	public void UnRegisterOnChangedCallback(Action<Furniture> callBackFunc){
		cbOnChanged -= callBackFunc;
	}

	public void RegisterUpdateAction(string luaFuncName){
		_updateActions.Add (luaFuncName);
	}
	public void UnRegisterUpdateAction(string luaFuncName){
		_updateActions.Remove(luaFuncName);
	}

	public void RegisterOnRemovedCallback(Action<Furniture> callBackFunc){
		cbOnRemoved += callBackFunc;
	}
	public void UnRegisterOnRemovedCallback(Action<Furniture> callBackFunc){
		cbOnRemoved-= callBackFunc;
	}

    #endregion

}

