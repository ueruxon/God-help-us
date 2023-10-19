using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.Data.ResourcesData
{
    [CreateAssetMenu(fileName = "New ResourceNode Config", menuName = "Game/ResourcesData/ResourceNodeConfig")]
    public class ResourceNodeConfig : ScriptableObject
    {
        public ResourceNode Prefab;
        public ResourceType Type;
        [Range(1, 10)] public int HitToDestroy = 3;
        
        [Range(20, 200)]
        [SerializeField] private int _maxTimeToRespawn = 40;
        [Range(20, 200)]
        [SerializeField] private int _minToRespawn = 20;
        
        public int TimeToRespawn => Random.Range(_minToRespawn, _maxTimeToRespawn);
    }
}