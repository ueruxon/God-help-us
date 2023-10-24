using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public enum BuildingStage
    {
        Released,
        Construction,
        Active
    }

    public class Building : MonoBehaviour, IResourceRequester
    {
        public event Action<string> Released; 
        public event Action<string> Constructed;
        
        [SerializeField] private Transform _activeBuildingView;
        [SerializeField] private Transform _constructionView;

        public string Id => _id;
        private string _id;
        private BuildingConfig _buildingConfig;

        private Dictionary<ResourceType, int> _requiredResourcesForConstruction;
        private Dictionary<ResourceType, int> _registeredResourcesForConstruction;
        private List<Resource> _registeredResources;
        
        private BuildingStage _stage;

        public void Construct(string id, BuildingConfig buildingConfig)
        {
            _id = id;
            _buildingConfig = buildingConfig;
            _requiredResourcesForConstruction = new Dictionary<ResourceType, int>();
            _registeredResourcesForConstruction = new Dictionary<ResourceType, int>();
            _registeredResources = new List<Resource>();

            if (buildingConfig is not null)
            {
                foreach (ConstructionData data in buildingConfig.RequiredResources)
                {
                    _requiredResourcesForConstruction[data.Type] = data.ResourceAmount;
                    _registeredResourcesForConstruction[data.Type] = 0;
                }
            }

            _stage = BuildingStage.Released;
        }

        public void Prepare()
        {
            _stage = BuildingStage.Construction;

            _activeBuildingView.gameObject.SetActive(false);
            _constructionView.gameObject.SetActive(true);
            
            Released?.Invoke(_id);
        }

        public Vector3 GetPosition() => 
            transform.position;

        public bool IsValidType(ResourceType type) => 
            _requiredResourcesForConstruction.ContainsKey(type);

        public bool Register(Resource resource)
        {
            if (_requiredResourcesForConstruction.ContainsKey(resource.Type))
            {
                int currentCount = _registeredResourcesForConstruction[resource.Type];
                if (currentCount + 1 <= _requiredResourcesForConstruction[resource.Type])
                {
                    _registeredResourcesForConstruction[resource.Type]++;
                    _registeredResources.Add(resource);

                    return true;
                }
            }
            
            return false;
        }

        public bool ContainsResource(Resource resource) =>
            _registeredResources.Contains(resource);

        public void Delivery(Resource resource)
        {
            _requiredResourcesForConstruction[resource.Type]--;
            _registeredResourcesForConstruction[resource.Type]--;
            _registeredResources.Remove(resource);
            
            Destroy(resource.gameObject);

            if (CheckRemainingResource()) 
                Build();
        }

        private void Build()
        {
            _activeBuildingView.gameObject.SetActive(true);
            _constructionView.gameObject.SetActive(false);
            _stage = BuildingStage.Active;
            
            Constructed?.Invoke(_id);
        }

        private bool CheckRemainingResource()
        {
            return _buildingConfig.RequiredResources.Any() && _buildingConfig.RequiredResources.All(x =>
            {
                int currentCount = _requiredResourcesForConstruction[x.Type];
                return currentCount <= 0;
            });
        }
    }
}