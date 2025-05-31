using ChestSystem.Chest;

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
            chestController.SetChestStateText("Unlocked");
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
