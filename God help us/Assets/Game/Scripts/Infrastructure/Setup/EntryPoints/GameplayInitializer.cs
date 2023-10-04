using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Game.Scripts.Infrastructure.Factories;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure.Setup.EntryPoints
{
    public class GameplayInitializer : IInitializable, IStartable
    {
        private readonly ActorFactory _actorFactory;
        private readonly ResourceCoordinator _resourceCoordinator;

        public GameplayInitializer(ActorFactory actorFactory, ResourceCoordinator resourceCoordinator)
        {
            _actorFactory = actorFactory;
            _resourceCoordinator = resourceCoordinator;
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
                Villager actor = _actorFactory.CreateActor<Villager>(ActorType.Villager, new Vector3(0, 0, i + 2));
                actor.gameObject.name = $"Actor {i}";
                actor.Init();
            }
        }
    }
}