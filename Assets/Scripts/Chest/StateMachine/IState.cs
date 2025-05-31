using UnityEngine;

namespace ChestSystem.StateMachine
{
    public interface IState
    {
        public void OnStateEnter();
        public void Update();
        public void OnStateExit();
    }
}
