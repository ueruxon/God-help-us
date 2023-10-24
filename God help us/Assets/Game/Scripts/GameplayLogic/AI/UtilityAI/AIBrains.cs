using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations;
using Game.Scripts.GameplayLogic.Registers;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI
{
    public class AIBrains
    {
        private readonly AIWhen When;
        private readonly AIGetInput GetInput;
        private readonly AIScoreCalculator ScoreCalculator;

        private Convolutions _convolutions;

        public AIBrains(BuildingRegistry buildingRegistry)
        {
            When = new AIWhen(buildingRegistry);
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
                {When.HasMiningJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+30), ActionType.Mining, "Mining"},
                {When.HasCollectJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+30), ActionType.Collect, "Collect Item"},
                {When.HasConstructJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+40), ActionType.Collect, "Construct"},
                {When.HasResourceInInventory, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+50), ActionType.Delivery, "Delivery"},
                {When.AnyAvailableJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+10), ActionType.JobRequested, "Job Requested"},
                {When.IsWorkerFreelancer, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+20), ActionType.Freelance, "Freelance"},
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