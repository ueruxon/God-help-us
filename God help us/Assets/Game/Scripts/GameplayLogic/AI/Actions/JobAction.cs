using Game.Scripts.GameplayLogic.JobManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "JobAction", menuName = "AI/Actions/Job")]
    public class JobAction : AIAction
    {
        private Job _currentJob;
        
        public override void OnEnter(AIContext context)
        {
            Debug.Log("Enter Job");
            _currentJob = context.GetCurrentJob();
            _currentJob.Enter(context);
        }

        public override void OnUpdate(AIContext context)
        {
           _currentJob.Update(context);
        }

        public override void OnExit(AIContext context)
        {
            _currentJob.Exit(context);
            _currentJob = null;
            
            Debug.Log("Exit Job");
        }
    }
}