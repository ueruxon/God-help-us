using Cysharp.Threading.Tasks;
using Game.Scripts.GameplayLogic.JobManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    public enum AIState
    {
        Idle,
        InWork
    }
    
    public class JobPlanner
    {
        private readonly JobController _jobController;
        private readonly AIAgent _agent;

        private Job _currentJob;
        private AIState _aiState;

        public JobPlanner(JobController jobController, AIAgent agent)
        {
            _jobController = jobController;
            _agent = agent;
        }

        public void Init() => 
            Release();

        public bool HasJob() =>
            _aiState == AIState.InWork;

        public Job GetJob() => 
            _currentJob;

        private async UniTaskVoid CheckAvailableJob()
        {
            await UniTask.Delay(500);
            
            while (_aiState == AIState.Idle)
            {
                if (_jobController.HasAnyJob()) 
                    ReceiveJob();

                await UniTask.Delay(500);
            }
        }

        private void ReceiveJob()
        {
            _currentJob = _jobController.GetPriorityJob(_agent.WorkerId);
            _currentJob.StatusChanged += OnJobStatusChanged;

            _aiState = AIState.InWork;
        }

        private void OnJobStatusChanged(Job.Status status)
        {
            switch (status)
            {
                case Job.Status.Completed:
                    _jobController.CompleteJob(_currentJob);
                    break;
                case Job.Status.Canceled:
                    _jobController.CanceledJob(_currentJob);
                    break;
            }

            _currentJob.StatusChanged -= OnJobStatusChanged;
            _currentJob = null;

            Release();
        }

        private void Release()
        {
            _aiState = AIState.Idle;
            CheckAvailableJob().Forget();
        }
    }
}