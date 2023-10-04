using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;

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
            ResourceNodeConfig data =_configProvider.GetDataForResourceNode(type);
            ResourceNode node = _assetProvider.Instantiate(data.Prefab, at);
            node.Construct(data);

            return node;
        }
    }
}