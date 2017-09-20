using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{
    public MouseController MouseController;
    public MouseDrawing MouseDrawing;
    public int horizontalSpeed = 1;
    public int verticalSpeed = 1;
    //public SelectionInfo SelectionInfo;

    public GameObject cursorPointer;

    private Vector3 currentMousePosition;
    public GameDrawMode GameDrawMode;
 
    
    void Start()
    {
        GameDrawMode = GameObject.FindObjectOfType<GameDrawMode>();
        MouseDrawing = new MouseDrawing();
        MouseController = new MouseController(MouseDrawing);
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameDrawMode.GameBuildMode = BuildMode.None;
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            GameDrawMode.GameBuildMode = BuildMode.Select;
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            GameDrawMode.GameBuildMode = BuildMode.Furniture;
        }

        currentMousePosition = MouseHelper.GetMousePositionInWorld();

        currentMousePosition.z = 0;

        Tile tileUnderMouse = GameManager.Instance.GetTileAt(currentMousePosition);

		if (tileUnderMouse == null) return;

        Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);

        cursorPointer.transform.position = cursorPosition;

        MouseController.OnLeftMouseButton(currentMousePosition);

        MouseController.OnRightMouseDown(verticalSpeed, horizontalSpeed, transform);

        MouseController.OnMiddleMouseDown(currentMousePosition);

        Camera.main.orthographicSize -= MouseHelper.SetOrphographicCameraSize(2, 1.5f);

    }




}
