using System;
using System.Collections.Generic;
using Game.Scripts.Data.ResourcesData;
using UnityEngine;

namespace Game.Scripts.Data.Buildings
{
    [Serializable]
    public class BuildingData
    {
        public BuildingCategory Category;
        public List<ConstructionData> RequiredResources;
    }

    [Serializable]
    public class ConstructionData
    {
        public ResourceType Type;
        public int ResourceAmount;
    }
}