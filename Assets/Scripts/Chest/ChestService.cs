using ChestSystem.UI;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestService 
    {
        private ChestController chestController;
        private ChestPool chestPool;

        public ChestService(ChestListSO chestListSO, ChestView chestPrefab)
        {
            chestPool = new ChestPool(chestListSO, chestPrefab);
        }

        public void GenerateChest(ChestSlotUIController chestSlotUIController)
        {
            Debug.Log("Trying to generate chest...");

            if (chestSlotUIController.HasAvailableSlot())
            {
                Debug.Log("Slot is available!");
                chestController = chestPool.GetChest(chestSlotUIController);
                chestController.EnableChest();
                chestController.SetChest();
            }
            else
            {
                Debug.LogWarning("No available chest slots!");
            }
        }


        public ChestController GetChestController() => chestController;
    }
}
