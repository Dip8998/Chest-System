using ChestSystem.Chest;
using ChestSystem.Main;
using ChestSystem.StateMachine;
using ChestSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Service
{
    public class UIService : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject unlockChestPanel;
        [SerializeField] private GameObject undoCollectPanel;
        [SerializeField] private GameObject displayCollectedValues;
        [SerializeField] private GameObject queueCancelPanel;
        [SerializeField] private GameObject slotsFullPopup;
        [SerializeField] private GameObject gemsAreNotEnoughPanel;
        [SerializeField] private GameObject alreadyInQueuePanel;

        [Header("Buttons")]
        [SerializeField] private Button startTimerButton;
        [SerializeField] private Button unlockChestWithGemsButton;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button collectButton;
        [SerializeField] private Button cancelQueueButton;
        [SerializeField] private Button addChestSlotButton;
        [SerializeField] private Button generateChestButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI gemsText;
        [SerializeField] private TextMeshProUGUI chestTypeTextForUnlockPanel;
        [SerializeField] private TextMeshProUGUI chestTypeTextForUndoPanel;
        [SerializeField] private TextMeshProUGUI collectedGemsText;
        [SerializeField] private TextMeshProUGUI collectedCoinsText;

        [SerializeField] private GameObject chestSlotPrefab;
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
            UIButtonListeners();
            HideAllUIPanels();
        }

        private void UIButtonListeners()
        {
            addChestSlotButton.onClick.AddListener(CreateSlot);
            generateChestButton.onClick.AddListener(() => GameService.Instance.chestService.GenerateChest(GetSlots()));
            startTimerButton.onClick.AddListener(SetTimer);
        }

        private void OnEnable()
        {
            GameService.Instance.eventService.OnGemsCountChanged.AddListener(UpdateGemsUI);
            GameService.Instance.eventService.OnCoinsCountChanged.AddListener(UpdateCoinsUI);
            GameService.Instance.eventService.OnSlotsFull.AddListener(ShowSlotsFullPopup);
            GameService.Instance.eventService.OnNotEnoughGemsToUnlock.AddListener(ShowNotEnoughGemsPopup);
            GameService.Instance.eventService.OnChestAlreadyInQueue.AddListener(ShowAlreadyInQueuePopUp);
        }

        private void OnDisable()
        {
            GameService.Instance.eventService.OnGemsCountChanged.RemoveListener(UpdateGemsUI);
            GameService.Instance.eventService.OnCoinsCountChanged.RemoveListener(UpdateCoinsUI);
            GameService.Instance.eventService.OnSlotsFull.RemoveListener(ShowSlotsFullPopup);
            GameService.Instance.eventService.OnNotEnoughGemsToUnlock.RemoveListener(ShowNotEnoughGemsPopup);
            GameService.Instance.eventService.OnChestAlreadyInQueue.RemoveListener(ShowAlreadyInQueuePopUp);
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

            RemoveButtonListners();
            AddButtonsListners();

            ShowRelevantPanel(currentChestController.CurrentChestState());
        }

        private void AddButtonsListners()
        {
            unlockChestWithGemsButton.onClick.AddListener(UnlockChestWithGems);
            collectButton.onClick.AddListener(CollectChest);
            undoButton.onClick.AddListener(UndoUnlock);
            cancelQueueButton.onClick.AddListener(CancelQueue);
        }

        private void RemoveButtonListners()
        {
            unlockChestWithGemsButton.onClick.RemoveAllListeners();
            collectButton.onClick.RemoveAllListeners();
            undoButton.onClick.RemoveAllListeners();
            cancelQueueButton.onClick.RemoveAllListeners();
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
            GameService.Instance.soundService.Play(Sound.Sounds.BUTTONCLICK);
            GameObject slot = Instantiate(chestSlotPrefab, chestSlotContainer);
            ChestSlotUIView chestUISlot = slot.GetComponent<ChestSlotUIView>();
            chestSlotUIController.AddSlot(chestUISlot);
        }

        public void ShowRelevantPanel(IState currentState)
        {
            HideAllUIPanels();

            switch (currentState)
            {
                case LockedState:
                case UnlockingState:
                    unlockChestPanel.SetActive(true);
                    break;
                case UnlockedState:
                    undoCollectPanel.SetActive(true);
                    break;
                case CollectedState:
                    displayCollectedValues.SetActive(true);
                    break;
                case QueuedState:
                    queueCancelPanel.SetActive(true);
                    break;
            }
        }

        private void SetTimer()
        {
            GameService.Instance.soundService.Play(Sound.Sounds.BUTTONCLICK);
            HideAllUIPanels();
            if (currentChestController != null)
            {
                GameService.Instance.chestService.EnqueueChestForUnlock(currentChestController);
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
            GameService.Instance.soundService.Play(Sound.Sounds.BUTTONCLICK);
            currentChestController.UndoUnlockChestWithGems();
            HideAllUIPanels();
        }

        private void CollectChest()
        {
            GameService.Instance.soundService.Play(Sound.Sounds.BUTTONCLICK);
            GameService.Instance.soundService.Play(Sound.Sounds.CHESTOPEN);
            currentChestController.Collect();
            HideAllUIPanels();
            chestSlotUIController.SetIsSlotHasAChest(currentChestSlotUIView, false);
            displayCollectedValues.SetActive(true);
        }

        private void CancelQueue()
        {
            GameService.Instance.soundService.Play(Sound.Sounds.BUTTONCLICK);
            if (currentChestController != null)
            {
                currentChestController.ChangeState(ChestState.Locked);
                GameService.Instance.chestService.RemoveFromQueue(currentChestController);
                HideAllUIPanels();
            }
        }

        private void UpdateGemsUI(int newGemsCount)
        {
            collectedGemsText.text = newGemsCount.ToString();
        }

        private void UpdateCoinsUI(int newCoinsCount)
        {
            collectedCoinsText.text = newCoinsCount.ToString();
        }

        public void ShowSlotsFullPopup()
        {
            HideAllUIPanels();
            GameService.Instance.soundService.Play(Sound.Sounds.POPUPSOUND);
            slotsFullPopup.SetActive(true);
        }

        public void ShowNotEnoughGemsPopup()
        {
            HideAllUIPanels();
            GameService.Instance.soundService.Play(Sound.Sounds.POPUPSOUND);
            gemsAreNotEnoughPanel.SetActive(true);
        }

        public void ShowAlreadyInQueuePopUp()
        {
            HideAllUIPanels();
            GameService.Instance.soundService.Play(Sound.Sounds.POPUPSOUND);
            alreadyInQueuePanel.SetActive(true);
        }

        public void CloseButtons()
        {
            GameService.Instance.soundService.Play(Sound.Sounds.BUTTONCLICK);
        }

        public void HideAllUIPanels()
        {
            unlockChestPanel.SetActive(false);
            undoCollectPanel.SetActive(false);
            displayCollectedValues.SetActive(false);
            queueCancelPanel.SetActive(false);
            slotsFullPopup.SetActive(false);
            gemsAreNotEnoughPanel.SetActive(false);
            alreadyInQueuePanel.SetActive(false);
        }

        public ChestSlotUIController GetSlots() => chestSlotUIController;
    }
}