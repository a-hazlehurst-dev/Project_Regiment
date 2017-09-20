using UnityEngine;


    public class MouseController
    {
        GameDrawMode _gameDrawMode;
        MouseDrawing _mouseDrawHelper;
  

        public MouseController(MouseDrawing mouseDrawHelper)
        {
            _gameDrawMode = GameManager.Instance.GameDrawMode;
            _mouseDrawHelper = mouseDrawHelper;
        }

        public void OnLeftMouseButton(Vector3 currentMousePosition)
        {
            if (Input.GetMouseButtonUp(0))
            {
                      
                var tile = GameManager.Instance.GetTileAt(currentMousePosition);

                _mouseDrawHelper.DrawTiles(tile);
                _mouseDrawHelper.DrawFurniture(tile);
                _mouseDrawHelper.DrawSelect(tile);

                //if (tile.Furniture != null )
                //{
                //    tile.Furniture.Deconstruct();
                //}
            }
        }

        public void OnMiddleMouseDown(Vector3 currentMousePosition)
        {
            if (Input.GetMouseButton(2))
            {
                var tile = GameManager.Instance.GetTileAt(currentMousePosition);
                tile.Floor = FloorType.Grass;

            }
        }
        public void OnRightMouseDown(float verticalSpeed, float horizontalSpeed, Transform transformToMove)
        {

            if (Input.GetMouseButton(1))
            {
                var h = verticalSpeed * Input.GetAxis("Mouse Y") * -1;
                var v = horizontalSpeed * Input.GetAxis("Mouse X") * -1;
                transformToMove.Translate(v, h, 0);
            }

        }
    }

