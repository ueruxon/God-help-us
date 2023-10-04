using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Actions;
using Game.Scripts.GameplayLogic.AI.UtilityAI;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    public class DecisionMaker
    {
        private readonly AIContext _context;
        private readonly AIBrains _brains;
        private readonly ActorData _actorData;

        private readonly Dictionary<ActionType, AIAction> _actionsByType;
        private readonly IEnumerable<IUtilityFunction> _utilityFunctions;
        
        public DecisionMaker(AIContext context, AIBrains brains, ActorData data)
        {
            _context = context;
            _brains = brains;
            _actorData = data;

            _actionsByType = _actorData.Actions.ToDictionary(x => x.Type, x => x);
            _actionsByType.TryAdd(_actorData.DefaultAction.Type, _actorData.DefaultAction);
            _utilityFunctions = _brains.GetConvolutions(_actorData);
        }

        public AIAction GetDefaultAction() => 
            _actorData.DefaultAction;
        
        public AIAction MakeBestDecision()
        {
            ActionType actionType = CalculateUtilityScore();
            AIAction bestScoreAction = _actionsByType[actionType];

            return bestScoreAction;
        }

        private ActionType CalculateUtilityScore()
        {
            // all apply utility functions
            IEnumerable<ScoreFactor> scoreFactors = (
                from utilityFunction in _utilityFunctions
                where utilityFunction.AppliesTo(_context)
                let input = utilityFunction.GetInput(_context)
                let score = utilityFunction.EvaluateScore(input, _context)
                let actionType = utilityFunction.GetActionType()

                select new ScoreFactor(actionType, utilityFunction.Name, score)
            );

            // foreach (ScoreFactor scoreFactor in scoreFactors)
            // {
            //     Debug.Log($"Score: {scoreFactor.Score}, " +
            //               $"Name: {scoreFactor.Name}, Count: {scoreFactors.ToList().Count}");
            // }

            return scoreFactors
                .FindMax(x => x.Score)
                .ActionType;
        }
    }

    // only for debug
    public class ScoreFactor
    {
        public string Name { get; }
        public float Score { get; }
        public ActionType ActionType { get; }

        public ScoreFactor(ActionType actionType, string name, float score)
        {
            Name = name;
            Score = score;
            ActionType = actionType;
        }

        public override string ToString()
        {
            return $"{Name} -> {(Score >= 0 ? "+" : "")}{Score}";
        }
    }
}