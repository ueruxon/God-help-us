using Game.Scripts.Factories;
using Game.Scripts.GameplayLogic.AI.Reporting;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Level;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;
using Game.Scripts.UI;
using Game.Scripts.UI.DebugWindow;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Framework.Setup.Gameplay
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private UIRoot _uiRoot;
        [SerializeField] private AIDebugWindow _debugWindow;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Configure from GameplayScope");
            
            builder.Register<JobController>(Lifetime.Scoped);
            builder.Register<JobFactory>(Lifetime.Scoped);
            
            builder.Register<ResourceFactory>(Lifetime.Scoped);
            builder.Register<ITickable, ResourceCoordinator>(Lifetime.Scoped).AsSelf();;
            
            builder.Register<BuildingFactory>(Lifetime.Scoped);
            builder.Register<BuildingRegistry>(Lifetime.Scoped);
            builder.Register<BuildingConstructor>(Lifetime.Scoped);
            builder.Register<BuildingResolver>(Lifetime.Scoped);

            builder.Register<ActorRegistry>(Lifetime.Scoped);
            builder.Register<AIReporter>(Lifetime.Scoped);
            builder.Register<ActorFactory>(Lifetime.Scoped);
            
            builder.Register<ITickable, LevelLoop>(Lifetime.Scoped).AsSelf();

            builder.RegisterComponent(_uiRoot);
            builder.RegisterComponentInNewPrefab(_debugWindow, Lifetime.Scoped).UnderTransform(_uiRoot.transform);

            builder.RegisterEntryPoint<GameplayFlow>();
        }
    }
}