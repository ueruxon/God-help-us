using System;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI
{
    public interface IUtilityFunction
    {
        string Name { get; set; }
        
        bool AppliesTo(AIContext context);
        float GetInput(AIContext context);
        float EvaluateScore(float input, AIContext context);

        Type GetActionType();
    }
}