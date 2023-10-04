using Game.Scripts.GameplayLogic.AI.Actions;

namespace Game.Scripts.GameplayLogic.AI
{
    public class ActionRunner
    {
        private readonly AIContext _context;

        private AIAction _currentAction;

        public ActionRunner(AIContext context)
        {
            _context = context;
        }

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
                _currentAction = action;
                _currentAction.OnEnter(_context);
            }
        }

        public void Tick()
        {
            _currentAction.OnUpdate(_context);
        }
    }
}