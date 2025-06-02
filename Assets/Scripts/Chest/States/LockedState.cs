using ChestSystem.Chest;

namespace ChestSystem.StateMachine
{
    public class LockedState : IState
    {
        private ChestStateMachine chestStateMachine;
        private ChestController chestController;

        public LockedState(ChestController chestController, ChestStateMachine chestStateMachine)
        {
            this.chestController = chestController;
            this.chestStateMachine = chestStateMachine;
        }

        public void OnStateEnter()
        {
            chestController.SetTimerText();
            chestController.SetChestStateText("Locked");
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
