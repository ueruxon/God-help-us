using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Game.Scripts.GameplayLogic.Services.ActorRegistry;
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
            builder.Register<IActorRegistry, ActorRegistry>(Lifetime.Scoped);
            builder.Register<ActorFactory>(Lifetime.Scoped);
            builder.Register<ResourceFactory>(Lifetime.Scoped);

            builder.Register<JobSequencer>(Lifetime.Scoped);
            builder.Register<ITickable, ResourceCoordinator>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<GameplayInitializer>();
        }
    }
}