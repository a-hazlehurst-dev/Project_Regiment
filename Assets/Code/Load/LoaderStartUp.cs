using System;

namespace Assets.Code.Load
{
    public class LoaderStartUp :ILoader
    {
        GameManager gameManager;

        public Action<string> On_LoadStateChanged;

        public void Load()
        {
            gameManager = GameManager.Instance;
            On_LoadStateChanged("loading Services");

            //get all the things needed to make the game run.
            var FurnitureService = new FurnitureService();
            gameManager.FurnitureService = FurnitureService;
            FurnitureService.Init();

            var RecipeService = new RecipeService();
            gameManager.RecipeService = RecipeService;
            RecipeService.Init();

            var JobService = new JobService();
            gameManager.JobService = JobService;
            JobService.Init();

            var CharacterService = new CharacterService();
            gameManager.CharacterService = CharacterService;
            CharacterService.Init();

            var  RoomService = new RoomService();
            gameManager.RoomService = RoomService;
            RoomService.Init();


            var InventoryService = new InventoryService();
            gameManager.InventoryService = InventoryService;
            InventoryService.Init();

            On_LoadStateChanged("completeted loading Services");


        }
    }
}
