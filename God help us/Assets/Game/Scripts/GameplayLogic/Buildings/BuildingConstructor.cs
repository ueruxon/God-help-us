using System;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.Infrastructure.Factories;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public class BuildingConstructor
    {
        public event Action<string> BuildingReleased;

        private readonly BuildingFactory _buildingFactory;

        public BuildingConstructor(BuildingFactory buildingFactory)
        {
            _buildingFactory = buildingFactory;
        }

        //todo
        public void CreateProductionBuilding(ProductionCategory category, Vector3 position)
        {
            Building building = _buildingFactory.CreateProductionBuilding(category, position);
            building.Prepare();

            BuildingReleased?.Invoke(building.Id);
        }

        //todo
        public void CreateStorage(ResourceType type, Vector3 position)
        {
            Building building = _buildingFactory.CreateStorage(type, position);
            building.Prepare();

            BuildingReleased?.Invoke(building.Id);
        }
    }
}