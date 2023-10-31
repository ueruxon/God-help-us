using System;
using System.Collections.Generic;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public class BuildingResolver
    {
        private readonly BuildingRegistry _buildingRegistry;
        private readonly BuildingConstructor _buildingConstructor;
        private readonly ResourceCoordinator _resourceCoordinator;
        private readonly JobFactory _jobFactory;
        private readonly JobController _jobController;

        public BuildingResolver(BuildingRegistry buildingRegistry, 
            BuildingConstructor buildingConstructor,
            ResourceCoordinator resourceCoordinator, JobFactory jobFactory, JobController jobController)
        {
            _buildingRegistry = buildingRegistry;
            _buildingConstructor = buildingConstructor;
            _resourceCoordinator = resourceCoordinator;
            _jobFactory = jobFactory;
            _jobController = jobController;
        }

        public void Init()
        {
            _buildingConstructor.BuildingReleased += OnBuildingReleased;
            _resourceCoordinator.ResourceSpawned += OnResourceSpawned;
        }
        
        private void OnBuildingReleased(string buildingId)
        {
            Building building = _buildingRegistry.GetBuilding(buildingId);
            List<Storage> storages = _buildingRegistry.GetStorages();

            foreach (Storage storage in storages) 
                ResolveBuilding(building, storage);

            _resourceCoordinator.RegisterResources(building);

            building.Constructed += OnBuildingConstructed;
        }

        private void ResolveBuilding(Building building, Storage storage)
        {
            if (building.IsValidType(storage.GetStoredType()))
            {
                while (storage.CanRequestResource())
                {
                    Resource incoming = storage.RequestResource();

                    if (!building.Register(incoming))
                        break;

                    storage.Resolve();
                    _jobController.AddJob(_jobFactory.CreateJob(JobCategory.Construct, storage));
                }
            }
        }

        private void OnBuildingConstructed(string buildingId)
        {
            Building building = _buildingRegistry.GetBuilding(buildingId);
            building.Constructed -= OnBuildingConstructed;

            Resolve(building.GetCategory(), buildingId);
        }

        private void Resolve(BuildingCategory category, string buildingId)
        {
            switch (category)
            {
                case BuildingCategory.Storage:
                    Storage storage = _buildingRegistry.GetStorage(buildingId);
                    storage.ResourceDelivered += OnStorageResourceDelivered;
                    OnResourceSpawned(storage.GetStoredType());
                    break;
                case BuildingCategory.ProductionBuilding:
                    ProductionBuilding building = _buildingRegistry.GetProductionBuilding(buildingId);
                    building.ResourceCanSpawn += SpawnResource;
                    break;
                case BuildingCategory.House:
                    break;
            }
        }

        private void OnResourceSpawned(ResourceType type)
        {
            List<Building> buildings = _buildingRegistry.GetAllBuildings();
            foreach (Building building in buildings)
            {
                if (building.IsReleased()) 
                    _resourceCoordinator.RegisterResources(building);
            }

            List<Storage> storages = _buildingRegistry.GetStorages(type);
            foreach (Storage storage in storages)
            {
                if (!storage.IsFull()) 
                    _resourceCoordinator.RegisterResources(storage);
            }
        }

        private void SpawnResource(ResourceType type, Vector3 position) =>
            _resourceCoordinator.SpawnResource(type, position);

        private void OnStorageResourceDelivered(string storageId)
        {
            List<Building> buildings = _buildingRegistry.GetAllBuildings();
            
            foreach (Building building in buildings)
            {
                if (building.IsReleased())
                {
                    Storage storage = _buildingRegistry.GetStorage(storageId);
                    ResolveBuilding(building, storage);
                }
            }
        }

        public void Cleanup()
        {
            foreach (Storage storage in _buildingRegistry.GetStorages()) 
                storage.ResourceDelivered -= OnStorageResourceDelivered;

            foreach (ProductionBuilding building in _buildingRegistry.GetAllProductionBuildings())
                building.ResourceCanSpawn -= SpawnResource;
        }
    }
}