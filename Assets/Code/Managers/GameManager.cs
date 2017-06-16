using UnityEngine;

public class GameManager : MonoBehaviour {


	public static GameManager Instance { get; protected set;}

	public GridManager  GridManager { get; protected set;}
	public SpriteManager SpriteManager { get; protected set;}
	public PeasantManager PeasantManager{ get; protected set;}

	void Awake(){
		if (Instance != null) {
			Debug.LogError ("There should only be one gamemanager");
		}

		Instance = this;

		SpriteManager = GetComponent<SpriteManager>();
		GridManager = GetComponent<GridManager> ();
		PeasantManager = GetComponent<PeasantManager>();
		InitGame();
	}




	void InitGame(){
		GridManager.GridSetup (SpriteManager);

    }

    void Update()
    {
        
    }

	
}
