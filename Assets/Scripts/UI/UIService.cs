using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.UI
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private Button addChestSlotButton;
        [SerializeField] private int assignedChestSlots;
        [SerializeField] private Transform chestSlotContainer;

        private ChestSlotUIController chestSlotUIController;


        private void Start()
        {
            chestSlotUIController = new ChestSlotUIController();
            AssignSlots();
            ButtonsListners();
        }

        private void ButtonsListners()
        {
            addChestSlotButton.onClick.AddListener(CreateSlot);
        }

        private void AssignSlots()
        {
            for (int i = 0; i < assignedChestSlots; i++)
            {
                CreateSlot();
            }
        }

        private void CreateSlot()
        {
            GameObject slot = Instantiate(chestSlotPrefab, chestSlotContainer);
            ChestSlotUIView chestUISlot = slot.GetComponent<ChestSlotUIView>();
            chestSlotUIController.AddSlot(chestUISlot);
        }
    }

}
