
using UnityEngine;



public class GameDrawMode  : MonoBehaviour
{

	public BuildMode GameBuildMode = BuildMode.None;

	public string FurnitureToDraw { get; set; }

	//private FurnitureSpriteRenderer _furnitureController;
	GameObject furniturePreview;
	CameraScript cameraScript;

    void Start()
    {

        cameraScript = GameObject.FindObjectOfType<CameraScript>();
        if (furniturePreview == null)
        {
            furniturePreview = new GameObject();
            furniturePreview.transform.SetParent(this.transform);
            furniturePreview.AddComponent<SpriteRenderer>().sortingLayerName = "Job";
            furniturePreview.SetActive(false);

        }
    }

    void Update()
    {
        if (GameBuildMode == BuildMode.Furniture && string.IsNullOrEmpty(FurnitureToDraw) == false)
        {
            furniturePreview.GetComponent<SpriteRenderer>().enabled = true;
            ShowFurnitureSpriteAtTile(FurnitureToDraw, MouseHelper.GetTileMouseIsOver());
        }
        else
        {
            furniturePreview.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        
    }

    public bool IsObjectDraggable ()
	{
		if (GameBuildMode == BuildMode.Floor || GameBuildMode == BuildMode.Deconstruct)
			return true;
		if (GameBuildMode == BuildMode.Furniture) {
			Furniture proto = GameManager.Instance.FurnitureService.FindPrototypes () [FurnitureToDraw];
			return proto.Width == 1 && proto.Height == 1;
		}
        

		return false;
	}


	

	private void ShowFurnitureSpriteAtTile (string furnitureType, Tile t)
	{
		furniturePreview.SetActive (true);

		SpriteRenderer sr = furniturePreview.GetComponent<SpriteRenderer> ();
            
		sr.sprite = GameManager.Instance.FurnitureSpriteRenderer.GetSpriteForFurniture (furnitureType);
		sr.color = new Color (.2f, .2f, .2f, 0.5f);
		if (!GameManager.Instance.FurnitureSpriteRenderer.IsFurniturePlacementValid (furnitureType, t)) {
			sr.color = new Color (1f, .0f, .0f, 0.5f);
		}
		//transparent

		Furniture furnPrototypes = GameManager.Instance.FurnitureService.FindPrototypes () [furnitureType];

		furniturePreview.transform.position = new Vector3 (t.X + ((furnPrototypes.Width - 1) / 2f), t.Y + ((furnPrototypes.Height - 1) / 2f), 0);

	}

	public void SetMode_Deconstruct ()
	{
		GameBuildMode = BuildMode.Deconstruct;

	}

	public void SetupMode_BuildFurniture (string objectType)
	{
		GameBuildMode = BuildMode.Furniture;
		FurnitureToDraw = objectType;
	}
	public void SetupMode_BuildTiles (string objectType){
		GameBuildMode = BuildMode.Floor;
		FurnitureToDraw = objectType;
	}
}



