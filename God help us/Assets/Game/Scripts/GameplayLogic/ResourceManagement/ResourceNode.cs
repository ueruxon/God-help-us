using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.ResourcesData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.ResourceManagement
{
    public class ResourceNode : MonoBehaviour, IGatherableResource
    {
        public event Action<ResourceType, Vector3> WorkedOut;
        
        [SerializeField] private List<Transform> _visualVariants;
        
        public string Id => _id;
        private string _id;
        private Transform _visual;
        
        private ResourceNodeConfig _config;
        private int _hitToDestroy;

        private bool _isActive;

        public void Construct(string id, ResourceNodeConfig config)
        {
            _id = id;
            _config = config;
        }

        public void Init()
        {
            _isActive = true;
            _hitToDestroy = _config.HitToDestroy;
            
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

        [Button]
        private void Shake()
        {
            _visual.DOShakePosition(0.5f, 0.05f);
            _visual.DOShakeRotation(0.5f, .25f);
            _visual.DOShakeScale(0.5f, 0.05f, 5, 10f);
        }
        
        private void Grow()
        {
            _visual.localScale = Vector3.zero;
            _visual.DOScale(Vector3.one, 0.7f).SetEase(Ease.InOutQuad);
            _visual.DOShakePosition(0.5f, 0.1f, randomness: 120f);
            _visual.DOShakeRotation(0.5f, .25f, randomness: 120f);
        }
        
        private async void Breakdown()
        {
            WorkedOut?.Invoke(_config.Type, transform.position);
            
            await Demolish();
            await Respawn();
        }

        private async UniTask Respawn()
        {
            int delayTime = _config.TimeToRespawn;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

            Init();
            Grow();
        }

        private async UniTask Demolish()
        {
            _visual.DOKill();
            _isActive = false;
            _visual.gameObject.SetActive(false);

            // анимация падения
            await UniTask.Delay(1000);
        }
    }
}