using System;
using Game.Scripts.Common.DataStructures.Heap;
using Game.Scripts.GameplayLogic.ResourceLogic;

namespace Game.Scripts.GameplayLogic.JobManagement
{
    public class Job : IPriorityItem
    {
        public enum Status
        {
            None,
            Requested,
            InWork,
            Completed,
            Canceled
        }
        
        public event Action<Status> StatusChanged;

        private readonly JobCategory _category;
        private readonly int _priority;
        private readonly JobData _data;

        public int Priority => _priority;
        public JobCategory Category => _category;
        public JobData JobData => _data;

        public float Offset { get; set; }
        public bool Enqueued { get; set; }
        
        private Status _status;

        public Job(JobCategory jobCategory, int priority, JobData data)
        {
            _category = jobCategory;
            _priority = priority;
            _data = data;
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

    public struct JobData
    {
        public IGatherableResource Node;
        public Resource Resource;
    }
}