using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.Data.ResourcesData
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Game/ResourcesData/ResourceConfig")]
    public class ResourceConfig : ScriptableObject
    {
        public Resource Prefab;
        public ResourceType Type;
        public Color Color;
    }
}