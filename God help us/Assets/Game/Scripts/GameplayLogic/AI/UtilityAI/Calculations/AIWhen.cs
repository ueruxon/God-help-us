namespace Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations
{
    public class AIWhen
    {
        public bool IsDontMove(AIContext context)
        {
            return context.MovementSystem.ReachedDestination();
        }

        public bool MoveToTarget(AIContext context)
        {
            return context.MovementSystem.ReachedDestination() == false;
        }

        public bool HasJob(AIContext context)
        {
            return context.HasJob();
        }
    }
}