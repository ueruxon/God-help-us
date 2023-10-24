using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Registers
{
    public class BuildingRegistry
    {
        private readonly Dictionary<string, Building> _allBuildingsById;

        private readonly List<Storage> _allStorages;
        private readonly Dictionary<ResourceType, List<Storage>> _storagesByType;

        private readonly List<ProductionBuilding> _allProductionBuildings;
        private readonly Dictionary<string, ProductionBuilding> _productionBuildingById;
        private readonly Dictionary<string, ProductionBuilding> _requestedProductionBuildings;
        
        public BuildingRegistry()
        {
            _allBuildingsById = new Dictionary<string, Building>();

            _allStorages = new List<Storage>();
            _storagesByType = new Dictionary<ResourceType, List<Storage>>();
            foreach (ResourceType value in Enum.GetValues(typeof(ResourceType)))
                _storagesByType[value] = new List<Storage>();
            
            _allProductionBuildings = new List<ProductionBuilding>();
            _productionBuildingById = new Dictionary<string, ProductionBuilding>();
            _requestedProductionBuildings = new Dictionary<string, ProductionBuilding>();
        }

        public void RegisterBuilding(string id, Building building) => 
            _allBuildingsById.Add(id, building);

        public void RegisterStorage(ResourceType storedType, Storage storage)
        {
            _storagesByType[storedType].Add(storage);
            _allStorages.Add(storage);
        }

        public List<Storage> GetStorages(ResourceType storedType) =>
            _storagesByType[storedType];

        public List<Storage> GetStorages() => 
            _allStorages;

        public void RegisterProductionBuilding(string id, ProductionBuilding productionBuilding)
        {
            _allProductionBuildings.Add(productionBuilding);
            _productionBuildingById.Add(id, productionBuilding);
            
            productionBuilding.StateChanged += ProductionBuildingStateChanged;
        }

        public Building GetBuilding(string buildingId) => 
            _allBuildingsById[buildingId];

        public ProductionBuilding GetProductionBuilding(string id) => 
            _productionBuildingById[id];

        public List<ProductionBuilding> GetAllProductionBuildings() => 
            _allProductionBuildings;

        public bool CheckAnyAvailableProductionBuilding()
        {
            foreach (ProductionBuilding building in _allProductionBuildings)
                if (building.IsAvailable())
                    return true;

            return false;
        }

        public ProductionBuilding GetAnyProductionBuilding()
        {
            return _allProductionBuildings
                .First(x => x.IsAvailable());
        }

        public bool WorkerContains(string workerId) => 
            _requestedProductionBuildings.ContainsKey(workerId);

        public ProductionBuilding GetRequesterBuilding(string workerId) => 
            _requestedProductionBuildings[workerId];

        private void ProductionBuildingStateChanged(ProductionState state, ProductionBuilding building)
        {
            if (state is ProductionState.Requested) 
                _requestedProductionBuildings[building.GetWorkerId()] = building;

            if (state == ProductionState.Freely) 
                _requestedProductionBuildings.Remove(building.GetWorkerId());
        }

        public List<Building> GetAllBuildings()
        {
            List<Building> buildings = new List<Building>(_allBuildingsById.Values);
            return buildings;
        }
    }
}