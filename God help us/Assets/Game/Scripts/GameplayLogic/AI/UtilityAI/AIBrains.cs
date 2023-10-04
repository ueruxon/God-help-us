using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Actions;
using Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI
{
    public class AIBrains
    {
        private readonly AIWhen When;
        private readonly AIGetInput GetInput;
        private readonly AIScoreCalculator ScoreCalculator;

        private Convolutions _convolutions;

        public AIBrains()
        {
            When = new AIWhen();
            GetInput = new AIGetInput();
            ScoreCalculator = new AIScoreCalculator();

            CreateRules();
        }

        public IEnumerable<IUtilityFunction> GetConvolutions(ActorData data)
        {
            IEnumerable<IUtilityFunction> convolutions = _convolutions
                .Where(utilityFunction =>
                    data.Actions.Any(aiAction => utilityFunction.GetActionType() == aiAction.Type));

            return convolutions;
        }

        private void CreateRules()
        {
            _convolutions = new Convolutions()
            {
                {When.IsDontMove, GetInput.IsTrue, ScoreCalculator.AsIs, ActionType.Idle, "Idle"},
                {When.HasJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+100), ActionType.Job, "Job"}
                //{When.MoveToTarget, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+5), ActionType.MoveToTarget, "MoveTo"},
                //{When.IsDontMove, GetInput.IsFalse, ScoreCalculator.AsIs, ActionType.Idle, "Idle 2"},
                //{When.ResourceNodeIsNear, GetInput.InventoryEmpty, ScoreCalculator.ScaleBy(+10), "Mining"},
                //{When.MoveToTarget, GetInput.InventoryEmpty, ScoreCalculator.ScaleBy(+10), "Mining"},
            };
        }
    }

    public class Convolutions : List<UtilityFunction>
    {
        public void Add(
            Func<AIContext, bool> appliesTo,
            Func<AIContext, float> getInput,
            Func<float, AIContext, float> evaluateScore,
            ActionType actionType,
            string name)
        {
            Add(new UtilityFunction(appliesTo, getInput, evaluateScore, actionType, name));
        }
    }
}