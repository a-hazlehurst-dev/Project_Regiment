using UnityEngine;

public class FooterMenuViewModel : MonoBehaviour {

    public GameObject pnlFloorsMenu;

    public void ToggleFloorMenuVisibility()
    {

        if (pnlFloorsMenu.activeSelf)
        {
            pnlFloorsMenu.SetActive(false);
            Vector3 p = new Vector3(-5f, -100, 0);
            pnlFloorsMenu.transform.position = p;
            return;
        }
        Vector3 position = new Vector3(5f, 180, 0);
        pnlFloorsMenu.transform.position = position;
        pnlFloorsMenu.SetActive(true);
    }

    public void SetDrawModeFloorGrass()
    {
        GameManager.Instance.SetDrawMode(Tile.FloorType.Grass);
    }
    
    public void SetDrawModeFloorMud()
    {
        GameManager.Instance.SetDrawMode(Tile.FloorType.Mud);
    }
}
