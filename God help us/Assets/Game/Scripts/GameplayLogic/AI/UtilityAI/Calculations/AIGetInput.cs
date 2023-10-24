using Game.Scripts.GameplayLogic.JobManagement;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations
{
    public class AIGetInput
    {
        private const float True = 1;
        private const float False = 0;
        
        public float IsTrue(AIContext context) => True;
        public float IsFalse(AIContext context) => False;

        public float ResourceAlreadyInInventory(AIContext context)
        {
            if (context.JobPlanner.HasJob() && context.JobPlanner.GetJob().Category == JobCategory.Collect)
            {
                if (context.Backpack.HasItem())
                {
                    if (context.Backpack.GetItem() == context.JobPlanner.GetJob().JobData.Provider.GetResource())
                    {
                        return True + 50f;
                    }
                }
            }

            return False;
        }
    }
}