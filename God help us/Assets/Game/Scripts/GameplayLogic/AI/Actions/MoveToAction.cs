using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "MoveTo", menuName = "AI/Actions/MoveTo")]
    public class MoveToAction : AIAction
    {
        public override void OnEnter(AIContext context)
        {
            
            Debug.Log("Going to target");
        }

        public override void OnExit(AIContext context)
        {
            Debug.Log("Stopped");
        }

        public override void OnUpdate(AIContext context)
        {
            
        }
    }
}