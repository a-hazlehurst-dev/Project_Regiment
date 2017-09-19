using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{
        public MouseController MouseController;
    public MouseDrawing _mouseDrawHelper;
    public int horizontalSpeed = 1;
    public int verticalSpeed = 1;
    public SelectionInfo SelectionInfo;

    public GameObject cursorPointer;

    private Vector3 currentMousePosition;
    public GameDrawMode GameDrawMode;
 
    
    void Start()
    {
        GameDrawMode = GameObject.FindObjectOfType<GameDrawMode>();
        _mouseDrawHelper = new MouseDrawing();
        MouseController = new MouseController(_mouseDrawHelper);
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SelectionInfo = null;
            GameDrawMode.GameBuildMode = BuildMode.None;
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            SelectionInfo = new SelectionInfo();
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

        MouseController.OnLeftMouseButton(currentMousePosition, SelectionInfo);

        MouseController.OnRightMouseDown(verticalSpeed, horizontalSpeed, transform);

        MouseController.OnMiddleMouseDown(currentMousePosition);

        Camera.main.orthographicSize -= MouseHelper.SetOrphographicCameraSize(2, 1.5f);

    }




}
