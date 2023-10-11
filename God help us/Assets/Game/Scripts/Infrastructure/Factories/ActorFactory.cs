using System;
using System.Collections.Generic;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI;
using Game.Scripts.GameplayLogic.AI.UtilityAI;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Services;
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

        public ActorFactory(IConfigProvider configProvider, 
            IAssetProvider assetProvider, 
            ActorRegistry actorRegistry,
            BuildingRegistry buildingRegistry,
            JobController jobController)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
            _actorRegistry = actorRegistry;
            _buildingRegistry = buildingRegistry;
            _jobController = jobController;
        }
        
        public T CreateActor<T>(ActorType actorType, Vector3 at) where T : Actor
        {
            ActorConfig config = _configProvider.GetDataForActor(actorType);
            Actor actor = _assetProvider.Instantiate(config.Prefab, at);

            AIAgent agent = actor.GetComponent<AIAgent>();
            AIBrains brains = new AIBrains();
            ActorData actorData = new ActorData
            {
                DefaultAction = Object.Instantiate(config.AIConfig.DefaultAction),
                Actions = new List<AIAction>(),
                Type = config.Type
            };

            string id = Guid.NewGuid().ToString();

            foreach (AIAction action in config.AIConfig.Actions) 
                actorData.Actions.Add(Object.Instantiate(action));

            actor.Construct(actorData);
            actor.name = $"Actor {id}";
            agent.Construct(id, actorData, brains, _jobController, _buildingRegistry);
            
            _actorRegistry.Register(id, actor);

            return actor as T;
        }
    }
}