using UnityEngine;

public class BaseTileRenderer : MonoBehaviour 
{
    private SpriteManager _spriteManager;
	private Transform gridHolder;

	// Creates the grid and acts a facade in front.
	public void InitialiseTileMap(SpriteManager spriteManager)
	{
        _spriteManager = spriteManager;

        gridHolder = new GameObject ("Grid").transform;
        gridHolder.SetParent(gameObject.transform);

		CreateInitialTileMapGraphics();
    }

	//Maps the tile data to the tile graphics
    private void CreateInitialTileMapGraphics()
    {
		
		for (int x = 0; x < GameManager.Instance.TileDataGrid.GridWidth; x++)
        {

            for (int y = 0; y < GameManager.Instance.TileDataGrid.GridHeight; y++)
            {
      
                var tile = GameManager.Instance.TileDataGrid.GetTileAt (x, y);
                var go  = new GameObject();
                go.transform.position = new Vector3(tile.X, tile.Y, 0);
                var spriteRenderer = go.AddComponent<SpriteRenderer>();

                if(tile.Floor == FloorType.Grass)
                {
                    spriteRenderer.sprite = _spriteManager.FloorTiles["grass_"];
                }
                else if (tile.Floor == FloorType.Mud)
                {
                    spriteRenderer.sprite = _spriteManager.FloorTiles["mud_"];
                }

                go.name = "tile_(" + tile.X + ", " + tile.Y + ")";
				var sr = go.GetComponent<SpriteRenderer> ();

				sr.sortingLayerName = "Floor";
                go.transform.SetParent(gridHolder);

				tile.RegisterFloorTypeChangedCb ( (tile_data) => { OnTileTypeChanged(tile_data, go);});
            }
        }
    }
   
	//callback that response to a change of a tile.
	void OnTileTypeChanged(Tile tile_data, GameObject tile_go){
		if (_spriteManager.FloorTiles.Count < (int)tile_data.Floor) {
			Debug.LogError ("GridManager.OnTileTypeChanged cannot find floor type with index: " + tile_data.Floor);
		} 
		else {
            if(tile_data.Floor == FloorType.Grass)
            {
                tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.FloorTiles["grass_"];
            }
            else if (tile_data.Floor == FloorType.Mud)
            {
                tile_go.GetComponent<SpriteRenderer>().sprite = _spriteManager.FloorTiles["mud_"];
            }
        }

		GameManager.Instance.InvalidateTileGraph ();
	}

}
