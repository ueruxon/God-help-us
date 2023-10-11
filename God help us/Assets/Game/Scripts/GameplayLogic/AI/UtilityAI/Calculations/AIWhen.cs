using Game.Scripts.GameplayLogic.JobManagement;
using UnityEngine;

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
            return context.JobPlanner.HasJob();
        }

        public bool HasMiningJob(AIContext context)
        {
            if (context.JobPlanner.HasJob())
            {
                return context.JobPlanner.GetJob().Category == JobCategory.Mining;
            }

            return false;
        }

        public bool HasCollectJob(AIContext context)
        {
            if (context.JobPlanner.HasJob())
                return context.JobPlanner.GetJob().Category == JobCategory.Collect;
            
            return false;
        }

        public bool HasResourceInInventory(AIContext context)
        {
            if (context.Backpack.HasItem())
                return true;

            return false;
        }
    }
}