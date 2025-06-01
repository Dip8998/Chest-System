using ChestSystem.Chest;
using ChestSystem.Command;
using ChestSystem.Resource;
using ChestSystem.UI;
using ChestSystem.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public ChestService chestService {  get; private set; }
        public ResourceService resourceService { get; private set; }
        public CommandInvoker commandInvoker { get; private set; }
        public UIService uiService => _uiService;

        [SerializeField] private ChestListSO chestListSO;
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private UIService _uiService;
        [SerializeField] private ResourceView resourceView;

        protected override void Awake()
        {
            base.Awake();
            resourceService = new ResourceService(resourceView);
            chestService = new ChestService(chestListSO, chestPrefab);
            commandInvoker = new CommandInvoker(resourceService);
        }
    }
}

