using System;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Factories
{
    public class BuildingFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigProvider _configProvider;
        private readonly BuildingRegistry _buildingRegistry;

        public BuildingFactory(IAssetProvider assetProvider, IConfigProvider configProvider,
            BuildingRegistry buildingRegistry)
        {
            _assetProvider = assetProvider;
            _configProvider = configProvider;
            _buildingRegistry = buildingRegistry;
        } 
        
        public ProductionBuilding CreateProductionBuilding(ProductionCategory category, Vector3 position)
        {
            ProductionBuildingConfig config = _configProvider.GetConfigForProductionBuilding(category);
            ProductionBuilding productionBuilding = _assetProvider.Instantiate(config.Prefab, position);
            Building building = productionBuilding.GetComponent<Building>();

            string id = Guid.NewGuid().ToString();
            productionBuilding.Construct(config, building);
            building.Construct(id, config.BuildingConfig);

            _buildingRegistry.RegisterBuilding(id, building);
            _buildingRegistry.RegisterProductionBuilding(id, productionBuilding);

            return productionBuilding;
        }

        public Storage CreateStorage(ResourceType type, Vector3 position)
        {
            StorageConfig config = _configProvider.GetConfigForStorage(type);
            Storage storage = _assetProvider.Instantiate(config.Prefab, position);
            Building building = storage.GetComponent<Building>();
            
            string id = Guid.NewGuid().ToString();
            
            storage.Construct(config);
            building.Construct(id, config.BuildingConfig);
            
            _buildingRegistry.RegisterBuilding(id, building);
            _buildingRegistry.RegisterStorage(type, storage);

            return storage;
        }
    }
}