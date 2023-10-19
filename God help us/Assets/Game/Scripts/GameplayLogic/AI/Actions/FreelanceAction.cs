using Game.Scripts.GameplayLogic.Buildings;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Freelance", menuName = "AI/Actions/Freelance")]
    public class FreelanceAction : AIAction
    {
        private ProductionBuilding _building;
        
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);

            _building = context.GetRequestedBuilding();
            
            Debug.Log("Go to Freelance");
            
            context.MovementSystem.SetDestination(_building.GetPosition());
            context.Animator.PlayWalking(true);
        }
        
        public override void OnUpdate(AIContext context)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                context.Animator.PlayWalking(false);
                _building.Work();
            }
        }
        
        public override void OnExit(AIContext context)
        {
            _building.Resolve();
            
            Debug.Log("End Freelance");
        }
    }
}