using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ChestSystem.Chest
{
    public class ChestModel
    {
        private Dictionary<ChestType, ChestSO> chestTypeData;
        private ChestType currentChestType;

        public ChestModel(ChestListSO chestListSO)
        {
            InitializeChestData(chestListSO);
        }

        private void InitializeChestData(ChestListSO chestListSO)
        {
            chestTypeData = new Dictionary<ChestType, ChestSO>();

            foreach (ChestSO chestSO in chestListSO.chestList)
            {
                if (chestSO == null) continue;

                if (!chestTypeData.ContainsKey(chestSO.chestType)) 
                {
                    chestTypeData.Add(chestSO.chestType, chestSO);
                }
            }
        }

        public Sprite GetChestImage(ChestType chestType)
        {
            if(chestTypeData.TryGetValue(chestType, out ChestSO so))
            {
                return so.chestSprite;
            } 
            return null;
        }

        public Dictionary<ChestType, float> GetChestTypeByChance()
        {
            Dictionary<ChestType, float> chance = new Dictionary<ChestType, float>();
            foreach(var entry in chestTypeData)
            {
                chance.Add(entry.Key, entry.Value.chestGeneratingChance);
            }
            return chance;
        }

        public void SetCurrentChestType(ChestType chestType) => currentChestType = chestType;

    }
}

    

