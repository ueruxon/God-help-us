using System;
using System.Collections.Generic;
using Game.Scripts.Common.Interfaces;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    [RequireComponent(typeof(Building))]
    public class Storage : MonoBehaviour, IResourceRequester, IResourceProvider
    {
        public event Action<Resource> ResourceDelivered;

        private const int StorageCapacity = 42;
        
        [SerializeField] private Transform _fillerContainer;
        [SerializeField] private Transform _interactionPoint;

        private StorageConfig _data;
        
        private Transform[] _resourceFillers;
        private int _activeFillerIndex;

        private List<Resource> _registeredResources;
        private Queue<Resource> _requestedResources;
        private List<Resource> _storedResources;

        private int _requestedCount;
        private int _requestedResourceIndex;

        public void Construct(StorageConfig config)
        {
            _data = config;
            _resourceFillers = new Transform[StorageCapacity];
            _activeFillerIndex = 0;
            _registeredResources = new List<Resource>();
            _storedResources = new List<Resource>();
            _requestedResources = new Queue<Resource>();

            _requestedCount = 0;
            _requestedResourceIndex = 0;

            GenerateResourceFillers();
        }

        public Vector3 GetPosition() => 
            _interactionPoint.position;

        public ResourceType GetStoredType() => 
            _data.StoredType;

        public bool IsFull() => 
            _registeredResources.Count == StorageCapacity;

        public bool CanRequestResource() =>
            _requestedCount < _storedResources.Count;
        
        public bool Register(Resource resource)
        {
            if (resource.Type == _data.StoredType)
            {
                _registeredResources.Add(resource);
                return true;
            }
            
            return false;
        }

        public void UnregisterResource(Resource resource)
        {
            if (_registeredResources.Contains(resource)) 
                _registeredResources.Remove(resource);

            if (_storedResources.Contains(resource))
                _storedResources.Remove(resource);
        }

        public Resource RequestResource()
        {
            Resource resource = _storedResources[_requestedResourceIndex];
            _requestedResources.Enqueue(resource);
            _requestedCount++;

            if (_requestedResourceIndex <= _storedResources.Count)
                _requestedResourceIndex++;
            
            return resource;
        }

        public Resource GetResource()
        {
            Resource resource = _requestedResources.Dequeue();
            _requestedCount--;
            
            if (_requestedResourceIndex > 0)
                _requestedResourceIndex--;

            UpdateVisualFillers(false);
            UnregisterResource(resource);

            return resource;
        }

        public bool ContainsRegisterResource(Resource resource) => 
            _registeredResources.Contains(resource);

        public void Delivery(Resource resource)
        {
            _storedResources.Add(resource);

            resource.transform.SetParent(transform);
            resource.gameObject.SetActive(false);

            UpdateVisualFillers(true);
            
            ResourceDelivered?.Invoke(resource);
        }

        private void UpdateVisualFillers(bool incoming)
        {
            if (incoming)
            {
                _resourceFillers[_activeFillerIndex].gameObject.SetActive(true);
                _activeFillerIndex++;
            }
            else
            {
                _activeFillerIndex--;
                _resourceFillers[_activeFillerIndex].gameObject.SetActive(false);
            }
        }

        private void GenerateResourceFillers()
        {
            float xStart = -3f;
            float yStart = 0;

            float xOffset = 1f;
            float yOffset = 0.65f;
            float zOffset = 1.75f;

            int dept = 2;
            int width = 7;
            int height = 3;

            int index = 0;

            for (int z = 0; z < dept; ++z)
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        if (x > 0 && x < width - 1 && 
                            y > 0 && y < height - 1 && 
                            z > 0 && z < dept - 1) 
                            continue;
                        
                        float xPosition = xStart + xOffset * x;
                        float yPosition = yStart + yOffset * y;
                        float zPosition = zOffset * z;

                        if (z == 0) 
                            zPosition = -zOffset;

                        Vector3 spawnPosition = _fillerContainer.position + new Vector3(
                            xPosition, 
                            yPosition, 
                            zPosition);
                        
                        Transform filler = Instantiate(_data.FillerPrefab, spawnPosition, Quaternion.identity);
                        filler.SetParent(_fillerContainer);
                        filler.gameObject.SetActive(false);
                        _resourceFillers[index] = filler;
                        
                        index++;

                        filler.name = $"Wood Filler: {index}";
                    }
                }
            }
        }
    }
}