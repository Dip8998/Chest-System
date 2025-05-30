using ChestSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        private ChestView chestPrefab;
        private ChestSlotUIController chestSlotUIController;
        private ChestSlotUIView chestSlotUIView;
        private ChestModel chestModel;

        public ChestController(ChestListSO chestListSO, ChestView chestPrefab, ChestSlotUIController chestSlotUIController)
        {
            this.chestPrefab = chestPrefab;
            this.chestSlotUIController = chestSlotUIController;
            chestModel = new ChestModel(chestListSO);
        }

        public void SetChest()
        {
            Transform parent = chestSlotUIController.GetAvailableSlotPosition();
            chestSlotUIView = chestSlotUIController.GetCurrentSlot();

            chestPrefab.transform.SetParent(parent, false);
            chestPrefab.transform.localPosition = Vector3.zero;
            chestPrefab.SetController(this);
        }

        public void EnableChest() => chestPrefab.gameObject.SetActive(true);
    }
}

