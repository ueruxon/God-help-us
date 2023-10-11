using System;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    public abstract class AIAction : ScriptableObject
    {
        public event Action<ActionStatus> StatusChanged;
        
        public enum ActionStatus
        {
            Running,
            Completed,
            Canceled
        }
        
        [SerializeField] public string ActionName;
        //[SerializeField, Range(1f, 0f)] public float Weight = 1f;

        public ActionType Type;
        //public float Score;

        private ActionStatus _status;

        public virtual void OnEnter(AIContext context) => 
            ChangeStatus(ActionStatus.Running);

        public abstract void OnExit(AIContext context);

        public virtual void OnUpdate(AIContext context) { }

        protected void ChangeStatus(ActionStatus status)
        {
            _status = status;
            StatusChanged?.Invoke(_status);
        }
    }
}