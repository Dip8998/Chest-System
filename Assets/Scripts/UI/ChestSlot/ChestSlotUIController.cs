using ChestSystem.Chest;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.UI
{
    public class ChestSlotUIController
    {
        private List<ChestSlotUIView> slotList = new List<ChestSlotUIView>();
        private Dictionary<ChestSlotUIView, bool> isSlotAvailable = new Dictionary<ChestSlotUIView, bool>();
        private ChestSlotUIView currentSlot;

        public void AddSlot(ChestSlotUIView chestSlotUIView)
        {
            slotList.Add(chestSlotUIView);
            isSlotAvailable[chestSlotUIView] = true;
            UpdateSlotOrder();
        }

        public Transform GetAvailableSlotPosition()
        {
            foreach (var slot in slotList)
            {
                if (isSlotAvailable.TryGetValue(slot, out bool available) && available)
                {
                    isSlotAvailable[slot] = false;
                    currentSlot = slot;
                    return slot.transform;
                }
            }
            return null;
        }

        public bool HasAvailableSlot()
        {
            foreach (var kv in isSlotAvailable)
            {
                if (kv.Value) return true;
            }
            return false;
        }

        public void SetIsSlotHasAChest(ChestSlotUIView slot, bool hasChest)
        {
            if (isSlotAvailable.ContainsKey(slot))
                isSlotAvailable[slot] = !hasChest;
        }

        private void UpdateSlotOrder()
        {
            for (int i = 0; i < slotList.Count; i++)
                slotList[i].transform.SetSiblingIndex(i);
        }

        public ChestSlotUIView GetCurrentSlot() => currentSlot;
    }
}
