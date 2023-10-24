using Game.Scripts.Common.Interfaces;
using Game.Scripts.GameplayLogic.JobManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Collect", menuName = "AI/Actions/Collect")]
    public class CollectAction : AIAction
    {
        private Job _currentJob;
        private IResourceProvider _resourceProvider;
        
        public override void OnEnter(AIContext context)
        {
            Debug.Log("Enter on Collect");
            base.OnEnter(context);
            
            _currentJob = context.JobPlanner.GetJob();
            _resourceProvider = _currentJob.JobData.Provider;
            
            context.MovementSystem.SetDestination(_resourceProvider.GetPosition());
            context.Animator.PlayWalking(true);
        }

        public override void OnUpdate(AIContext context)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                context.Animator.PlayWalking(false);
                context.Backpack.Pickup(_resourceProvider.GetResource());
                
                _currentJob.ChangeStatus(Job.Status.Completed);
                ChangeStatus(ActionStatus.Completed);
            }
        }

        public override void OnExit(AIContext context)
        {
            context.Animator.PlayWalking(false);

            _resourceProvider = null;
            _currentJob = null;

            Debug.Log("Exit on Collect");
        }
    }
}