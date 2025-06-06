using ChestSystem.StateMachine;
using ChestSystem.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using ChestSystem.Main;
using ChestSystem.Command;
using ChestSystem.Sound;

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

        public void UpdateToUnlockedImage()
        {
            Sprite unlockedSprite = chestModel.GetUnlockedChestImage(currentChestType);
            if (unlockedSprite != null)
            {
                chestPrefab.SetChestImage(unlockedSprite);
            }
        }


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

                if (state == ChestState.Unlocked)
                {
                    GameService.Instance.eventService.OnChestUnlocked.InvokeEvent(this);
                }
            }
        }

        public void UpdateState()
        {
            stateMachine.Update();
        }

        public void EnableUnlockSelection()
        {
            GameService.Instance.soundService.Play(Sounds.CHESTPRESSEDSOUND);
            GameService.Instance.uiService.SetUnlockSelectionPanel(
                GetGemsRequiredToUnlockCount(),
                chestModel.GetCurrentChestType().ToString(),
                this,
                chestSlotUIController,
                chestSlotUIView,
                isChestUnlockWithGems
                );
        }

        public void UnlockChestWithGems()
        {
            ICommand openChestWithGemsCommand = new ChestOpenWithGems(GameService.Instance.resourceService, this);
            GameService.Instance.commandInvoker.ProcessCommands(this, openChestWithGemsCommand);

            SetIsChestUnlockWithGems(true);
            GameService.Instance.chestService.RemoveFromQueue(this); 

            ChangeState(ChestState.Unlocked); 
        }

        public void UndoUnlockChestWithGems()
        {
            GameService.Instance.commandInvoker.Undo(this);
            ChangeState(ChestState.Locked);

            Sprite lockedSprite = GetChestImage(chestModel.GetCurrentChestType()); 
            chestPrefab.SetChestImage(lockedSprite);

            float originalDuration = chestModel.GetUnlockDurationForChestType(chestModel.GetCurrentChestType());
            chestModel.SetRemainingTime(originalDuration);

            SetTimerText();
            SetIsChestUnlockWithGems(false);
            GameService.Instance.chestService.RemoveFromQueue(this);
        }

        public int GetGemsRequiredToUnlockCount()
        {
            float timer = chestModel.GetRemainingTime();
            float gemsRequired = timer / 10f;

            requiredGems = (int)Math.Ceiling(gemsRequired);
            return requiredGems;
        }

        public void Collect()
        {
            ChangeState(ChestState.Collected);
            RemoveChest();

            chestSlotUIController.SetIsSlotHasAChest(chestSlotUIView, false);

            int coins = GetCollectedCoins();
            int gems = GetCollectedGems();

            GameService.Instance.eventService.OnChestCollected.InvokeEvent( coins, gems);
            GameService.Instance.eventService.OnCoinsCountChanged.InvokeEvent(coins);
            GameService.Instance.eventService.OnGemsCountChanged.InvokeEvent(gems);
        }

        private int GetCollectedCoins()
        {
            var rewards = chestModel.GetChestRewards(chestModel.GetCurrentChestType());
            return UnityEngine.Random.Range(rewards.minCoins, rewards.maxCoins + 1);
        }

        private int GetCollectedGems()
        {
            var rewards = chestModel.GetChestRewards(chestModel.GetCurrentChestType());
            return UnityEngine.Random.Range(rewards.minGems, rewards.maxGems + 1);
        }

        public void SetRemainingTime(float time) => chestModel.SetRemainingTime(time);

        public void SetTimerText() => chestPrefab.SetChestTimerText(GetRemainingTimeInSeconds());

        public void SetTimerText(float timeInSeconds) => chestPrefab.SetChestTimerText(timeInSeconds);

        public void DisableTimerText() => chestPrefab.DisableTimerText();

        public float GetRemainingTimeInSeconds() => chestModel.GetRemainingTime() * 60;

        public void SetChestStateText(string state) => chestPrefab.SetChestStateText(state);

        public void RemoveChest()
        {
            GameService.Instance.chestService.ReturnChestToPool(this);
            chestPrefab.gameObject.SetActive(false);
        }

        public void EnableChest() => chestPrefab.gameObject.SetActive(true);

        public ChestType GetChestType() => currentChestType;

        public void SetIsChestUnlockWithGems(bool isChestUnlockWithGems) => this.isChestUnlockWithGems = isChestUnlockWithGems;
    }
}