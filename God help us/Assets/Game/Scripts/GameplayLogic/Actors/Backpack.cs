using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Actors
{
    public class Backpack
    {
        private Transform _actorTransform;
        private Transform _container;
        
        private Resource _item;

        public Backpack(Transform actorTransform, Transform container)
        {
            _actorTransform = actorTransform;
            _container = container;
        }
        
        public void Pickup(Resource resource)
        {
            _item = resource;
            _item.gameObject.SetActive(true);
            _item.transform.SetParent(_container);
            _item.transform.position = _container.position;
            _item.transform.rotation = _container.rotation;
        }

        public void Drop()
        {
            if (HasItem())
                _item.transform.SetParent(null);
            
            _item = null;
        }

        public Resource GetItem() => 
            _item;

        public bool HasItem() =>
            _item is not null;

        public void Tick() => 
            _container.rotation = Quaternion.Slerp(_container.rotation, _actorTransform.rotation, 5f * Time.deltaTime);
    }
}