using Game.Scripts.Data.Actors;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.Level;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;
using Game.Scripts.Infrastructure.Factories;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.UI.DebugWindow;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure.Setup.EntryPoints
{
    public class GameplayInitializer : IInitializable, IStartable
    {
        private readonly ActorFactory _actorFactory;
        private readonly ResourceCoordinator _resourceCoordinator;
        private readonly IConfigProvider _configProvider;
        private readonly BuildingRegistry _buildingRegistry;
        private readonly AIDebugWindow _debugWindow;
        private readonly LevelLoop _levelLoop;

        public GameplayInitializer(ActorFactory actorFactory, 
            ResourceCoordinator resourceCoordinator, 
            IConfigProvider configProvider, 
            BuildingRegistry buildingRegistry,
            AIDebugWindow debugWindow,
            LevelLoop levelLoop)
        {
            _actorFactory = actorFactory;
            _resourceCoordinator = resourceCoordinator;
            _configProvider = configProvider;
            _buildingRegistry = buildingRegistry;
            _levelLoop = levelLoop;
            _debugWindow = debugWindow;
        }
        
        public void Initialize()
        {
            _resourceCoordinator.Init();
            _debugWindow.Init();
            _levelLoop.Init();
        }

        public void Start()
        {
            // for test
            for (int i = 0; i < 2; i++)
            {
                Actor actor = _actorFactory.CreateActor(ActorType.Villager, new Vector3(0, 0, i + 2));
                actor.Init();
            }

            // Storage[] storages = Object.FindObjectsOfType<Storage>();
            //
            // foreach (Storage storage in storages)
            // {
            //     storage.Construct(_configProvider.GetConfigForStorage(ResourceType.Wood));
            //     _buildingRegistry.RegisterStorage(ResourceType.Wood, storage);
            // }
            //
            // ProductionBuilding[] buildings = Object.FindObjectsOfType<ProductionBuilding>();
            //
            // foreach (ProductionBuilding building in buildings)
            // {
            //     building.Construct(_configProvider.GetConfigForProductionBuilding(ProductionCategory.Lumber));
            //     _buildingRegistry.RegisterProductionBuilding(ProductionCategory.Lumber, building);
            // }
        }
    }
}