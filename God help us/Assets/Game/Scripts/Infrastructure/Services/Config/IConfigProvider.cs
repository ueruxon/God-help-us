using Game.Scripts.Data.Actors;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;

namespace Game.Scripts.Infrastructure.Services.Config
{
    public interface IConfigProvider
    {
        public void Load();
        public ActorConfig GetDataForActor(ActorType actorType);
        public ResourceNodeConfig GetDataForResourceNode(ResourceType type);
        public ResourceConfig GetDataForResource(ResourceType type);
        public ResourceNodePointsConfig GetResourcesNodeConfigOnLevel();
        public StorageConfig GetConfigForStorage(ResourceType type);
        public ProductionBuildingConfig GetConfigForProductionBuilding(ProductionCategory category);
    }
}