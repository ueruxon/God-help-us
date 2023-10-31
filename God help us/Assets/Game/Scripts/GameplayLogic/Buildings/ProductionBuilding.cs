using System;
using Game.Scripts.Data.Buildings;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Buildings
{
    public enum ProductionState
    {
        Inactive,
        Freely,
        Requested,
        Active
    }
    
    [RequireComponent(typeof(Building))]
    public class ProductionBuilding : MonoBehaviour
    {
        public event Action<ProductionState, ProductionBuilding> StateChanged;
        public event Action<ResourceType, Vector3> ResourceCanSpawn; 

        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private WorkProgressBar _progressBar;
        [SerializeField] private Transform _spawnPoint;
        
        private ProductionBuildingConfig _config;
        private Building _building;
        private ProductionState _state;

        private string _workerId;
        
        private float _currentWorkTimer = 0f;
        private bool _showProgress;
        
        public void Construct(ProductionBuildingConfig config, Building building)
        {
            _config = config;
            _building = building;
            _building.Constructed += OnBuildingConstructed;
            _state = ProductionState.Inactive;
            _showProgress = false;
            _progressBar.Hide();
        }

        public Vector3 GetPosition() =>
            _interactionPoint.position;
        
        public bool IsAvailable() => 
            _state == ProductionState.Freely;

        public void Request(string actorId)
        {
            _workerId = actorId;
            _state = ProductionState.Requested;
            
            StateChanged?.Invoke(_state, this);
        }

        public void Resolve()
        {
            _state = ProductionState.Freely;
            _progressBar.Hide();
            _showProgress = false;
            
            StateChanged?.Invoke(_state, this);
            
            //_workerId = null;
        }

        public string GetWorkerId() => 
            _workerId;

        public void Work()
        {
            _currentWorkTimer += Time.deltaTime;
            
            if (_currentWorkTimer > _config.TimeToSpawnResource)
            {
                _currentWorkTimer = 0f;
                ResourceCanSpawn?.Invoke(_config.ProducedType, _spawnPoint.position);
            }
            
            UpdateProgressBar();
        }

        private void OnBuildingConstructed(string id) => 
            _state = ProductionState.Freely;

        private void UpdateProgressBar()
        {
            if (_showProgress == false && _currentWorkTimer > 0.5f)
            {
                _showProgress = true;
                _progressBar.Show();
            }

            if (_showProgress && _currentWorkTimer < 0.5f)
            {
                _showProgress = false;
                _progressBar.Hide();
            }
            
            if (_showProgress) 
                _progressBar.SetValue(_currentWorkTimer, _config.TimeToSpawnResource);
        }

        private void OnDestroy() => 
            _building.Constructed -= OnBuildingConstructed;
    }
}