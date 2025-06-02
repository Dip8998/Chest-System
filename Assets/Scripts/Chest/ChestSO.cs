using UnityEngine;

namespace ChestSystem.Chest
{
    [CreateAssetMenu(fileName = "NewChestScriptableObject", menuName = "ScriptableObject/ChestSO")]
    public class ChestSO : ScriptableObject
    {
        public ChestType chestType;
        public Sprite chestSprite;
        public Sprite chestUnlockedSprite;
        public float chestTimer;
        public float chestGeneratingChance; 
        public int minCoinValue;
        public int maxCoinValue;
        public int minGemValue;
        public int maxGemValue;
    }

    public enum ChestType
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
}

