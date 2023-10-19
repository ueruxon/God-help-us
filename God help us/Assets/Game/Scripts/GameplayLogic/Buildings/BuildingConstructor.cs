﻿using System;
using System.Collections.Generic;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;
using Game.Scripts.Infrastructure.Factories;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public class BuildingConstructor
    {
        public event Action<string> ProductionBuildingCreated;
        public event Action StorageCreated; 

        private readonly BuildingRegistry _buildingRegistry;
        private readonly BuildingFactory _buildingFactory;
        private readonly JobFactory _jobFactory;
        private readonly JobController _jobController;

        public BuildingConstructor(BuildingRegistry buildingRegistry, 
            BuildingFactory buildingFactory,
            JobFactory jobFactory,
            JobController jobController)
        {
            _buildingRegistry = buildingRegistry;
            _buildingFactory = buildingFactory;
            _jobFactory = jobFactory;
            _jobController = jobController;
        }

        //todo
        public void CreateProductionBuilding(ProductionCategory category, Vector3 position)
        {
            ProductionBuilding prBuilding = _buildingFactory.CreateProductionBuilding(category, position);
            Building building = prBuilding.GetBuilding();
            building.Released += OnBuildingReleased;
            building.Constructed += OnBuildingConstructed;
            
            building.Prepare();
        }

        //todo

        public void CreateStorage(ResourceType type, Vector3 position)
        {
            _buildingFactory.CreateStorage(type, position);
            
            StorageCreated?.Invoke();
        }

        private void OnBuildingConstructed(string buildingId)
        {
            Building building = _buildingRegistry.GetBuilding(buildingId);
            building.Constructed -= OnBuildingConstructed;
            
            ProductionBuildingCreated?.Invoke(building.Id);
        }

        private void OnBuildingReleased(string buildingId)
        {
            Building building = _buildingRegistry.GetBuilding(buildingId);
            List<ConstructionData> requiredData = building.GetRequiredConstructionData();
            
            foreach (ConstructionData data in requiredData)
            {
                int remainingCount = data.ResourceAmount;
                
                Debug.Log($"RemainingCount: {remainingCount}");
                
                List<Storage> storages = _buildingRegistry.GetStorages(data.Type);
                
                if (storages.Count == 0)
                    continue;

                foreach (Storage storage in storages)
                {
                    while (storage.CanRequestResource() && remainingCount > 0)
                    {
                        remainingCount--;
                        Resource resource = storage.RequestResource();

                        _jobController.AddJob(_jobFactory.CreateJob(JobCategory.Collect, resource));
                    }
                }
            }
        }
    }
}