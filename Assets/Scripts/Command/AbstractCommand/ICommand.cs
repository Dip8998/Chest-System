using UnityEngine;

namespace ChestSystem.Command
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}
