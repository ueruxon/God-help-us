using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common.Extensions;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Reporting;
using Game.Scripts.GameplayLogic.AI.UtilityAI;

namespace Game.Scripts.GameplayLogic.AI
{
    public class DecisionMaker
    {
        private readonly AIContext _context;
        private readonly AIBrains _brains;
        private readonly ActorData _actorData;
        private readonly AIReporter _aiReporter;

        private readonly Dictionary<ActionType, AIAction> _actionsByType;
        private readonly IEnumerable<IUtilityFunction> _utilityFunctions;

        private ActionType _prevActionType;
        
        public DecisionMaker(AIContext context, AIBrains brains, ActorData data, AIReporter aiReporter)
        {
            _context = context;
            _brains = brains;
            _actorData = data;
            _aiReporter = aiReporter;

            _actionsByType = _actorData.Actions.ToDictionary(x => x.Type, x => x);
            _actionsByType.TryAdd(_actorData.DefaultAction.Type, _actorData.DefaultAction);
            _utilityFunctions = _brains.GetConvolutions(_actorData);
        }

        public AIAction GetDefaultAction() => 
            _actorData.DefaultAction;
        
        public AIAction MakeBestDecision()
        {
            ActionDetail actionDetail = CalculateUtilityScore();
            AIAction bestScoreAction = _actionsByType[actionDetail.ActionType];

            if (_prevActionType != actionDetail.ActionType)
                _aiReporter.ReportDecisionDetails(_context.ActorId, actionDetail, bestScoreAction);
            _prevActionType = actionDetail.ActionType;
            
            return bestScoreAction;
        }

        private ActionDetail CalculateUtilityScore()
        {
            // all apply utility functions
            IEnumerable<ActionDetail> scoreFactors = (
                from utilityFunction in _utilityFunctions
                where utilityFunction.AppliesTo(_context)
                let input = utilityFunction.GetInput(_context)
                let score = utilityFunction.EvaluateScore(input, _context)
                let actionType = utilityFunction.GetActionType()

                select new ActionDetail(actionType, utilityFunction.Name, score)
            );

            return scoreFactors
                .FindMax(x => x.Score);
        }
    }

    // only for debug
    public class ActionDetail
    {
        public string Name { get; }
        public float Score { get; }
        public ActionType ActionType { get; }

        public ActionDetail(ActionType actionType, string name, float score)
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