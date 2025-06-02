using ChestSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestPool 
    {
        private List<PooledChest> pooledChests = new List<PooledChest>();
        private ChestListSO chestListSO;
        private ChestView chestView;

        public ChestPool(ChestListSO chestListSO, ChestView chestView)
        {
            this.chestListSO = chestListSO;
            this.chestView = chestView;
        }

        public ChestController GetChest(ChestSlotUIController chestSlotUIController)
        {
            if (pooledChests.Count > 0)
            {
                PooledChest pooledChest = pooledChests.Find(item => !item.isUsed);

                if (pooledChest != null)
                {
                    pooledChest.isUsed = true;
                    return pooledChest.chest;
                }
            }
            return CreateChest(chestSlotUIController);
        }

        private ChestController CreateChest(ChestSlotUIController chestSlotUIController)
        {
            PooledChest pooledChest = new PooledChest();
            ChestView chestViewPrefab = Object.Instantiate(chestView);
            pooledChest.chest = new ChestController(chestListSO, chestViewPrefab, chestSlotUIController);
            pooledChest.isUsed = true;
            pooledChests.Add(pooledChest);

            return pooledChest.chest;
        }

        public void ReturnToPool(ChestController chestToReturn)
        {
            PooledChest pooledChest = pooledChests.Find(item => item.chest.Equals(chestToReturn));
            pooledChest.isUsed = false;
        }

        public class PooledChest
        {
            public ChestController chest;
            public bool isUsed;
        }
    }
}