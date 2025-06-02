using ChestSystem.Chest;
using ChestSystem.Main;
using UnityEngine;

namespace ChestSystem.StateMachine
{
    public class UnlockingState : IState
    {
        private ChestStateMachine chestStateMachine;
        private ChestController chestController;

        public UnlockingState(ChestController chestController, ChestStateMachine chestStateMachine)
        {
            this.chestController = chestController;
            this.chestStateMachine = chestStateMachine;
        }

        public void OnStateEnter()
        {
            chestController.SetChestStateText("Unlocking");
        }

        public void Update()
        {
            float currentRemainingTime = chestController.GetRemainingTimeInSeconds(); 

            if (currentRemainingTime > 0)
            {
                chestController.SetRemainingTime((currentRemainingTime - Time.deltaTime) / 60f); 
                chestController.SetTimerText(); 
            }
            else 
            {
                chestController.SetRemainingTime(0); 
                chestController.DisableTimerText();
                chestController.ChangeState(ChestState.Unlocked);
            }
        }

        public void OnStateExit()
        {
        }
    }
}