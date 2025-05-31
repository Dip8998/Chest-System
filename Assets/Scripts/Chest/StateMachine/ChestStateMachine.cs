using ChestSystem.Chest;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.StateMachine
{
    public class ChestStateMachine 
    {
        private IState currentState;
        public Dictionary<ChestState, IState> state = new Dictionary<ChestState, IState>();

        public ChestStateMachine(ChestController chestController)
        {
            CreateStates(chestController);
        }

        private void CreateStates(ChestController chestController)
        {
            state.Add(ChestState.Locked, new LockedState(chestController, this));
            state.Add(ChestState.Unlocking, new UnlockingState(chestController, this));
            state.Add(ChestState.Unlocked, new UnlockedState(chestController, this));
            state.Add(ChestState.Collected, new CollectedState(chestController, this));
        }

        private void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void ChangeState(ChestState newState) => ChangeState(state[newState]);

        public void Update() => currentState?.Update();

        public IState GetCurrentState() => currentState;

        public Dictionary<ChestState, IState> GetStates() => state;
    }
}

