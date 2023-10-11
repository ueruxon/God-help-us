using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Services;
using Game.Scripts.Infrastructure.Factories;
using Game.Scripts.Infrastructure.Setup.EntryPoints;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure.Setup.Scopes
{
    public class LevelScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ResourceFactory>(Lifetime.Scoped);

            builder.Register<BuildingFactory>(Lifetime.Scoped);
            builder.Register<BuildingRegistry>(Lifetime.Scoped);
            
            builder.Register<ActorRegistry>(Lifetime.Scoped);
            builder.Register<ActorFactory>(Lifetime.Scoped);
            
            builder.Register<JobController>(Lifetime.Scoped);
            builder.Register<JobFactory>(Lifetime.Scoped);
            builder.Register<ITickable, ResourceCoordinator>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<GameplayInitializer>();
        }
    }
}