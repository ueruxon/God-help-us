using UnityEngine;

namespace Game.Scripts.GameplayLogic.AI.Systems.Movement
{
    public interface IMovementSystem : ISystem
    {
        public void Init();

        public void CalculatePath(Vector3 targetPosition);
        
        public bool ReachedDestination();

        public float GetRemainingDistance();

        public void SetDestination(Vector3 destination);
        
        public void Stop();
    }
}