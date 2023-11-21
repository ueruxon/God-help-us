using Cysharp.Threading.Tasks;
using Game.Scripts.Data.Actors;
using Game.Scripts.Factories;
using Game.Scripts.Framework.Services.Config;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.Level;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;
using Game.Scripts.UI.DebugWindow;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Framework.Setup.Gameplay
{
    public class GameplayFlow : IInitializable
    {
        private readonly ActorFactory _actorFactory;
        private readonly ResourceCoordinator _resourceCoordinator;
        private readonly IConfigProvider _configProvider;
        private readonly BuildingRegistry _buildingRegistry;
        private readonly BuildingResolver _buildingResolver;
        private readonly AIDebugWindow _debugWindow;
        private readonly LevelLoop _levelLoop;

        public GameplayFlow(ActorFactory actorFactory, 
            ResourceCoordinator resourceCoordinator, 
            IConfigProvider configProvider, 
            BuildingRegistry buildingRegistry,
            BuildingResolver buildingResolver,
            AIDebugWindow debugWindow,
            LevelLoop levelLoop)
        {
            _actorFactory = actorFactory;
            _resourceCoordinator = resourceCoordinator;
            _configProvider = configProvider;
            _buildingRegistry = buildingRegistry;
            _buildingResolver = buildingResolver;
            _levelLoop = levelLoop;
            _debugWindow = debugWindow;
        }
        
        public void Initialize() => 
            InitGameSystems().Forget();

        private async UniTaskVoid InitGameSystems()
        {
            await _resourceCoordinator.Init();
            await InitTestActors();
            
            _buildingResolver.Init();
            _debugWindow.Init();
            _levelLoop.Init();
        }
        
        private async UniTask InitTestActors()
        {
            for (int i = 0; i < 2; i++)
            {
                Actor actor = await _actorFactory.CreateActor(ActorType.Villager, new Vector3(0, 0, i + 2));
                actor.Init();
            }
        }
    }
}