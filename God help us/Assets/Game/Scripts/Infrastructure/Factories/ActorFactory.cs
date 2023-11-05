using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI;
using Game.Scripts.GameplayLogic.AI.Reporting;
using Game.Scripts.GameplayLogic.AI.UtilityAI;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Scripts.Infrastructure.Factories
{
    public class ActorFactory
    {
        private readonly IConfigProvider _configProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly ActorRegistry _actorRegistry;
        private readonly BuildingRegistry _buildingRegistry;
        private readonly JobController _jobController;
        private readonly AIReporter _aiReporter;

        public ActorFactory(IConfigProvider configProvider, 
            IAssetProvider assetProvider, 
            ActorRegistry actorRegistry,
            BuildingRegistry buildingRegistry,
            JobController jobController,
            AIReporter reporter)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
            _actorRegistry = actorRegistry;
            _buildingRegistry = buildingRegistry;
            _jobController = jobController;
            _aiReporter = reporter;
        }

        public void PreloadAssets()
        {
            
        }
        
        public async UniTask<Actor> CreateActor(ActorType actorType, Vector3 at)
        {
            ActorConfig config = _configProvider.GetDataForActor(actorType);
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(config.PrefabReference);
            Actor actor = _assetProvider.Instantiate(prefab, at).GetComponent<Actor>();
            string id = Guid.NewGuid().ToString();

            AIAgent agent = actor.GetComponent<AIAgent>();
            AIBrains brains = new AIBrains(_buildingRegistry);
            ActorData actorData = new ActorData
            {
                DefaultAction = Object.Instantiate(config.AIConfig.DefaultAction),
                Actions = new List<AIAction>(),
                Type = config.Type
            };

            foreach (AIAction action in config.AIConfig.Actions) 
                actorData.Actions.Add(Object.Instantiate(action));
            
            actor.Construct(actorData);
            actor.name = $"Actor {id}";
            agent.Construct(id, actorData, brains, _jobController, _buildingRegistry, _aiReporter);
            
            _actorRegistry.Register(id, actor);

            return actor;
        }
    }
}