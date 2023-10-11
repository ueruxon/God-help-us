using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.ResourceLogic;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Delivery", menuName = "AI/Actions/Delivery")]
    public class DeliveryAction : AIAction
    {
        private Storage _customer;
        
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            Resource resource = context.Backpack.GetItem();
            _customer = context.GetStorageForResource(resource);
            
            context.MovementSystem.SetDestination(_customer.GetPosition());
            context.Animator.PlayWalking(true);
        }

        public override void OnUpdate(AIContext context)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                context.Animator.PlayWalking(false);
                context.Backpack.Drop();

                _customer.Delivery();
                
                ChangeStatus(ActionStatus.Completed);
            }
        }

        public override void OnExit(AIContext context)
        {
            _customer = null;
        }
    }
}