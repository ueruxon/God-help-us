using System;
using Game.Scripts.Common;
using Game.Scripts.Data.ResourcesData;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.ResourceLogic
{
    public class ResourceNodeMarker : MonoBehaviour
    {
        public ResourceType Type;

        private void OnDrawGizmos()
        {
            if (Type == ResourceType.Wood)
                Gizmos.color = ColorHelper.BrownColor;
            if (Type == ResourceType.Stone)
                Gizmos.color = Color.gray;
            if (Type == ResourceType.Coal)
                Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}