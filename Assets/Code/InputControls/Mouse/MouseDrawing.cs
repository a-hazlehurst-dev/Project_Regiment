

using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrawing
{
    public void DrawTiles(Tile tile)
    {
        var GameDrawMode = GameManager.Instance.GameDrawMode;
        if (GameDrawMode.GameBuildMode != BuildMode.Floor)
        {
            return;
        }

        FloorType floorMode = FloorType.Grass;//default draw mode

        if (GameDrawMode.FurnitureToDraw.Equals("grass"))
        {
            floorMode = FloorType.Grass;
        }
        else if (GameDrawMode.FurnitureToDraw.Equals("mud"))
        {
            floorMode = FloorType.Mud;
        }

        tile.Floor = floorMode;
    }

    public void DrawFurniture(Tile tile)
    {
        var GameDrawMode = GameManager.Instance.GameDrawMode;
        if (GameDrawMode.GameBuildMode != BuildMode.Furniture)
        {
            return;
        }

        //if the furniture can be placed on the given tile, and their is no pending job on the tile.
        if (GameManager.Instance.FurnitureSpriteRenderer.IsFurniturePlacementValid(GameDrawMode.FurnitureToDraw, tile) && tile.PendingFurnitureJob == null)
        {
            Job job;
            if (GameManager.Instance.FurnitureService.FindFurnitureRequirements().ContainsKey(GameDrawMode.FurnitureToDraw))
            {
                //if there are any furniture requirements for this item ( wall needs clay), then create a new job with those requirements.
                //make a clone
                job = GameManager.Instance.FurnitureService.FindFurnitureRequirements()[GameDrawMode.FurnitureToDraw].Clone();
                // assign the tile
                job.Tile = tile;
            }
            else
            {
                //if no requirements exists for this furniture then, create a job default job without any requirements
                job = new Job(tile, GameDrawMode.FurnitureToDraw, FurnitureActions.JobComplete_FurnitureBuilding, .2f, null);
            }
            //tile is valid for this furniture type and not job already in place.
            job.FurniturePrototype = GameManager.Instance.FurnitureService.FindPrototypes()[GameDrawMode.FurnitureToDraw];
            GameManager.Instance.JobService.Add(job);
            //GameManager.Instance.JobQueue.Enqueue(job);

            tile.PendingFurnitureJob = job;

            job.UnRegister_JobStopped_Callback((theJob) =>
            {
                theJob.Tile.PendingFurnitureJob = null;
            });

        }
    }
    public void DrawSelect(Tile tile, SelectionInfo selectionInfo)
    {
        var GameDrawMode = GameManager.Instance.GameDrawMode;

        if (GameDrawMode.GameBuildMode != BuildMode.Select)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Input.GetMouseButtonUp(0))
        {
            Tile tileUnderMouse = MouseHelper.GetTileMouseIsOver();
            if (selectionInfo == null || selectionInfo.Tile != tileUnderMouse)
            {

                selectionInfo = new SelectionInfo();
                selectionInfo.Tile = tileUnderMouse;
                RebuildTileSelectionContent(selectionInfo);

                for (int i = 0; i < selectionInfo.Content.Length; i++)
                {

                    if (selectionInfo.Content[i] != null)
                    {
                        selectionInfo.SubSelect = i;
                        break;
                    }
                }
            }
            else
            {

                ///rebuild - array of sub selections, incase characters move in or out.

                RebuildTileSelectionContent(selectionInfo);
                do
                {
                    selectionInfo.SubSelect = (selectionInfo.SubSelect + 1) % selectionInfo.Content.Length;
                } while (selectionInfo.Content[selectionInfo.SubSelect] == null);

            }

            Debug.Log(selectionInfo.SubSelect);
        }
    }

    void RebuildTileSelectionContent(SelectionInfo selectionInfo)
    {
        var tile = selectionInfo.Tile;
        ///ensure all the characters and plus rest of tile data is available.
        selectionInfo.Content = new ISelectableItem[tile.Characters.Count + 3];

        for (int i = 0; i < tile.Characters.Count; i++)
        {
            selectionInfo.Content[i] = tile.Characters[i];
        }

        selectionInfo.Content[selectionInfo.Content.Length - 3] = tile.Furniture;
        selectionInfo.Content[selectionInfo.Content.Length - 2] = tile.inventory;
        selectionInfo.Content[selectionInfo.Content.Length - 1] = tile;
    }
}