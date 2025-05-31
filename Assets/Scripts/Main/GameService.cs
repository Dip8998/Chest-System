using ChestSystem.Chest;
using ChestSystem.UI;
using ChestSystem.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public ChestService chestService {  get; private set; }
        public UIService uiService => _uiService;

        [SerializeField] private ChestListSO chestListSO;
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private UIService _uiService;

        protected override void Awake()
        {
            base.Awake();
            chestService = new ChestService(chestListSO, chestPrefab);
        }
    }
}

