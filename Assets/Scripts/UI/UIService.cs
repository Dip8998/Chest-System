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
        [SerializeField] private GameObject displayCollectedValues;
        [SerializeField] private GameObject queueCancelPanel;
        [SerializeField] private GameObject slotsFullPopup;
        [SerializeField] private Button startTimerButton;
        [SerializeField] private Button unlockChestWithGemsButton;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button collectButton;
        [SerializeField] private Button cancelQueueButton;
        [SerializeField] private TextMeshProUGUI gemsText;
        [SerializeField] private TextMeshProUGUI chestTypeTextForUnlockPanel;
        [SerializeField] private TextMeshProUGUI chestTypeTextForUndoPanel;
        [SerializeField] private TextMeshProUGUI collectedGemsText;
        [SerializeField] private TextMeshProUGUI collectedCoinsText;

        [SerializeField] private GameObject chestSlotPrefab;
        [SerializeField] private Button addChestSlotButton;
        [SerializeField] private Button generateChestButton;
        [SerializeField] private int assignedChestSlots;
        [SerializeField] private Transform chestSlotContainer;

        private ChestSlotUIController chestSlotUIController;
        private ChestController currentChestController;
        private ChestSlotUIView currentChestSlotUIView;
        private bool isUnlockedWithGems = false;


        private void Start()
        {
            chestSlotUIController = new ChestSlotUIController();
            AssignSlots();
            ButtonsListners();
            HideAllUIPanels();
            slotsFullPopup.SetActive(false);
        }

        public void SetUnlockSelectionPanel(
            int gemsRequired,
            string chestType,
            ChestController chestController,
            ChestSlotUIController chestSlotUIController,
            ChestSlotUIView chestSlotUIView,
            bool isUnlockedWithGems
            )
        {
            this.currentChestController = chestController;
            this.chestSlotUIController = chestSlotUIController;
            currentChestSlotUIView = chestSlotUIView;
            this.isUnlockedWithGems = isUnlockedWithGems;

            gemsText.text = gemsRequired.ToString();
            chestTypeTextForUnlockPanel.text = chestType + " Chest";
            chestTypeTextForUndoPanel.text = chestType + " Chest";

            unlockChestWithGemsButton.onClick.RemoveAllListeners();
            unlockChestWithGemsButton.onClick.AddListener(UnlockChestWithGems);

            collectButton.onClick.RemoveAllListeners();
            collectButton.onClick.AddListener(CollectChest);

            undoButton.onClick.RemoveAllListeners();
            undoButton.onClick.AddListener(UndoUnlock);

            cancelQueueButton.onClick.RemoveAllListeners();
            cancelQueueButton.onClick.AddListener(CancelQueue);

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
            displayCollectedValues.SetActive(false);
            queueCancelPanel.SetActive(false);
            slotsFullPopup.SetActive(false);
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
            else if (currentState is CollectedState)
            {
                displayCollectedValues.SetActive(true);
            }
            else if (currentState is QueuedState)
            {
                queueCancelPanel.SetActive(true);
            }
        }

        private void UnlockChestWithGems()
        {
            int requiredGems = currentChestController.GetGemsRequiredToUnlockCount();
            int currentGems = GameService.Instance.resourceService.GetResourceController().GetGemsCount();

            if (currentGems >= requiredGems)
            {
                currentChestController.UnlockChestWithGems();
                HideAllUIPanels();
            }
            else
            {
                ShowNotEnoughGemsPopup();
            }
        }

        private void UndoUnlock()
        {
            currentChestController.UndoUnlockChestWithGems();
            HideAllUIPanels();
        }

        private void CollectChest()
        {
            currentChestController.Collect();
            HideAllUIPanels();
            chestSlotUIController.SetIsSlotHasAChest(currentChestSlotUIView, false);
            displayCollectedValues.SetActive(true);
        }

        private void SetTimer()
        {
            HideAllUIPanels();
            if (currentChestController != null)
            {
                GameService.Instance.chestService.EnqueueChestForUnlock(currentChestController);
            }
        }

        private void CancelQueue()
        {
            if (currentChestController != null)
            {
                currentChestController.ChangeState(ChestState.Locked);
                GameService.Instance.chestService.RemoveFromQueue(currentChestController);
                HideAllUIPanels();
            }
        }

        public void SetCollectedValues(int collectedCoins, int collectedGems)
        {
            collectedCoinsText.text = collectedCoins.ToString();
            collectedGemsText.text = collectedGems.ToString();
        }

        public void ShowSlotsFullPopup()
        {
            HideAllUIPanels();
            slotsFullPopup.SetActive(true);
            Invoke(nameof(HideSlotsFullPopup), 2f);
        }

        private void HideSlotsFullPopup()
        {
            slotsFullPopup.SetActive(false);
        }

        public void ShowNotEnoughGemsPopup()
        {
            HideAllUIPanels();

            Debug.Log("Not enough gems to unlock this chest!");

        }
    }
}