using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using UnityEngine;

namespace Game.Scripts.Data.Buildings
{
    [CreateAssetMenu(fileName = "New Storage", menuName = "Game/BuildingsData/StorageConfig")]
    public class StorageConfig : ScriptableObject
    {
        public Storage Prefab;
        public Transform FillerPrefab;
        public ResourceType StoredType;
        
        public BuildingConfig BuildingConfig;
    }
}