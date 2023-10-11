using System.Collections.Generic;
using Game.Scripts.Data.Buildings;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public class Storage : MonoBehaviour
    {
        private const int StorageCapacity = 42;
        
        [SerializeField] private Transform _fillerContainer;
        [SerializeField] private Transform _interactionPoint;

        private StorageConfig _data;
        
        private Transform[] _resourceFillers;
        private int _activeFillerIndex;

        private List<string> _registeredResources;
        private int _registeredCount;
        private int _resourceCount;

        public void Construct(StorageConfig config)
        {
            _data = config;
            _resourceFillers = new Transform[StorageCapacity];
            _activeFillerIndex = 0;
            _registeredResources = new List<string>();
            _registeredCount = 0;

            GenerateResourceFillers();
        }

        public Vector3 GetPosition() => 
            _interactionPoint.position;
        
        public bool HasResource() => 
            _resourceCount > 0;
        
        public bool IsFull() => 
            _resourceCount == StorageCapacity;
        
        public bool RegisterResource(string id)
        {
            if (_registeredCount + 1 <= StorageCapacity)
            {
                _registeredCount++;
                _registeredResources.Add(id);
                return true;
            }
            
            return false;
        }

        public bool UnregisterResource(string id)
        {
            if (_registeredResources.Contains(id))
            {
                _registeredCount--;
                _registeredResources.Remove(id);
                return true;
            }
            
            return false;
        }

        public bool ContainsResource(string id) => 
            _registeredResources.Contains(id);

        public void Delivery()
        {
            _resourceFillers[_activeFillerIndex].gameObject.SetActive(true);
            _activeFillerIndex++;
            _resourceCount++;

            //_resourceRepository.AddResource(resource.GetResourceType(), 1);
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