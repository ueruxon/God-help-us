using System;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    public abstract class AIAction : ScriptableObject
    {
        public event Action<ActionStatus, AIAction> StatusChanged;
        
        public enum ActionStatus
        {
            Running,
            Completed,
            Canceled
        }
        
        [SerializeField] public string ActionName;
        
        private ActionStatus _status;

        public virtual void OnEnter(AIContext context) => 
            ChangeStatus(ActionStatus.Running);

        public virtual void OnExit(AIContext context) { }

        public virtual void OnUpdate(AIContext context) { }

        protected void ChangeStatus(ActionStatus status)
        {
            _status = status;
            StatusChanged?.Invoke(_status, this);
        }
    }
}