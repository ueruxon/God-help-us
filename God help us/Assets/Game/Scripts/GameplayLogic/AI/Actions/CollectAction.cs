using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.ResourceLogic;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Collect", menuName = "AI/Actions/Collect")]
    public class CollectAction : AIAction
    {
        private Job _currentJob;
        private Resource _resource;
        
        public override void OnEnter(AIContext context)
        {
            Debug.Log("Enter on Collect");
            base.OnEnter(context);
            
            _currentJob = context.JobPlanner.GetJob();
            _resource = _currentJob.JobData.Resource;
            
            context.MovementSystem.SetDestination(_resource.transform.position);
            context.Animator.PlayWalking(true);
        }

        public override void OnUpdate(AIContext context)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                context.Animator.PlayWalking(false);
                context.Backpack.Pickup(_resource);
                
                _currentJob.ChangeStatus(Job.Status.Completed);
                ChangeStatus(ActionStatus.Completed);
            }
        }

        public override void OnExit(AIContext context)
        {
            context.Animator.PlayWalking(false);

            _resource = null;
            _currentJob = null;

            Debug.Log("Exit on Collect");
        }
    }
}