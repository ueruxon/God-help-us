using System.Collections.Generic;
using Game.Scripts.Data.Buildings;
using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    [RequireComponent(typeof(Building))]
    public class Storage : MonoBehaviour, IResourceRequester
    {
        private const int StorageCapacity = 42;
        
        [SerializeField] private Transform _fillerContainer;
        [SerializeField] private Transform _interactionPoint;

        private StorageConfig _data;
        
        private Transform[] _resourceFillers;
        private int _activeFillerIndex;

        private List<Resource> _registeredResources;
        private Stack<Resource> _storedResources;

        private int _resourceCount;
        private int _registeredCount;
        private int _requestedCount;
        private int _requestedResourceIndex;

        public void Construct(StorageConfig config)
        {
            _data = config;
            _resourceFillers = new Transform[StorageCapacity];
            _activeFillerIndex = 0;
            _registeredResources = new List<Resource>();
            _storedResources = new Stack<Resource>();

            _resourceCount = 0;
            _registeredCount = 0;
            _requestedCount = 0;
            _requestedResourceIndex = 0;

            GenerateResourceFillers();
        }

        public Vector3 GetPosition() => 
            _interactionPoint.position;
        
        public bool HasResource() => 
            _resourceCount > 0;

        public bool CanRequestResource() => 
            _requestedCount + 1 <= _resourceCount;

        public bool IsFull() => 
            _resourceCount == StorageCapacity;

        public bool RegisterResource(Resource resource)
        {
            if (_registeredCount + 1 <= StorageCapacity)
            {
                _registeredCount++;
                _registeredResources.Add(resource);
                return true;
            }
            
            return false;
        }

        public bool UnregisterResource(Resource resource)
        {
            if (_registeredResources.Contains(resource))
            {
                _registeredCount--;
                _registeredResources.Remove(resource);
                return true;
            }
            
            return false;
        }

        public Resource RequestResource()
        {
            Resource resource = _registeredResources[_requestedResourceIndex];
            
            _requestedCount++;
            _requestedResourceIndex++;
            
            return resource;
        }

        public void GetResource(string id)
        {
            //Resource resource = _storedResources[_requestedResourceIndex];
        
            
            _resourceFillers[_activeFillerIndex].gameObject.SetActive(false);
            
            _resourceCount--;
            _activeFillerIndex--;
            _requestedCount--;
        }

        public bool ContainsResource(Resource resource) => 
            _registeredResources.Contains(resource);

        public void Delivery(Resource resource)
        {
            _resourceFillers[_activeFillerIndex].gameObject.SetActive(true);
            _activeFillerIndex++;
            _resourceCount++;

            resource.transform.SetParent(transform);
            resource.gameObject.SetActive(false);

            _storedResources.Push(resource);
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