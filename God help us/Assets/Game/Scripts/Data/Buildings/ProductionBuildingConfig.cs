using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using UnityEngine;

namespace Game.Scripts.Data.Buildings
{
    [CreateAssetMenu(fileName = "New Production Building", menuName = "Game/BuildingsData/ProductionConfig")]
    public class ProductionBuildingConfig : ScriptableObject
    {
        public ProductionBuilding Prefab;
        public ProductionCategory Category;
        public ResourceType ProducedType;

        public float TimeToSpawnResource = 10f;
        
        public BuildingData BuildingData;
    }
}