using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{

    public int horizontalSpeed = 1;
    public int verticalSpeed = 1;

    public GameObject cursorPointer;

    private Vector3 currentMousePosition;
    public GameDrawMode GameDrawMode;
    public SelectionInfo SelectionInfo;
    

    void Start()
    {

        GameDrawMode = GameObject.FindObjectOfType<GameDrawMode>();
    }

    public Tile GetMouseOverTile()
    {
        return GameManager.Instance.GetTileAt( currentMousePosition  );
    }

    public Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public void DrawTiles(Tile tile)
    {
        FloorType floorMode = FloorType.Grass;//default draw mode

        if (GameDrawMode.FurnitureToDraw.Equals("grass"))
        {
            floorMode = FloorType.Grass;
        }
        else if (GameDrawMode.FurnitureToDraw.Equals("mud"))
        {
            floorMode = FloorType.Mud;
        }
        tile.Floor = floorMode;
    }

    void UpdateSelection()
    {
   
        if(GameDrawMode.GameBuildMode != BuildMode.Select)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }
        

        if (Input.GetMouseButtonUp(0))
        {
            Tile tileUnderMouse = GetMouseOverTile();
            if(SelectionInfo == null || SelectionInfo.Tile != tileUnderMouse)
            {
              
                SelectionInfo = new SelectionInfo();
                SelectionInfo.Tile = tileUnderMouse;
                RebuildTileSelectionContent();

                for (int i = 0; i < SelectionInfo.Content.Length; i++)
                {

                    if (SelectionInfo.Content[i] != null)
                    {
                        SelectionInfo.SubSelect = i;
                        break;
                    }
                }
            }
            else
            {

                ///rebuild - array of sub selections, incase characters move in or out.

                RebuildTileSelectionContent();
                do
                {
                    SelectionInfo.SubSelect = (SelectionInfo.SubSelect + 1) % SelectionInfo.Content.Length;
                } while (SelectionInfo.Content[SelectionInfo.SubSelect] == null);
                
            }

            Debug.Log(SelectionInfo.SubSelect);
        }

    }

    void RebuildTileSelectionContent()
    {
        var tile = SelectionInfo.Tile;
        ///ensure all the characters and plus rest of tile data is available.
        SelectionInfo.Content = new object[tile.Characters.Count + 3];

        for (int i = 0; i < tile.Characters.Count; i++)
        {
            SelectionInfo.Content[i] = tile.Characters[i];
        }

        SelectionInfo.Content[SelectionInfo.Content.Length - 3] = tile.Furniture;
        SelectionInfo.Content[SelectionInfo.Content.Length - 2] = tile.inventory;
        SelectionInfo.Content[SelectionInfo.Content.Length - 1] = tile;
    }

    public void DrawFurniture(Tile tile)
    {

        //if the furniture can be placed on the given tile, and their is no pending job on the tile.
        if (GameManager.Instance.FurnitureSpriteRenderer.IsFurniturePlacementValid(GameDrawMode.FurnitureToDraw, tile) && tile.PendingFurnitureJob == null)
        {
            Job job;
            if (GameManager.Instance.FurnitureService.FindFurnitureRequirements().ContainsKey(GameDrawMode.FurnitureToDraw))
            {
                //if there are any furniture requirements for this item ( wall needs clay), then create a new job with those requirements.
                //make a clone
                job = GameManager.Instance.FurnitureService.FindFurnitureRequirements()[GameDrawMode.FurnitureToDraw].Clone();
                // assign the tile
                job.Tile = tile;
            }
            else
            {
                //if no requirements exists for this furniture then, create a job default job without any requirements
                job = new Job(tile, GameDrawMode.FurnitureToDraw, FurnitureActions.JobComplete_FurnitureBuilding, .2f, null);
            }
            //tile is valid for this furniture type and not job already in place.
            job.FurniturePrototype = GameManager.Instance.FurnitureService.FindPrototypes()[GameDrawMode.FurnitureToDraw];
            GameManager.Instance.JobService.Add(job);
            //GameManager.Instance.JobQueue.Enqueue(job);

            tile.PendingFurnitureJob = job;

			job.UnRegister_JobStopped_Callback((theJob) =>
            {
                theJob.Tile.PendingFurnitureJob = null;
            });

        }
    }

    void Update()
    {
        ///on button down,
        /// check build mode

        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SelectionInfo = null;
            GameDrawMode.GameBuildMode = BuildMode.None;
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            GameDrawMode.GameBuildMode = BuildMode.Select;
        }
        if(Input.GetKeyUp(KeyCode.F2))
        {
            GameDrawMode.GameBuildMode = BuildMode.Furniture;
        }

        currentMousePosition = GetMousePosition();
        currentMousePosition.z = 0;
        Tile tileUnderMouse = GameManager.Instance.GetTileAt(currentMousePosition);

		if (tileUnderMouse == null) return;

        Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);

        cursorPointer.transform.position = cursorPosition;

        UpdateSelection();

        if (Input.GetMouseButtonUp(0))
        {

            new SelectionInfo();
            var tile = GameManager.Instance.GetTileAt(currentMousePosition);

            if (GameDrawMode.GameBuildMode == BuildMode.Floor)
            {
                DrawTiles(tile);

            }
            else if (GameDrawMode.GameBuildMode == BuildMode.Deconstruct)
            {
                if (tile.Furniture != null)
                {
                    tile.Furniture.Deconstruct();
                }
            }
            else if (GameDrawMode.GameBuildMode == BuildMode.Furniture)
            {
                DrawFurniture(tile);
            }
        }

        if (Input.GetMouseButton(1))
        {
            var tile = GameManager.Instance.GetTileAt(currentMousePosition);
            tile.Floor = FloorType.Grass;
        }

        if (Input.GetMouseButton(2))
        {
            var h = verticalSpeed * Input.GetAxis("Mouse Y") * -1;
            var v = horizontalSpeed * Input.GetAxis("Mouse X") * -1;
            transform.Translate(v, h, 0);
        }

        Camera.main.orthographicSize -= (Camera.main.orthographicSize / 2) * Input.GetAxis("Mouse ScrollWheel") * 1.5f;

    }

 




}
