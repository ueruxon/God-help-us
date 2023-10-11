using System;
using System.Collections.Generic;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;

namespace Game.Scripts.GameplayLogic.Services
{
    public class BuildingRegistry
    {
        private readonly Dictionary<ResourceType, List<Storage>> _storagesByType;

        public BuildingRegistry()
        {
            _storagesByType = new Dictionary<ResourceType, List<Storage>>();
            
            foreach (ResourceType value in Enum.GetValues(typeof(ResourceType)))
                _storagesByType[value] = new List<Storage>();
        }

        public void RegisterStorage(ResourceType storedType, Storage storage) => 
            _storagesByType[storedType].Add(storage);

        public List<Storage> GetStorages(ResourceType storedType) => 
            _storagesByType[storedType];
    }
}