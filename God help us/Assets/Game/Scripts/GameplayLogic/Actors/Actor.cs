using System.Collections.Generic;
using Game.Scripts.Data.Actors;
using Game.Scripts.GameplayLogic.AI;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Actors
{
    [RequireComponent(typeof(AIAgent))]
    public abstract class Actor : MonoBehaviour
    {
        protected AIAgent AiAgent;
        protected ActorData Data;
        
        public void Construct(ActorData data)
        {
            Data = data;
            AiAgent = GetComponent<AIAgent>();
        }

        public virtual void Init()
        {
            AiAgent.Init();
        }
    }
    
    public class ActorData
    {
        public List<AIAction> Actions;
        public AIAction DefaultAction;
        public ActorType Type;
    }
}