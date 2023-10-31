using System.Collections.Generic;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.GameplayLogic.Level
{
    public class LevelLoop : ITickable
    {
        private readonly BuildingConstructor _buildingConstructor;

        public LevelLoop(BuildingConstructor buildingConstructor)
        {
            _buildingConstructor = buildingConstructor;
        }

        public void Init()
        {
            
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
                _buildingConstructor.CreateProductionBuilding(ProductionCategory.Lumber, new Vector3(20, 0, -20));
            }
        }
    }
}