using Game.Scripts.GameplayLogic.Buildings;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "JobRequested", menuName = "AI/Actions/Job Requested")]
    public class JobRequestAction : AIAction
    {
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);

            if (context.ThereIsAnyAvailableProductionBuilding())
            {
                ProductionBuilding building = context.GetAvailableProductionBuilding();
                building.Request(context.ActorId);
            }
        }
    }
}