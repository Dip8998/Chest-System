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

            ChestType chestType = GetRandomChestType();
            chestModel.SetCurrentChestType(chestType);

            chestPrefab.SetController(this, chestType);
        }

        public Sprite GetChestImage(ChestType chestType) => chestModel.GetChestImage(chestType);

        public ChestType GetRandomChestType()
        {
            Dictionary<ChestType, float> chestTypeChance = chestModel.GetChestTypeByChance();
            float totalWeight = 0f;

            foreach (var value in chestTypeChance.Values)
            {
                totalWeight += value;
            }

            float randomvalue = UnityEngine.Random.Range(0, totalWeight);
            float cumilativeWeight = 0f;

            foreach (var entry in chestTypeChance)
            {
                cumilativeWeight += entry.Value;

                if (randomvalue < cumilativeWeight)
                {
                    chestModel.SetCurrentChestType(entry.Key);
                    return entry.Key;
                }
            }
            return ChestType.Common;
        }

        public void EnableChest() => chestPrefab.gameObject.SetActive(true);
    }
}

