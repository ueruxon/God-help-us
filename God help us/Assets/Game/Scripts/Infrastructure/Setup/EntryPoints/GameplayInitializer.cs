using Game.Scripts.Data.Actors;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Game.Scripts.GameplayLogic.Services;
using Game.Scripts.Infrastructure.Factories;
using Game.Scripts.Infrastructure.Services.Config;
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

        public GameplayInitializer(ActorFactory actorFactory, ResourceCoordinator resourceCoordinator, 
            IConfigProvider configProvider, BuildingRegistry buildingRegistry)
        {
            _actorFactory = actorFactory;
            _resourceCoordinator = resourceCoordinator;
            _configProvider = configProvider;
            _buildingRegistry = buildingRegistry;
        }
        
        public void Initialize()
        {
            _resourceCoordinator.Init();
        }

        public void Start()
        {
            // for test
            for (int i = 0; i < 1; i++)
            {
                Actor actor = _actorFactory.CreateActor<Actor>(ActorType.Villager, new Vector3(0, 0, i + 2));
                actor.Init();
            }

            Storage[] storages = Object.FindObjectsOfType<Storage>();

            foreach (Storage storage in storages)
            {
                storage.Construct(_configProvider.GetConfigForStorage(ResourceType.Wood));
                _buildingRegistry.RegisterStorage(ResourceType.Wood, storage);
            }
        }
    }
}