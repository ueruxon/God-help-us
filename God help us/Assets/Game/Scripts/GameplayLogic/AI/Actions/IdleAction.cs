using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Idle", menuName = "AI/Actions/Idle")]
    public class IdleAction : AIAction
    {
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            Debug.Log("Enter on Idle");
            context.MovementSystem.Stop();
        }

        public override void OnExit(AIContext context)
        {
            Debug.Log("Exit on Idle");
        }
    }
}