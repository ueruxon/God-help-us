using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.Actors;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.Infrastructure.Services.AssetManagement;

namespace Game.Scripts.Infrastructure.Services.Config
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<ActorType, ActorConfig> _actorDataByType;

        private ResourceNodePointsConfig _resourceNodePointsConfig;
        private Dictionary<ResourceType, ResourceNodeConfig> _resourceNodeDataByType;
        private Dictionary<ResourceType, ResourceConfig> _resourceDataByType;

        private Dictionary<ResourceType, StorageConfig> _storageDataByType;

        public ConfigProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public void Load()
        {
            _actorDataByType = _assetProvider
                .LoadAll<ActorConfig>(AssetPath.ActorsDataPath)
                .ToDictionary(x => x.Type, x => x);

            _resourceNodePointsConfig = _assetProvider.Load<ResourceNodePointsConfig>(AssetPath.ResourceNodePointsConfigPath);
            _resourceNodeDataByType = _assetProvider
                .LoadAll<ResourceNodeConfig>(AssetPath.ResourceConfigsPath)
                .ToDictionary(x => x.Type, x => x);
            _resourceDataByType = _assetProvider
                .LoadAll<ResourceConfig>(AssetPath.ResourceConfigsPath)
                .ToDictionary(x => x.Type, x => x);

            _storageDataByType = _assetProvider
                .LoadAll<StorageConfig>(AssetPath.StorageConfigsPath)
                .ToDictionary(x => x.StoredType, x => x);
        }

        public ActorConfig GetDataForActor(ActorType type)
        {
            if (_actorDataByType.TryGetValue(type, out ActorConfig data))
                return data;
            
            throw new KeyNotFoundException($"No config found for {type}");
        }
        
        public ResourceNodeConfig GetDataForResourceNode(ResourceType type)
        {
            if (_resourceNodeDataByType.TryGetValue(type, out ResourceNodeConfig data))
                return data;
            
            throw new KeyNotFoundException($"No config found for {type}");
        }

        public ResourceConfig GetDataForResource(ResourceType type)
        {
            if (_resourceDataByType.TryGetValue(type, out ResourceConfig data))
                return data;
            
            throw new KeyNotFoundException($"No config found for {type}");
        }

        public ResourceNodePointsConfig GetResourcesNodeConfigOnLevel()
        {
            if (_resourceNodePointsConfig is not null)
                return _resourceNodePointsConfig;

            throw new KeyNotFoundException($"No config found for ResourceNodePointsConfig");
        }

        public StorageConfig GetConfigForStorage(ResourceType type)
        {
            if (_storageDataByType.TryGetValue(type, out StorageConfig data))
                return data;
            
            throw new KeyNotFoundException($"No config found for {type}");
        }
    }
    
}