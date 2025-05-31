using ChestSystem.Chest;
using ChestSystem.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.UI
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private GameObject unlockChestPanel;
        [SerializeField] private GameObject undoCollectPanel;
        [SerializeField] private Button startTimerButton;
        [SerializeField] private Button unlockChestWithGemsButton;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button collectButton;
        [SerializeField] private TextMeshProUGUI gemsText;
        [SerializeField] private TextMeshProUGUI chestTypeText;

        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private Button addChestSlotButton;
        [SerializeField] private Button generateChestButton;
        [SerializeField] private int assignedChestSlots;
        [SerializeField] private Transform chestSlotContainer;

        private ChestSlotUIController chestSlotUIController;
        private ChestController currentChestController;


        private void Start()
        {
            chestSlotUIController = new ChestSlotUIController();
            AssignSlots();
            ButtonsListners();
        }

        public void SetCurrentChestController(ChestController chestController)
        {
            currentChestController = chestController;
        }

        private void ButtonsListners()
        {
            addChestSlotButton.onClick.AddListener(CreateSlot);
            generateChestButton.onClick.AddListener(() => GameService.Instance.chestService.GenerateChest(GetSlots()));
            startTimerButton.onClick.AddListener(SetTimer);
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

        public ChestSlotUIController GetSlots() => chestSlotUIController;

        private void HideAllUIPanels()
        {
            unlockChestPanel.SetActive(false);
        }

        public void ShowUnlockChestPanel()
        {
            unlockChestPanel.SetActive(true);
        }

        private void SetTimer()
        {
            HideAllUIPanels();
            if (currentChestController != null)
            {
                GameService.Instance.chestService.EnqueueChestForUnlock(currentChestController);
            }
        }
    }

}
