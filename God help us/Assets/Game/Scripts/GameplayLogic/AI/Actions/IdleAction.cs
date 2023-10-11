﻿using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Actions
{
    [CreateAssetMenu(fileName = "Idle", menuName = "AI/Actions/Idle")]
    public class IdleAction : AIAction
    {
        public override void OnEnter(AIContext context)
        {
            Debug.Log("Enter on Idle");
            base.OnEnter(context);
        }

        public override void OnExit(AIContext context)
        {
            Debug.Log("Exit on Idle");
        }
    }
}