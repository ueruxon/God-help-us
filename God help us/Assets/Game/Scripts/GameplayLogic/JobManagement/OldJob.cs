using System;
using Game.Scripts.Common.DataStructures.Heap;
using Game.Scripts.GameplayLogic.AI;

namespace Game.Scripts.GameplayLogic.JobManagement
{
    public abstract class OldJob : IPriorityItem
    {
        public event Action<Status> StatusChanged;
        
        public enum Status
        {
            None,
            Requested,
            InWork,
            Completed,
            Canceled
        }
        
        public int Priority
        {
            get;
            set;
        }
        public float Offset { get; set; }
        public bool Enqueued { get; set; }

        private Status _status;

        public virtual void Enter(AIContext context)
        {
            
        }

        public virtual void Update(AIContext context)
        {
            
        }

        public virtual void Exit(AIContext context)
        {
            
        }

        public void ChangeStatus(Status newStatus)
        {
            if (_status != newStatus)
            {
                _status = newStatus;
                StatusChanged?.Invoke(_status);
            }
        }
    }
}