using ChestSystem.Main;
using ChestSystem.UI;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using ChestSystem.StateMachine;

namespace ChestSystem.Chest
{
    public class ChestService
    {
        private ChestController chestController;
        private ChestPool chestPool;

        private Queue<ChestController> unlockQueue;
        public ChestController currentUnlockingChest { get; private set; }
        public ChestController nextQueuedChest { get; private set; }

        public ChestService(ChestListSO chestListSO, ChestView chestPrefab)
        {
            chestPool = new ChestPool(chestListSO, chestPrefab);
            unlockQueue = new Queue<ChestController>();
        }

        public void EnqueueChestForUnlock(ChestController chestToUnlock)
        {
            if (currentUnlockingChest == null)
            {
                StartUnlockingChest(chestToUnlock);
            }
            else
            {
                if (unlockQueue.Count > 0 && unlockQueue.Peek() != chestToUnlock)
                {
                    return;
                }

                if (currentUnlockingChest == chestToUnlock || unlockQueue.Contains(chestToUnlock))
                {
                    return;
                }

                unlockQueue.Enqueue(chestToUnlock);
                chestToUnlock.ChangeState(ChestState.Queued);
                UpdateNextQueuedChest();
            }
        }

        private void StartUnlockingChest(ChestController chest)
        {
            currentUnlockingChest = chest;
            chest.ChangeState(ChestState.Unlocking);
            chest.OnChestUnlocked += OnCurrentChestUnlocked;
            UpdateNextQueuedChest();
        }

        private void OnCurrentChestUnlocked(ChestController unlockedChest)
        {
            unlockedChest.OnChestUnlocked -= OnCurrentChestUnlocked;
            currentUnlockingChest = null;

            if (unlockQueue.Count > 0)
            {
                ChestController nextChest = unlockQueue.Dequeue();
                StartUnlockingChest(nextChest);
            }
            else
            {
                UpdateNextQueuedChest();
            }
        }

        private void UpdateNextQueuedChest()
        {
            nextQueuedChest = unlockQueue.Count > 0 ? unlockQueue.Peek() : null;
        }

        public void RemoveFromQueue(ChestController chestToRemove)
        {
            if (currentUnlockingChest == chestToRemove)
            {
                currentUnlockingChest.OnChestUnlocked -= OnCurrentChestUnlocked;
                currentUnlockingChest.ChangeState(ChestState.Locked);
                currentUnlockingChest = null;

                if (unlockQueue.Count > 0)
                {
                    ChestController nextChest = unlockQueue.Dequeue();
                    StartUnlockingChest(nextChest);
                }
                else
                {
                    UpdateNextQueuedChest();
                }
            }
            else if (unlockQueue.Contains(chestToRemove))
            {
                Queue<ChestController> tempQueue = new Queue<ChestController>();
                while (unlockQueue.Count > 0)
                {
                    ChestController chest = unlockQueue.Dequeue();
                    if (chest != chestToRemove)
                        tempQueue.Enqueue(chest);
                    else
                        chest.ChangeState(ChestState.Locked);
                }
                unlockQueue = tempQueue;
                UpdateNextQueuedChest();
            }
        }

        public void GenerateChest(ChestSlotUIController chestSlotUIController)
        {
            if (chestSlotUIController.HasAvailableSlot())
            {
                chestController = chestPool.GetChest(chestSlotUIController);
                chestController.EnableChest();
                chestController.SetChest();
            }
            else
            {
                GameService.Instance.uiService.ShowSlotsFullPopup();
            }
        }

        public ChestController GetChestController() => chestController;

        public bool IsAnyChestUnlocking() => currentUnlockingChest != null;

        public void ReturnChestToPool(ChestController chestController) => chestPool.ReturnToPool(chestController);
    }
}