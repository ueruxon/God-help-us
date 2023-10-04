using System;
using UnityEngine;

namespace Game.Scripts.Common
{
    public static class UniqueIdGenerator
    {
        public static string GenerateIdForGameObject(GameObject gameObject)
        {
            string id = $"_{Guid.NewGuid().ToString()}";
            return id;
        }
    }
}