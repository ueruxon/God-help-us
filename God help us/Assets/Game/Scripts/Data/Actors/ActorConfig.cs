using System;
using System.Collections.Generic;
using Game.Scripts.GameplayLogic.AI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Data.Actors
{
    [CreateAssetMenu(fileName = "ActorConfig", menuName = "Game/Actor")]
    public class ActorConfig : ScriptableObject
    {
        [EnumToggleButtons] 
        public ActorType Type;
        public AssetReference PrefabReference;
        public AIConfig AIConfig;
    }

    [Serializable]
    public class AIConfig
    {
        public AIAction DefaultAction;
        public List<AIAction> Actions;
    }
}