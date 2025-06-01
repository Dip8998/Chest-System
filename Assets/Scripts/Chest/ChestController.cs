using ChestSystem.StateMachine;
using ChestSystem.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using ChestSystem.Main;
using ChestSystem.Command;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        private ChestView chestPrefab;
        private ChestSlotUIController chestSlotUIController;
        private ChestSlotUIView chestSlotUIView;
        private ChestModel chestModel;
        private ChestStateMachine stateMachine;
        private int requiredGems;
        private bool isChestUnlockWithGems = false;

        public event Action<ChestController> OnChestUnlocked;
        private ChestType currentChestType;

        public ChestController(ChestListSO chestListSO, ChestView chestPrefab, ChestSlotUIController chestSlotUIController)
        {
            this.chestPrefab = chestPrefab;
            this.chestSlotUIController = chestSlotUIController;
            chestModel = new ChestModel(chestListSO);
            CreateStateMachine();
        }

        private void CreateStateMachine()
        {
            stateMachine = new ChestStateMachine(this);
        }

        public void SetChest()
        {
            Transform parent = chestSlotUIController.GetAvailableSlotPosition();
            chestSlotUIView = chestSlotUIController.GetCurrentSlot();

            chestPrefab.transform.SetParent(parent, false);
            chestPrefab.transform.localPosition = Vector3.zero;

            ChestType chestType = GetRandomChestType();
            chestModel.SetCurrentChestType(chestType);
            this.currentChestType = chestType;

            float unlockDuration = chestModel.GetUnlockDurationForChestType(chestType);
            chestModel.SetRemainingTime(unlockDuration);

            chestPrefab.SetController(this, chestType);
            ChangeState(ChestState.Locked);
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

        public string TimeFormat(float time)
        {
            float timeInSeconds = time;

            int hours = (int)timeInSeconds / 3600;
            int minutes = (int)(timeInSeconds % 3600) / 60;
            int seconds = (int)(timeInSeconds % 60);

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        public IState CurrentChestState() => stateMachine.GetCurrentState();

        public void ChangeState(ChestState state)
        {
            if (CurrentChestState() != stateMachine.GetStates()[state])
            {
                stateMachine.ChangeState(state);
                chestPrefab.SetChestStateText(state.ToString());
            }
        }

        public void UpdateState()
        {
            stateMachine.Update();
            if (CurrentChestState() is UnlockedState && GameService.Instance.chestService.currentUnlockingChest == this)
            {
                OnChestUnlocked?.Invoke(this);
            }
        }

        public void EnableUnlockSelection()
        {
            GameService.Instance.uiService.SetUnlockSelectionPanel(
                GetGemsRequiredToUnlockCount(),
                chestModel.GetCurrentChestType().ToString(),
                this,
                chestSlotUIController,
                isChestUnlockWithGems
                );
        }

        public void UnlockChestWithGems()
        {
            ICommand openChestWithGemsCommand = new ChestOpenWithGems(GameService.Instance.resourceService, this);
            GameService.Instance.commandInvoker.ProcessCommands(this, openChestWithGemsCommand);
        }

        public int GetGemsRequiredToUnlockCount()
        {
            float timer = chestModel.GetRemainingTime();
            float gemsRequired = timer / 10f;

            requiredGems = (int)Math.Ceiling(gemsRequired);
            return requiredGems;
        }

        public void SetRemainingTime(float time) => chestModel.SetRemainingTime(time);

        public void SetTimerText() => chestPrefab.SetChestTimerText(GetRemainingTimeInSeconds());

        public void SetTimerText(float timeInSeconds) => chestPrefab.SetChestTimerText(timeInSeconds);

        public void DisableTimerText() => chestPrefab.DisableTimerText();

        public float GetRemainingTimeInSeconds() => chestModel.GetRemainingTime() * 60;

        public void SetChestStateText(string state) => chestPrefab.SetChestStateText(state);

        public void EnableChest() => chestPrefab.gameObject.SetActive(true);

        public ChestType GetChestType() => currentChestType;

        public void SetIsChestUnlockWithGems(bool isChestUnlockWithGems) => this.isChestUnlockWithGems = isChestUnlockWithGems;
    }
}