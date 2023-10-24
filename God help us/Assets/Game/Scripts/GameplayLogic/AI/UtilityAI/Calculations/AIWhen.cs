using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations
{
    public class AIWhen
    {
        private readonly BuildingRegistry _buildingRegistry;

        public AIWhen(BuildingRegistry buildingRegistry)
        {
            _buildingRegistry = buildingRegistry;
        }

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

        public bool HasConstructJob(AIContext context)
        {
            if (context.JobPlanner.HasJob())
                return context.JobPlanner.GetJob().Category == JobCategory.Construct;
            
            return false;
        }

        public bool HasResourceInInventory(AIContext context)
        {
            if (context.Backpack.HasItem())
                return true;

            return false;
        }

        public bool AnyAvailableJob(AIContext context) => 
            _buildingRegistry.CheckAnyAvailableProductionBuilding();

        public bool IsWorkerFreelancer(AIContext context) => 
            _buildingRegistry.WorkerContains(context.ActorId);
    }
}