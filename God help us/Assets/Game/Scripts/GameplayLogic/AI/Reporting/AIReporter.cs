using System;
using System.Collections.Generic;
using Game.Scripts.GameplayLogic.Registers;

namespace Game.Scripts.GameplayLogic.AI.Reporting
{
    public class AIReporter
    {
        public event Action DecisionDetailsReported;
        
        private readonly ActorRegistry _actorRegistry;

        private readonly Dictionary<string, List<ReportDetails>> _historyDetailsByActorId;
        private readonly List<ReportDetails> _historyDetails;

        public AIReporter(ActorRegistry actorRegistry)
        {
            _actorRegistry = actorRegistry;
            _historyDetailsByActorId = new Dictionary<string, List<ReportDetails>>();
        }

        public void ReportDecisionDetails(string actorId, ActionDetail actionDetails, AIAction action)
        {
            ReportDetails details = new ReportDetails()
            {
                ActorName = _actorRegistry.GetActor(actorId).name,
                ActionName = $"Action: {actionDetails.Name}",
                Score = $"Score: {actionDetails.Score.ToString()}",
            };

            if (_historyDetailsByActorId.ContainsKey(actorId))
            {
                _historyDetailsByActorId[actorId].Add(details);

                if (_historyDetailsByActorId[actorId].Count > 6) 
                    _historyDetailsByActorId[actorId].RemoveAt(0);
            }
            else
                _historyDetailsByActorId.Add(actorId, new List<ReportDetails>() {details});

            DecisionDetailsReported?.Invoke();
        }
        
        public bool ContainsDetails(string actorId) => 
            _historyDetailsByActorId.ContainsKey(actorId);

        public List<ReportDetails> GetReportDetailsForActor(string actorId) => 
            _historyDetailsByActorId[actorId];
    }
}