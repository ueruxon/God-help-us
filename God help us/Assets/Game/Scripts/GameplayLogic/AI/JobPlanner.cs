using Cysharp.Threading.Tasks;
using Game.Scripts.GameplayLogic.JobManagement;

namespace Game.Scripts.GameplayLogic.AI
{
    public enum AIState
    {
        Idle,
        InWork
    }
    
    public class JobPlanner
    {
        private readonly JobSequencer _jobSequencer;
        private readonly AIAgent _agent;

        private Job _currentJob;
        private AIState _aiState;

        public JobPlanner(JobSequencer jobSequencer, AIAgent agent)
        {
            _jobSequencer = jobSequencer;
            _agent = agent;
        }

        public void Init() => 
            Release();

        private async UniTaskVoid CheckAvailableJob()
        {
            await UniTask.Delay(500);
            
            while (_aiState == AIState.Idle)
            {
                if (_jobSequencer.HasAnyJob()) 
                    ReceiveJob();

                await UniTask.Delay(500);
            }
        }

        private void ReceiveJob()
        {
            _currentJob = _jobSequencer.GetPriorityJob();
            _currentJob.StatusChanged += OnJobStatusChanged;
            
            _aiState = AIState.InWork;
        }

        public bool HasJob() =>
            _aiState == AIState.InWork;

        public Job GetJob() => 
            _currentJob;

        private void OnJobStatusChanged(Job.Status status)
        {
            switch (status)
            {
                case Job.Status.Completed:
                    _jobSequencer.CompleteJob(_currentJob);
                    break;
                case Job.Status.Canceled:
                    _jobSequencer.CanceledJob(_currentJob);
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