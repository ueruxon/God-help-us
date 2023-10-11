using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.ResourceLogic;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Mining", menuName = "AI/Actions/Mining")]
    public class MiningAction : AIAction
    {
        [SerializeField] private float _hitCooldown = 2f;

        private Job _currentJob;
        private IGatherableResource _resourceNode;
        private float _currentHitTimer = 0f;

        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            Debug.Log("Enter on Mining");
            
            _currentJob = context.JobPlanner.GetJob();
            _resourceNode = _currentJob.JobData.Node;
            
            context.MovementSystem.SetDestination(_resourceNode.GetPosition());
            context.Animator.PlayWalking(true);
        }

        public override void OnUpdate(AIContext context)
        {
            // скорее всего проверку надо осуществлять по дистанции до позиции
            // ведь что будет если агент прервет движение
            if (context.MovementSystem.ReachedDestination())
            {
                context.Animator.PlayWalking(false);
                Mining(context);
            }
        }
        
        private void Mining(AIContext context)
        {
            _currentHitTimer += Time.deltaTime;

            if (_currentHitTimer > _hitCooldown)
            {
                context.Animator.PlayHit();

                _currentHitTimer = 0;
                _resourceNode.ExtractResource();
            }

            if (_resourceNode.IsActive() == false)
            {
                _currentJob.ChangeStatus(Job.Status.Completed);
                ChangeStatus(ActionStatus.Completed);
            }
        }

        public override void OnExit(AIContext context)
        {
            context.Animator.PlayWalking(false);
            
            _currentHitTimer = 0;
            _currentJob = null;

            Debug.Log("Exit on Mining");
        }
    }
}