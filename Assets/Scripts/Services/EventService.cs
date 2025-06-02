using ChestSystem.Chest;
using ChestSystem.Event;
using ChestSystem.UI; 

namespace ChestSystem.Service
{
    public class EventService
    {
        public EventController<ChestController> OnChestUnlocked { get; private set; } 
        public EventController<int, int> OnChestCollected { get; private set; } 
        public EventController<ChestController> OnChestRemovedFromQueue { get; private set; }
        public EventController OnNotEnoughGemsToUnlock { get; private set; }
        public EventController OnChestAlreadyInQueue { get; private set; }

        public EventController<int> OnGemsCountChanged { get; private set; }
        public EventController<int> OnCoinsCountChanged { get; private set; }

        public EventController OnSlotsFull { get; private set; }

        public EventService()
        {
            OnChestUnlocked = new EventController<ChestController>();
            OnChestCollected = new EventController<int, int>();
            OnChestRemovedFromQueue = new EventController<ChestController>();
            OnNotEnoughGemsToUnlock = new EventController();
            OnChestAlreadyInQueue = new EventController();

            OnGemsCountChanged = new EventController<int>();
            OnCoinsCountChanged = new EventController<int>();

            OnSlotsFull = new EventController();
        }
    }
}