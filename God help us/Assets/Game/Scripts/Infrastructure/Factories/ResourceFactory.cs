using System;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.ResourceManagement;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Infrastructure.Factories
{
    public class ResourceFactory
    {
        private readonly IConfigProvider _configProvider;
        private readonly IAssetProvider _assetProvider;

        public ResourceFactory(IConfigProvider configProvider, IAssetProvider assetProvider)
        {
            _configProvider = configProvider;
            _assetProvider = assetProvider;
        }

        public ResourceNode CreateResourceNode(ResourceType type, Vector3 at)
        {
            ResourceNodeConfig data = _configProvider.GetDataForResourceNode(type);
            ResourceNode node = _assetProvider.Instantiate(data.Prefab, at);
            
            string id = Guid.NewGuid().ToString();
            node.Construct(id, data);
            node.name = $"ResourceNode: {type.ToString()}, id: {id}";

            return node;
        }

        public Resource CreateResource(ResourceType type, Vector3 at)
        {
            ResourceConfig data = _configProvider.GetDataForResource(type);
            Resource resource = _assetProvider.Instantiate(data.Prefab, GetRandomPositionOnRadius(at));
            
            string id = Guid.NewGuid().ToString();
            resource.Construct(id, data);
            resource.name = $"Resource: {type.ToString()}, Position: {resource.transform.position.x}";
            
            return resource;
        }
        
        
        private Vector3 GetRandomPositionOnRadius(Vector3 startPosition)
        {
            float randomRadius = Random.Range(2, 4);

            float randomAngle = Random.Range(0f, 360f);
            float x = randomRadius * Mathf.Cos(randomAngle);
            float z = randomRadius * Mathf.Sin(randomAngle);
            
            return new Vector3(startPosition.x + x, .2f, startPosition.z + z);
        }
    }
}