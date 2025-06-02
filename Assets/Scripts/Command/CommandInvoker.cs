using ChestSystem.Chest;
using ChestSystem.Resource;
using ChestSystem.Service;
using System.Collections.Generic;

namespace ChestSystem.Command
{
    public class CommandInvoker
    {
        private Dictionary<ChestController, Stack<ICommand>> chestHistory = new Dictionary<ChestController, Stack<ICommand>>();
        private ResourceService resourceService;

        public CommandInvoker(ResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        public void ProcessCommands(ChestController chestController,ICommand commandToProcess)
        {
            commandToProcess.Execute();
            RegisterCommand(chestController,commandToProcess);
        }

        private void RegisterCommand(ChestController chestController, ICommand command)
        {
            if (!chestHistory.ContainsKey(chestController))
            {
                chestHistory[chestController] = new Stack<ICommand>();
            }

            chestHistory[chestController].Push(command);
        }

        public void Undo(ChestController chestController)
        {
            if (chestHistory.ContainsKey(chestController) && chestHistory[chestController].Count > 0)
            {
                chestHistory[chestController].Pop().Undo();
            }
        }
    }
}
