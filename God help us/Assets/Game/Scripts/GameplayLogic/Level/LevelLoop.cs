using System.Collections.Generic;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.GameplayLogic.Level
{
    public class LevelLoop : ITickable
    {
        private readonly BuildingRegistry _buildingRegistry;
        private readonly BuildingCoordinator _buildingCoordinator;
        private readonly ResourceCoordinator _resourceCoordinator;
        private readonly JobController _jobController;
        private readonly JobFactory _jobFactory;

        public LevelLoop(BuildingRegistry buildingRegistry,
            BuildingCoordinator buildingCoordinator,
            ResourceCoordinator resourceCoordinator,
            JobController jobController,
            JobFactory jobFactory)
        {
            _buildingRegistry = buildingRegistry;
            _buildingCoordinator = buildingCoordinator;
            _resourceCoordinator = resourceCoordinator;
            _jobController = jobController;
            _jobFactory = jobFactory;
        }

        public void Init()
        {
            _resourceCoordinator.ResourceSpawned += RegisterResourceInStorage;
            _buildingCoordinator.ProductionBuildingCreated += ProductionBuildingWasCreated;
            _buildingCoordinator.StorageCreated += RegisterResourceInStorage;
        }
        

        private void RegisterResourceInStorage()
        {
            List<Storage> storages = _buildingRegistry.GetStorages();

            foreach (Storage storage in storages)
            {
                if (!storage.IsFull()) 
                    _resourceCoordinator.RegisterResources(storage);
            }
        }

        private void ProductionBuildingWasCreated(string buildingId)
        {
            ProductionBuilding building = _buildingRegistry.GetProductionBuilding(buildingId);
            building.ResourceCanSpawn += SpawnResource;
        }

        private void SpawnResource(ResourceType type, Vector3 position) =>
            _resourceCoordinator.SpawnResource(type, position);
        

        public void Cleanup()
        {
            _resourceCoordinator.ResourceSpawned -= RegisterResourceInStorage;
            _buildingCoordinator.ProductionBuildingCreated -= ProductionBuildingWasCreated;
            _buildingCoordinator.StorageCreated -= RegisterResourceInStorage;

            foreach (ProductionBuilding building in _buildingRegistry.GetAllProductionBuildings())
                building.ResourceCanSpawn -= SpawnResource;
        }

        public void Tick()
        {
            // for test

            if (Input.GetKeyDown(KeyCode.G))
            {
                _buildingCoordinator.CreateStorage(ResourceType.Wood, new Vector3(-10, 0, -10));
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                _buildingCoordinator.CreateProductionBuilding(ProductionCategory.Lumber, new Vector3(20, 0, -20));
            }
        }
    }
}