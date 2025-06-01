using ChestSystem.Chest;
using ChestSystem.Main;
using UnityEngine;

namespace ChestSystem.StateMachine
{
    public class UnlockingState : IState
    {
        private ChestStateMachine chestStateMachine;
        private ChestController chestController;
        private float remainingTime;

        public UnlockingState(ChestController chestController, ChestStateMachine chestStateMachine)
        {
            this.chestController = chestController;
            this.chestStateMachine = chestStateMachine;
        }

        public void OnStateEnter()
        {
            chestController.SetChestStateText("Unlocking");
            remainingTime = chestController.GetRemainingTimeInSeconds();
        }

        public void Update()
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;

                chestController.SetRemainingTime(remainingTime / 60);
                chestController.SetTimerText(remainingTime);
            }
            else if (remainingTime <= 0)
            {
                remainingTime = 0;
                chestController.SetRemainingTime(remainingTime / 60);
                chestController.DisableTimerText();
                chestStateMachine.ChangeState(ChestState.Unlocked);
            }
        }

        public void OnStateExit() { }

    }
}