using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Systems.Movement;
using Game.Scripts.GameplayLogic.JobManagement;

namespace Game.Scripts.GameplayLogic.AI
{
    public class AIContext
    {
        private readonly JobPlanner _jobPlanner;
        public IMovementSystem MovementSystem { get; }
        public ActorAnimator Animator { get; }
        
        public AIContext(JobPlanner jobPlanner, IMovementSystem movementSystem, ActorAnimator animator)
        {
            _jobPlanner = jobPlanner;
            Animator = animator;
            MovementSystem = movementSystem;
        }
        
        public bool HasJob() => 
            _jobPlanner.HasJob();

        public Job GetCurrentJob() => 
            _jobPlanner.GetJob();
    }
}