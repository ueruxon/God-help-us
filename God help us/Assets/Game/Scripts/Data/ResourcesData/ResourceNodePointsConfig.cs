using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Data.ResourcesData
{
    [CreateAssetMenu(fileName = "Resources Points on Level", menuName = "Game/ResourcesData/ResourceNodePoints")]
    public class ResourceNodePointsConfig : ScriptableObject
    {
        public List<ResourcePointData> ResourceNodePoints;

        [Button]
        public void CollectNodes()
        {
            ResourceNodePoints = FindObjectsOfType<ResourceNodeMarker>()
                .Select(x =>
                    new ResourcePointData(x.Type, x.transform.position, x.transform))
                .ToList();;
        }
    }

    [Serializable]
    public class ResourcePointData
    {
        public ResourceType Type;
        public Vector3 Position;
        public Transform Container;

        public ResourcePointData(ResourceType type, Vector3 spawnerPosition, Transform container)
        {
            Type = type;
            Position = spawnerPosition;
            Container = container;
        }
    }
}