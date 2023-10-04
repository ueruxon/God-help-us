using System.Collections.Generic;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI;
using Game.Scripts.GameplayLogic.AI.Actions;
using Game.Scripts.GameplayLogic.AI.UtilityAI;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Services.ActorRegistry;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Factories
{
    public class ActorFactory
    {
        private readonly IConfigProvider _configProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly IActorRegistry _actorRegistry;
        private readonly JobSequencer _jobSequencer;

        public ActorFactory(IConfigProvider configProvider, IAssetProvider assetProvider, 
            IActorRegistry actorRegistry,
            JobSequencer jobSequencer)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
            _actorRegistry = actorRegistry;
            _jobSequencer = jobSequencer;
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

            foreach (AIAction action in config.AIConfig.Actions) 
                actorData.Actions.Add(Object.Instantiate(action));

            actor.Construct(actorData);
            agent.Construct(actorData, brains, _jobSequencer);
            
            _actorRegistry.Register(actor);

            return actor as T;
        }
    }
}