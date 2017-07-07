using UnityEngine;

public class FooterMenuViewModel : MonoBehaviour {

    public GameObject pnlFloorsMenu;
	public GameObject pnlFurnitureMenu;

    public void ToggleFloorMenuVisibility()
    {

		InactivateAll();
        Vector3 position = new Vector3(5f, 180, 0);
        pnlFloorsMenu.transform.position = position;
        pnlFloorsMenu.SetActive(true);
    }

	public void ToggleFurnitureMenuVisibility()
	{
		InactivateAll ();

		Vector3 position = new Vector3(5f, 180, 0);
		pnlFurnitureMenu.transform.position = position;
		pnlFurnitureMenu.SetActive(true);
	}

	public void InactivateAll(){
		pnlFloorsMenu.SetActive(false);
		Vector3 p = new Vector3(-5f, -100, 0);
		pnlFloorsMenu.transform.position = p;

		pnlFurnitureMenu.SetActive(false);
		pnlFurnitureMenu.transform.position = p;
		return;
	}

    public void SetDrawModeFloorGrass()
    {
        GameManager.Instance.SetDrawMode(1,"grass");
    }
    
    public void SetDrawModeFloorMud()
	{
		GameManager.Instance.SetDrawMode (1,"mud");
	}

	public void SetDrawModeToWall()
	{
		GameManager.Instance.SetDrawMode(2, "wall");
	}

	public void SetDrawModeDoor()
	{
		GameManager.Instance.SetDrawMode(2, "door");
	}
}
