using System;

namespace Game.Scripts.GameplayLogic.AI
{
    public class ActionRunner
    {
        public event Action<AIAction.ActionStatus> ActionStatusChanged;
        
        private readonly AIContext _context;
        private AIAction _currentAction;

        public ActionRunner(AIContext context) => 
            _context = context;

        public void InitWithDefaultAction(AIAction action)
        {
            _currentAction = action;
            _currentAction.OnEnter(_context);
        }
        
        public void SetAction(AIAction action)
        {
            if (!Equals(_currentAction, action))
            {
                _currentAction.OnExit(_context);
                _currentAction.StatusChanged -= OnActionStatusChanged;
                _currentAction = action;
                _currentAction.OnEnter(_context);
                _currentAction.StatusChanged += OnActionStatusChanged;
            }
        }

        public void Tick() => 
            _currentAction.OnUpdate(_context);

        private void OnActionStatusChanged(AIAction.ActionStatus status, AIAction action) => 
            ActionStatusChanged?.Invoke(status);
    }
}