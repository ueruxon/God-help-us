using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Data.ResourcesData
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Game/ResourcesData/ResourceConfig")]
    public class ResourceConfig : ScriptableObject
    {
        public AssetReference PrefabReference;
        public ResourceType Type;
        public Color Color;
    }
}