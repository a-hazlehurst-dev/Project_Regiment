
using UnityEngine;
namespace Assets.Code.Scripts
{
    public class GameDrawMode  : MonoBehaviour
    {
        public int ObjectTypeToDraw { get; set; }

        public string FurnitureTypeToDraw {get; set;}

        //private FurnitureController _furnitureController;
        GameObject furniturePreview;
        CameraScript cameraScript;

         void Start()
        {

            cameraScript = GameObject.FindObjectOfType<CameraScript>();
            if (furniturePreview == null)
            {
                furniturePreview = new GameObject();
                furniturePreview.transform.SetParent(this.transform);
                furniturePreview.AddComponent<SpriteRenderer>().sortingLayerName = "Job"; 
                furniturePreview.SetActive(false);

            }
        }
         void Update()
        {
            if (ObjectTypeToDraw ==2 && string.IsNullOrEmpty(FurnitureTypeToDraw) == false)
            {

                ShowFurnitureSpriteAtTile(FurnitureTypeToDraw, cameraScript.GetMouseOverTile());
            }
        }

        private void ShowFurnitureSpriteAtTile(string furnitureType, Tile t)
        {
            furniturePreview.SetActive(true);

            SpriteRenderer sr = furniturePreview.GetComponent<SpriteRenderer>();
            
            sr.sprite = GameManager.Instance.FurnitureController.GetSpriteForFurniture(furnitureType);
            sr.color = new Color(.2f, .2f, .2f, 0.5f);
            if (!GameManager.Instance.FurnitureController.IsFurniturePlacementValid(furnitureType, t))
            {
                sr.color = new Color(1f, .0f, .0f, 0.5f);
            }
             //transparent

            Furniture furnPrototypes = GameManager.Instance._furnitureService.FindPrototypes()[furnitureType];

            furniturePreview.transform.position = new Vector3(t.X + ((furnPrototypes.Width - 1) / 2f), t.Y + ((furnPrototypes.Height - 1) / 2f), 0);

        }
    }
}
