using UnityEngine;

public class FooterMenuViewModel : MonoBehaviour {

    public GameObject pnlFloorsMenu;
	public GameObject pnlFurnitureMenu;
	public GameDrawMode gameDrawMode;

	void Start(){
		gameDrawMode = GameObject.FindObjectOfType<GameDrawMode> ();
	}

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

		Vector3 position = new Vector3(5f, 350, 0);
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


	public void SetDeconstruct(){
		gameDrawMode.SetMode_Deconstruct ();
	}

    public void SetFurnitureMode( string objectType)
    {
		gameDrawMode.SetupMode_BuildFurniture (objectType);
    }

    public void SetTileMode(string objectType)
    {
		gameDrawMode.SetupMode_BuildTiles (objectType);
        
    }
   
}
