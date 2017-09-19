using UnityEngine;


public static class MouseHelper
{
    public static float SetOrphographicCameraSize(float mulitplier, float mouseIncrement)
    {
        return (Camera.main.orthographicSize / mulitplier) * Input.GetAxis("Mouse ScrollWheel") * mouseIncrement;
    }

    public static Tile GetTileMouseIsOver()
    {
        var currentMousePosition = GetMousePositionInWorld();

        currentMousePosition.z = 0;

        return GameManager.Instance.GetTileAt(currentMousePosition);
    }


    public static Vector3 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
