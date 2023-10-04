using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.GameplayLogic.AI.Systems.Movement
{
    public class AISimpleMover : MonoBehaviour, IMovementSystem
    {
        [SerializeField] private NavMeshAgent _agent;
        
        public void Init()
        {
            //Debug.Log(_agent.isStopped);
        }

        public void CalculatePath(Vector3 targetPosition) => 
            _agent.SetDestination(targetPosition);

        public bool ReachedDestination()
        {
            if (_agent.pathPending == false)
                if (_agent.remainingDistance <= _agent.stoppingDistance + .5f)
                    if (_agent.hasPath == false || _agent.velocity.sqrMagnitude == 0f)
                        return true;

            return false;
        }

        public float GetRemainingDistance() => 
            _agent.remainingDistance;

        public void SetDestination(Vector3 destination) => 
            _agent.SetDestination(destination);

        public void Stop()
        {
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;
        }
    }
}