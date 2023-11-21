using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Actions;
using Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations;
using Game.Scripts.GameplayLogic.Registers;
using UnityEngine;

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
            // IEnumerable<IUtilityFunction> convolutions = _convolutions
            //     .Where(utilityFunction =>
            //         data.Actions.Any(aiAction =>
            //         {
            //             Debug.Log($"" +
            //                       $"utility {utilityFunction.GetActionType()}, " +
            //                       $"aiAction {aiAction.GetType()}" +
            //                       $"bool {utilityFunction.GetActionType() == aiAction.GetType()}");
            //             return utilityFunction.GetActionType() == aiAction.GetType();
            //         }));

            Convolutions filteredConvolutions = new Convolutions();
            foreach (UtilityFunction utilityFunction in _convolutions)
            {
                foreach (AIAction aiAction in data.Actions)
                {
                    if (utilityFunction.GetActionType() == aiAction.GetType()) 
                        filteredConvolutions.Add(utilityFunction);
                }
            }
            
            return filteredConvolutions;
        }

        private void CreateRules()
        {
            _convolutions = new Convolutions()
            {
                {When.IsDontMove, GetInput.IsTrue, ScoreCalculator.AsIs, typeof(IdleAction), "Idle"},
                {When.HasMiningJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+30), typeof(MiningAction), "Mining"},
                {When.HasCollectJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+30), typeof(CollectAction), "Collect Item"},
                {When.HasConstructJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+40), typeof(CollectAction), "Construct"},
                {When.HasResourceInInventory, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+50), typeof(DeliveryAction), "Delivery"},
                {When.AnyAvailableJob, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+10), typeof(JobRequestAction), "Job Requested"},
                {When.IsWorkerFreelancer, GetInput.IsTrue, ScoreCalculator.IncreaseBy(+20), typeof(FreelanceAction), "Freelance"},
            };
        }
    }

    public class Convolutions : List<UtilityFunction>
    {
        public void Add(
            Func<AIContext, bool> appliesTo,
            Func<AIContext, float> getInput,
            Func<float, AIContext, float> evaluateScore,
            Type actionType,
            string name)
        {
            Add(new UtilityFunction(appliesTo, getInput, evaluateScore, actionType, name));
        }
    }
}