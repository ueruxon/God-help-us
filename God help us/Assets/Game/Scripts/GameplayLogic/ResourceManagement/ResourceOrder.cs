using Game.Scripts.Common.Interfaces;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.ResourceManagement
{
    public class ResourceOrder : IResourceProvider
    {
        private readonly Resource _resource;

        public ResourceOrder(Resource resource)
        {
            _resource = resource;
        }
        
        public Vector3 GetPosition() => 
            _resource.transform.position;

        public Resource GetResource() => 
            _resource;
    }
}