using Game.Scripts.Data.Actors;
using Game.Scripts.Data.ResourcesData;

namespace Game.Scripts.Infrastructure.Services.Config
{
    public interface IConfigProvider
    {
        public void Load();
        public ActorConfig GetDataForActor(ActorType actorType);
        public ResourceNodeConfig GetDataForResourceNode(ResourceType type);
        public ResourceNodePointsConfig GetResourcesNodeConfigOnLevel();
    }
}