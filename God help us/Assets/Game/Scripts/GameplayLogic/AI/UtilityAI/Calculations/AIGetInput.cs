namespace Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations
{
    public class AIGetInput
    {
        private const float True = 1;
        private const float False = 0;
        
        public float IsTrue(AIContext context) => True;
        public float IsFalse(AIContext context) => False;
    }
}