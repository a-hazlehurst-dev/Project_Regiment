using UnityEngine;

public class GameManager : MonoBehaviour {

	public GridManager  gridManager;
    public SpriteManager spriteManager;
    public PeasantManager peasantManager;

	void Awake(){
        spriteManager = GetComponent<SpriteManager>();
        gridManager = GetComponent<GridManager> ();
        peasantManager = GetComponent<PeasantManager>();
		InitGame();
	}

	void InitGame(){
		gridManager.GridSetup (spriteManager);
        peasantManager.CreatePeasant();
        var peasent = peasantManager.GetPeasant(1);
        GameObject toInstantiate = spriteManager.peasentSprites[Random.Range(0, spriteManager.peasentSprites.Length)];
        if (toInstantiate == null)
        {
            Debug.Log("test");
        }

        var peasentToMake = Instantiate(toInstantiate, new Vector3(50, 50, 0), Quaternion.identity) as GameObject;
        peasentToMake.GetComponent<PeasentModel>().data = peasent;
    }

    void Update()
    {
        
    }

	
}
