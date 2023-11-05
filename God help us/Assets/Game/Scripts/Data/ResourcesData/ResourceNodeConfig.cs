using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Data.ResourcesData
{
    [CreateAssetMenu(fileName = "New ResourceNode Config", menuName = "Game/ResourcesData/ResourceNodeConfig")]
    public class ResourceNodeConfig : ScriptableObject
    {
        public AssetReference PrefabReference;
        public ResourceType Type;
        [Range(1, 10)] public int HitToDestroy = 3;

        [Range(5, 200)]
        [SerializeField] private int _minToRespawn = 20;
        [Range(5, 200)]
        [SerializeField] private int _maxTimeToRespawn = 40;

        public int TimeToRespawn => Random.Range(_minToRespawn, _maxTimeToRespawn);
    }
}