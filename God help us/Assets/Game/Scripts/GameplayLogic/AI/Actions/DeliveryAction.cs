using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.ResourceManagement;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Delivery", menuName = "AI/Actions/Delivery")]
    public class DeliveryAction : AIAction
    {
        private IResourceRequester _customer;

        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            Resource resource = context.Backpack.GetItem();
            _customer = context.GetResourceRequester(resource);
            
            Debug.Log($"Customer: {_customer.GetPosition()}");

            context.MovementSystem.SetDestination(_customer.GetPosition());
            context.Animator.PlayWalking(true);
        }

        public override void OnUpdate(AIContext context)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                context.Animator.PlayWalking(false);
                
                _customer.Delivery(context.Backpack.GetItem());
                context.Backpack.Drop();

                ChangeStatus(ActionStatus.Completed);
            }
        }

        public override void OnExit(AIContext context)
        {
            _customer = null;
        }
    }
}