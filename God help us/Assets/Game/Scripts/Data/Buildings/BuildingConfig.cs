using System;
using System.Collections.Generic;
using Game.Scripts.Data.ResourcesData;

namespace Game.Scripts.Data.Buildings
{
    [Serializable]
    public class BuildingConfig
    {
        public List<ConstructionData> RequiredResources;
    }

    [Serializable]
    public class ConstructionData
    {
        public ResourceType Type;
        public int ResourceAmount;
    }
}