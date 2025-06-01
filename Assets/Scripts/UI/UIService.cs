using ChestSystem.Chest;
using ChestSystem.Main;
using ChestSystem.StateMachine;
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
        [SerializeField] private TextMeshProUGUI chestTypeTextForUnlockPanel;
        [SerializeField] private TextMeshProUGUI chestTypeTextForUndoPanel;

        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private Button addChestSlotButton;
        [SerializeField] private Button generateChestButton;
        [SerializeField] private int assignedChestSlots;
        [SerializeField] private Transform chestSlotContainer;

        private ChestSlotUIController chestSlotUIController;
        private ChestController currentChestController;
        private bool isUnlockedWithGems = false;


        private void Start()
        {
            chestSlotUIController = new ChestSlotUIController();
            AssignSlots();
            ButtonsListners();
        }

        public void SetUnlockSelectionPanel(
            int gemsRequired, 
            string chestType, 
            ChestController chestController,
            ChestSlotUIController chestSlotUIController,
            bool isUnlockedWithGems
            )
        {
            this.currentChestController = chestController;
            this.chestSlotUIController = chestSlotUIController;
            this.isUnlockedWithGems = isUnlockedWithGems;

            gemsText.text = gemsRequired.ToString();
            chestTypeTextForUnlockPanel.text = chestType + " Chest";
            chestTypeTextForUndoPanel.text = chestType + " Chest";

            unlockChestWithGemsButton.onClick.RemoveAllListeners();
            unlockChestWithGemsButton.onClick.AddListener(UnlockChestWithGems);
            
            ShowRelevantPanel(currentChestController.CurrentChestState());
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

        public void HideAllUIPanels()
        {
            unlockChestPanel.SetActive(false);
            undoCollectPanel.SetActive(false);
        }

        public void ShowRelevantPanel(IState currentState)
        {
            HideAllUIPanels();

            if (currentState is LockedState || currentState is UnlockingState)
            {
                unlockChestPanel.SetActive(true);
            }
            else if (currentState is UnlockedState)
            {
                undoCollectPanel.SetActive(true);
            }
        }

        private void UnlockChestWithGems()
        {
            currentChestController.UnlockChestWithGems();
            HideAllUIPanels();
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
