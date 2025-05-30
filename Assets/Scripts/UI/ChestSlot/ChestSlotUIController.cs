using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.UI
{
    public class ChestSlotUIController
    {
        private List<ChestSlotUIView> slotList = new List<ChestSlotUIView>();
        private ChestSlotUIView currentSlot;

        public void AddSlot(ChestSlotUIView chestSlotUIView)
        {
            slotList.Add(chestSlotUIView);
            chestSlotUIView.SetChest(false); 
            UpdateSlotOrder();
        }

        public Transform GetAvailableSlotPosition()
        {
            ChestSlotUIView slot = FindFirstAvailableSlot();
            if (slot != null)
            {
                slot.SetChest(true);
                currentSlot = slot;
                return slot.transform;
            }
            return null;
        }

        public bool HasAvailableSlot() => FindFirstAvailableSlot() != null;

        public ChestSlotUIView GetFirstAvailableSlot() => FindFirstAvailableSlot();

        private ChestSlotUIView FindFirstAvailableSlot()
        {
            foreach (ChestSlotUIView slot in slotList)
            {
                if (!slot.hasChest)
                {
                    return slot;
                }
            }
            return null;
        }

        private void UpdateSlotOrder()
        {
            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].transform.SetSiblingIndex(i);
            }
        }

        public ChestSlotUIView GetCurrentSlot() => currentSlot;
    }
}