using System;
using System.Collections.Generic;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI;
using Game.Scripts.GameplayLogic.AI.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Data.Actors
{
    [CreateAssetMenu(fileName = "ActorConfig", menuName = "Game/Actor")]
    public class ActorConfig : ScriptableObject
    {
        [EnumToggleButtons] 
        public ActorType Type;
        public Actor Prefab;
        public AIConfig AIConfig;
    }

    [Serializable]
    public class AIConfig
    {
        public AIAction DefaultAction;
        public List<AIAction> Actions;
    }
}