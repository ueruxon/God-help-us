using System;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Reporting;
using Game.Scripts.GameplayLogic.AI.Systems.Movement;
using Game.Scripts.GameplayLogic.AI.UtilityAI;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    [RequireComponent(typeof(Animator), typeof(ActorAnimator))]
    public class AIAgent : MonoBehaviour, IJobRequester
    {
        [SerializeField] private Transform _backpackContainer;
        
        public string WorkerId => _id;
        private string _id;
        
        private JobController _jobController;

        private AIBrains _brains;
        private AIContext _aiContext;
        private DecisionMaker _decisionMaker;
        private ActionRunner _actionRunner;
        private JobPlanner _jobPlanner;
        private AIReporter _aiReporter;

        private IMovementSystem _movementSystem;
        private ActorAnimator _actorAnimator;
        private Backpack _backpack;

        private readonly float _aiDecisionDelay = 0.2f;
        private float _currentTimer = 0;
        
        public void Construct(string id, 
            ActorData data, 
            AIBrains aiBrains, 
            JobController jobController,
            BuildingRegistry buildingRegistry,
            AIReporter reporter)
        {
            _id = id;
            _brains = aiBrains;
            
            _jobPlanner = new JobPlanner(jobController, this);
            _movementSystem = GetComponent<AISimpleMover>();
            _actorAnimator = GetComponent<ActorAnimator>();
            _backpack = new Backpack(transform, _backpackContainer);

            _aiReporter = reporter;
            _aiContext = new AIContext(_id, _jobPlanner, _movementSystem, _actorAnimator, _backpack, buildingRegistry);
            _decisionMaker = new DecisionMaker(_aiContext, _brains, data, _aiReporter);
            _actionRunner = new ActionRunner(_aiContext);
            _actionRunner.ActionStatusChanged += DecideActionImmediately;
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

        private void LateUpdate() => 
            _backpack.Tick();

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

        private void DecideActionImmediately(AIAction.ActionStatus status)
        {
            if (status != AIAction.ActionStatus.Running)
            {
                AIAction action = _decisionMaker.MakeBestDecision();
                _actionRunner.SetAction(action);
            }
        }
    }
}