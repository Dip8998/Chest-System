using ChestSystem.Chest;

namespace ChestSystem.StateMachine
{
    public class CollectedState : IState
    {
        private ChestStateMachine chestStateMachine;
        private ChestController chestController;

        public CollectedState(ChestController chestController, ChestStateMachine chestStateMachine)
        {
            this.chestController = chestController;
            this.chestStateMachine = chestStateMachine;
        }

        public void OnStateEnter()
        {
            chestController.SetChestStateText("Collected");
            chestController.EnableUnlockSelection();
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
