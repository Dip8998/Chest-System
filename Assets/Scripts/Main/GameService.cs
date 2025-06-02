using ChestSystem.Chest;
using ChestSystem.Command;
using ChestSystem.Event;
using ChestSystem.Resource;
using ChestSystem.Service;
using ChestSystem.Sound;
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
        public EventService eventService { get; private set; } 
        public UIService uiService => _uiService;
        public SoundService soundService => _soundService;

        [SerializeField] private ChestListSO chestListSO;
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private UIService _uiService;
        [SerializeField] private SoundService _soundService;
        [SerializeField] private ResourceView resourceView;

        protected override void Awake()
        {
            base.Awake();
            eventService = new EventService();
            resourceService = new ResourceService(resourceView);
            chestService = new ChestService(chestListSO, chestPrefab);
            commandInvoker = new CommandInvoker(resourceService);
        }
    }
}

