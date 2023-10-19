using UnityEngine;

namespace Game.Scripts.GameplayLogic.ResourceManagement
{
    public interface IGatherableResource
    {
        public Vector3 GetPosition();
        public bool IsActive();
        void ExtractResource();
    }
}