using UnityEngine;

namespace ChestSystem.UI
{
    public class ChestSlotUIView : MonoBehaviour
    {
        public bool hasChest { get; private set; }

        public void SetChest(bool hasChest)
        {
            this.hasChest = hasChest;
        }
    }
}
