using ChestSystem.Chest;
using ChestSystem.Resource;
using ChestSystem.Main;
using ChestSystem.Service;

namespace ChestSystem.Command
{
    public class ChestOpenWithGems : ICommand
    {
        private ChestController chestController;
        private ResourceController resourceController;
        private int gemsCostForThisChest; 

        public ChestOpenWithGems(ResourceService resourceService, ChestController chestController)
        {
            this.chestController = chestController;
            this.resourceController = resourceService.GetResourceController();
            this.gemsCostForThisChest = chestController.GetGemsRequiredToUnlockCount();
        }

        public void Execute()
        {
            int currentGems = resourceController.GetGemsCount();

            if (gemsCostForThisChest <= currentGems)
            {
                resourceController.DeductGems(gemsCostForThisChest);
                chestController.ChangeState(ChestState.Unlocked);
                chestController.DisableTimerText();
                chestController.SetIsChestUnlockWithGems(true);
            }
            else
            {
                GameService.Instance.eventService.OnNotEnoughGemsToUnlock.InvokeEvent();
            }
        }

        public void Undo()
        {
            resourceController.AddGems(gemsCostForThisChest);
        }
    }
}