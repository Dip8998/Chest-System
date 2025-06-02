using ChestSystem.Chest;
using ChestSystem.Main;

namespace ChestSystem.StateMachine
{
    public class UnlockedState : IState
    {
        private ChestStateMachine chestStateMachine;
        private ChestController chestController;

        public UnlockedState(ChestController chestController, ChestStateMachine chestStateMachine)
        {
            this.chestController = chestController;
            this.chestStateMachine = chestStateMachine;
        }

        public void OnStateEnter()
        {
            GameService.Instance.soundService.Play(Sound.Sounds.CHESTUNLOCKED);
            chestController.SetChestStateText("Unlocked");
            chestController.UpdateToUnlockedImage();
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
