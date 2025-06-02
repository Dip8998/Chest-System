using ChestSystem.Chest;
using UnityEngine;

namespace ChestSystem.StateMachine
{
    public class QueuedState : IState
    {
        private ChestController chestController;
        private ChestStateMachine chestStateMachine;

        public QueuedState(ChestController controller, ChestStateMachine chestStateMachine)
        {
            chestController = controller;
            this.chestStateMachine = chestStateMachine;
        }

        public void OnStateEnter()
        {
            chestController.SetChestStateText("QUEUED"); 
            chestController.DisableTimerText(); 
        }

        public void Update()
        {
            
        }

        public void OnStateExit()
        {

        }
    }
}