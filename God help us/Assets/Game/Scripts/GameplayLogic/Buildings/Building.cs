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
        private Dictionary<ResourceType, int> _requiredConstructionResources;

        private BuildingStage _stage;

        public void Construct(string id, BuildingConfig buildingConfig)
        {
            _id = id;
            _buildingConfig = buildingConfig;
            _requiredConstructionResources = new Dictionary<ResourceType, int>();

            if (buildingConfig is not null)
            {
                foreach (ConstructionData data in buildingConfig.RequiredResources)
                    _requiredConstructionResources.Add(data.Type, data.ResourceAmount);
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

        public List<ConstructionData> GetRequiredConstructionData() => 
            _buildingConfig.RequiredResources;

        public void Delivery(Resource resource)
        {
            _requiredConstructionResources[resource.Type]++;

            if (CheckRemainingResource())
            {
                Build();
            }
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
                int currentCount = _requiredConstructionResources[x.Type];
                return currentCount == x.ResourceAmount;
            });
        }
    }
}