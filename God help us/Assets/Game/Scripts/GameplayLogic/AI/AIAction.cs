using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI
{
    public abstract class AIAction : ScriptableObject
    {
        [SerializeField] public string ActionName;
        [SerializeField, Range(1f, 0f)] public float Weight = 1f;

        public ActionType Type;
        public float Score;

        public virtual void Init(AIContext context) { }
        public abstract void OnEnter(AIContext context);
        public abstract void OnExit(AIContext context);

        public virtual void OnUpdate(AIContext context) { }
    }
}