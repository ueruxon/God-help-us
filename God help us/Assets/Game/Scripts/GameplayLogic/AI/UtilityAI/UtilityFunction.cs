using System;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI
{
    public class UtilityFunction : IUtilityFunction
    {
        private readonly Func<AIContext, bool> _appliesTo;
        private readonly Func<AIContext, float> _getInput;
        private readonly Func<float, AIContext, float> _evaluateScore;
        private readonly Type _actionType;

        public string Name { get; set; }

        public UtilityFunction(
            Func<AIContext, bool> appliesTo,
            Func<AIContext, float>  getInput,
            Func<float, AIContext, float> evaluateScore,
            Type actionType,
            string name)
        {
            Name = name;
            _appliesTo = appliesTo;
            _getInput = getInput;
            _evaluateScore = evaluateScore;
            _actionType = actionType;
        }

        public bool AppliesTo(AIContext context) => _appliesTo(context);

        public float GetInput(AIContext context) => _getInput(context);

        public float EvaluateScore(float input, AIContext context) => _evaluateScore(input, context);
        
        public Type GetActionType() => 
            _actionType;
    }
}