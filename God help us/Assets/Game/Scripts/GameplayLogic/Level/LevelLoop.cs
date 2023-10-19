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
        private readonly BuildingConstructor _buildingConstructor;
        private readonly ResourceCoordinator _resourceCoordinator;
        private readonly JobController _jobController;
        private readonly JobFactory _jobFactory;

        public LevelLoop(BuildingRegistry buildingRegistry,
            BuildingConstructor buildingConstructor,
            ResourceCoordinator resourceCoordinator, 
            JobController jobController, 
            JobFactory jobFactory)
        {
            _buildingRegistry = buildingRegistry;
            _buildingConstructor = buildingConstructor;
            _resourceCoordinator = resourceCoordinator;
            _jobController = jobController;
            _jobFactory = jobFactory;
        }

        public void Init()
        {
            _resourceCoordinator.ResourceSpawned += ResourceWasSpawned;
            _buildingConstructor.ProductionBuildingCreated += ProductionBuildingWasCreated;
            _buildingConstructor.StorageCreated += StorageWasCreated;
        }

        private void ResourceWasSpawned()
        {
            Queue<Resource> resourcesOnLevel = _resourceCoordinator.GetAllResourcesOnLevel();
            int resourceCount = resourcesOnLevel.Count;

            for (int i = 0; i < resourceCount; i++)
            {
                Resource resource = resourcesOnLevel.Dequeue();
                
                if (RegisterResourceInStorage(resource) == false) 
                    resourcesOnLevel.Enqueue(resource);
            }
        }

        private void StorageWasCreated()
        {
            Queue<Resource> resourcesOnLevel = _resourceCoordinator.GetAllResourcesOnLevel();
            int resourceCount = resourcesOnLevel.Count;

            if (resourceCount > 0)
            {
                for (int i = 0; i < resourceCount; i++)
                {
                    Resource resource = resourcesOnLevel.Dequeue();
                
                    if (RegisterResourceInStorage(resource) == false) 
                        resourcesOnLevel.Enqueue(resource);
                }
            }
        }

        private void ProductionBuildingWasCreated(string buildingId)
        {
            ProductionBuilding building = _buildingRegistry.GetProductionBuilding(buildingId);
            building.ResourceCanSpawn += SpawnResource;
        }

        private void SpawnResource(ResourceType type, Vector3 position) => 
            _resourceCoordinator.SpawnResource(type, position);

        private bool RegisterResourceInStorage(Resource resource)
        {
            List<Storage> storages = _buildingRegistry.GetStorages(resource.Type);
            
            foreach (Storage storage in storages)
            {
                if (storage.IsFull() == false)
                {
                    if (storage.RegisterResource(resource))
                    {
                        _jobController.AddJob(_jobFactory.CreateJob(JobCategory.Collect, resource));
                        return true;
                    }
                }
            }

            return false;
        }

        public void Cleanup()
        {
            _resourceCoordinator.ResourceSpawned -= ResourceWasSpawned;
            _buildingConstructor.ProductionBuildingCreated -= ProductionBuildingWasCreated;
            _buildingConstructor.StorageCreated -= StorageWasCreated;

            foreach (ProductionBuilding building in _buildingRegistry.GetAllProductionBuildings())
                building.ResourceCanSpawn -= SpawnResource;
        }

        public void Tick()
        {
            // for test

            if (Input.GetKeyDown(KeyCode.G))
            {
                _buildingConstructor.CreateStorage(ResourceType.Wood, new Vector3(-10, 0, -10));
            }
            
            if (Input.GetKeyDown(KeyCode.H))
            {
                _buildingConstructor.CreateProductionBuilding(ProductionCategory.Lumber, new Vector3(10, 0, -10));
            }
        }
    }
}