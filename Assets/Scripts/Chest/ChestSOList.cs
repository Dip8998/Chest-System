using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Chest
{
    [CreateAssetMenu(fileName = "NewChestScriptableObject", menuName = "ScriptableObject/ChestListSO")]
    public class ChestListSO : ScriptableObject
    {
        public List<ChestSO> chestList;
    }
}