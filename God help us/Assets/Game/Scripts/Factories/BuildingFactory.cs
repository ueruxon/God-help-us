using System;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.Framework.Services.AssetManagement;
using Game.Scripts.Framework.Services.Config;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.Registers;
using UnityEngine;

namespace Game.Scripts.Factories
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
        
        public Building CreateProductionBuilding(ProductionCategory category, Vector3 position)
        {
            ProductionBuildingConfig config = _configProvider.GetConfigForProductionBuilding(category);
            ProductionBuilding productionBuilding = _assetProvider.Instantiate(config.Prefab, position);
            Building building = productionBuilding.GetComponent<Building>();

            string id = Guid.NewGuid().ToString();
            building.Construct(id, config.BuildingData);
            productionBuilding.Construct(config, building);

            _buildingRegistry.RegisterBuilding(id, building);
            _buildingRegistry.RegisterProductionBuilding(id, productionBuilding);

            return building;
        }

        public Building CreateStorage(ResourceType type, Vector3 position)
        {
            StorageConfig config = _configProvider.GetConfigForStorage(type);
            Storage storage = _assetProvider.Instantiate(config.Prefab, position);
            Building building = storage.GetComponent<Building>();
            
            string id = Guid.NewGuid().ToString();

            building.Construct(id, config.BuildingData);
            storage.Construct(config, building);

            _buildingRegistry.RegisterBuilding(id, building);
            _buildingRegistry.RegisterStorage(id, type, storage);

            return building;
        }
    }
}