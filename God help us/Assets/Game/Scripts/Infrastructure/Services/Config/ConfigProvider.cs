using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Scripts.Data.Actors;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using UnityEngine;

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
        private Dictionary<ProductionCategory, ProductionBuildingConfig> _productionDataByType;

        public ConfigProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Init() => 
            await Load();

        public async UniTask Load()
        {
            List<ActorConfig> actorConfigs = await _assetProvider.LoadAssetsByLabel<ActorConfig>(AssetLabels.ActorConfig);
            _actorDataByType = actorConfigs.ToDictionary(x => x.Type, x => x);

            _resourceNodePointsConfig = await _assetProvider.LoadAsync<ResourceNodePointsConfig>(AssetKeys.ResourceNodesOnLevel);

            List<ResourceNodeConfig> nodeConfigs = await _assetProvider.LoadAssetsByLabel<ResourceNodeConfig>(AssetLabels.ResourceNodeConfig);
            _resourceNodeDataByType = nodeConfigs.ToDictionary(x => x.Type, x => x);

            List<ResourceConfig> resourceConfigs =
                await _assetProvider.LoadAssetsByLabel<ResourceConfig>(AssetLabels.ResourceConfig);
            _resourceDataByType = resourceConfigs
                .ToDictionary(x => x.Type, x => x);
            
            List<StorageConfig> storageConfigs = 
                await _assetProvider.LoadAssetsByLabel<StorageConfig>(AssetLabels.BuildingConfig);
            _storageDataByType = storageConfigs.ToDictionary(x => x.StoredType, x => x);

            List<ProductionBuildingConfig> productionBuildingConfigs =
                await _assetProvider.LoadAssetsByLabel<ProductionBuildingConfig>(AssetLabels.BuildingConfig);
            _productionDataByType = productionBuildingConfigs.ToDictionary(x => x.Category, x => x);
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

        public ProductionBuildingConfig GetConfigForProductionBuilding(ProductionCategory category)
        {
            if (_productionDataByType.TryGetValue(category, out ProductionBuildingConfig data))
                return data;
            
            throw new KeyNotFoundException($"No config found for {category}");
        }
    }
    
}