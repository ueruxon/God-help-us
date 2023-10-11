using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Common;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.ResourcesData;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.ResourceLogic
{
    public class ResourceNode : MonoBehaviour, IGatherableResource
    {
        public event Action<ResourceType, Vector3> Destroyed;
        
        [SerializeField] private List<Transform> _visualVariants;

        private string _uniqueId;
        private Transform _visual;
        
        private ResourceNodeConfig _config;
        private int _hitToDestroy;

        private bool _isActive;

        public void Construct(ResourceNodeConfig config)
        {
            _config = config;
            _hitToDestroy = config.HitToDestroy;
            _uniqueId = UniqueIdGenerator.GenerateIdForGameObject(gameObject);
        }

        public void Init()
        {
            _isActive = true;
            
            foreach (Transform variant in _visualVariants) 
                variant.gameObject.SetActive(false);

            _visual = _visualVariants.GetRandomItem();
            _visual.gameObject.SetActive(true);
        }

        public Vector3 GetPosition() => 
            transform.position;

        public bool IsActive() => 
            _isActive;

        public void ExtractResource()
        {
            _hitToDestroy--;
            Shake();

            if (_hitToDestroy == 0) 
                Breakdown();
        }

        private void Shake()
        {
            _visual.DOShakePosition(0.5f, 0.25f);
            _visual.DOShakeRotation(0.5f, 1f);
            _visual.DOShakeScale(0.5f, 0.2f, 10, 120f);
        }
        
        private void Breakdown()
        {
            _visual.DOKill();
            _isActive = false;
            //_collider.enabled = false;
            //_demolishFeedback.PlayFeedbacks();

            //_nodeState = ResourceNodeState.WorkedOut;
            //_workIndicator.Hide();
            
            Destroyed?.Invoke(_config.Type, transform.position);
        }
    }
}