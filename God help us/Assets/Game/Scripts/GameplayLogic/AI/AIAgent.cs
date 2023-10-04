using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Actions;
using Game.Scripts.GameplayLogic.AI.Systems.Movement;
using Game.Scripts.GameplayLogic.AI.UtilityAI;
using Game.Scripts.GameplayLogic.JobManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    [RequireComponent(typeof(Animator), typeof(ActorAnimator))]
    public class AIAgent : MonoBehaviour
    {
        private JobSequencer _jobSequencer;
        
        private AIBrains _brains;
        private AIContext _aiContext;
        private DecisionMaker _decisionMaker;
        private ActionRunner _actionRunner;
        private JobPlanner _jobPlanner;
        
        private IMovementSystem _movementSystem;
        private ActorAnimator _actorAnimator;

        private readonly float _aiDecisionDelay = 0.2f;
        private float _currentTimer = 0;

        public void Construct(ActorData data, AIBrains aiBrains, JobSequencer jobSequencer)
        {
            _brains = aiBrains;

            _jobPlanner = new JobPlanner(jobSequencer, this);
            _movementSystem = GetComponent<AISimpleMover>();
            _actorAnimator = GetComponent<ActorAnimator>();
            _aiContext = new AIContext(_jobPlanner, _movementSystem, _actorAnimator);
            _decisionMaker = new DecisionMaker(_aiContext, _brains, data);
            _actionRunner = new ActionRunner(_aiContext);
        }

        public void Init()
        {
            _jobPlanner.Init();
            
            AIAction action = _decisionMaker.GetDefaultAction();
            _actionRunner.InitWithDefaultAction(action);
        }

        private void Update()
        {
            DecideAction();

            _actionRunner.Tick();
        }

        private void DecideAction()
        {
            _currentTimer += Time.deltaTime;

            if (_currentTimer > _aiDecisionDelay)
            {
                _currentTimer = 0;

                AIAction action = _decisionMaker.MakeBestDecision();
                _actionRunner.SetAction(action);
            }
        }
    }
}