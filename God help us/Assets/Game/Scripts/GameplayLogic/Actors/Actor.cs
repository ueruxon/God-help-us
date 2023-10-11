using System.Collections.Generic;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.AI;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Actors
{
    [RequireComponent(typeof(AIAgent))]
    public class Actor : MonoBehaviour
    {
        private AIAgent _aiAgent;
        private ActorData _data;
        
        public void Construct(ActorData data)
        {
            _data = data;
            _aiAgent = GetComponent<AIAgent>();
        }

        public void Init()
        {
            _aiAgent.Init();
        }
    }
    
    public class ActorData
    {
        public List<AIAction> Actions;
        public AIAction DefaultAction;
        public ActorType Type;
    }
}