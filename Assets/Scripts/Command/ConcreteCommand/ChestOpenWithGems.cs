using ChestSystem.Chest;
using ChestSystem.Resource;

namespace ChestSystem.Command
{
    public class ChestOpenWithGems : ICommand
    {
        private ChestController chestController;
        private ResourceController resourceController;
        private int requiredGems;
        private int gemsCount;

        public ChestOpenWithGems(ResourceService resourceService, ChestController chestController)
        {
            this.chestController = chestController;
            this.resourceController = resourceService.GetResourceController();
        }

        public void Execute()
        {
            requiredGems = chestController.GetGemsRequiredToUnlockCount();
            gemsCount = resourceController.GetGemsCount();

            if(requiredGems <= gemsCount)
            {
                int remainingGems = gemsCount - requiredGems;
                chestController.ChangeState(ChestState.Unlocked);
                chestController.DisableTimerText();
                chestController.SetIsChestUnlockWithGems(true);
                resourceController.SetGemsCount(remainingGems);
            }
        }

        public void Undo()
        {

        }
    }
}
