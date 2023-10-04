using Game.Scripts.GameplayLogic.AI;
using Game.Scripts.GameplayLogic.ResourceLogic;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.JobManagement.Jobs
{
    public class MiningJob : Job
    {
        private IGatherableResource _gatherableResource;
        
        // потом поменяем
        private float _currentHitTimer = 0f;
        private readonly float _hitCooldown = 2f;
        
        public MiningJob(IGatherableResource gatherableResource, int jobPriority)
        {
            _gatherableResource = gatherableResource;
            Priority = jobPriority;
        }

        public override void Enter(AIContext context)
        {
            context.MovementSystem.SetDestination(_gatherableResource.GetPosition());
            context.Animator.PlayWalking(true);
        }

        public override void Update(AIContext context)
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
                _gatherableResource.ExtractResource();
            }

            if (_gatherableResource.IsActive() == false) 
                ChangeStatus(Status.Completed);
        }

        public override void Exit(AIContext context) => 
            context.Animator.PlayWalking(false);
    }
}