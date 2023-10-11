using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Systems.Movement;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Game.Scripts.GameplayLogic.Services;

namespace Game.Scripts.GameplayLogic.AI
{
    public class AIContext
    {
        private readonly BuildingRegistry _buildingRegistry;
        public Backpack Backpack { get; }
        public JobPlanner JobPlanner { get; }
        public IMovementSystem MovementSystem { get; }
        public ActorAnimator Animator { get; }
        
        public AIContext(JobPlanner jobPlanner, IMovementSystem movementSystem,
            ActorAnimator animator,
            Backpack backpack, 
            BuildingRegistry buildingRegistry)
        {
            _buildingRegistry = buildingRegistry;
            Backpack = backpack;
            JobPlanner = jobPlanner;
            Animator = animator;
            MovementSystem = movementSystem;
        }

        public Storage GetStorageForResource(Resource resource)
        {
            List<Storage> storages = _buildingRegistry.GetStorages(resource.Type);

            foreach (Storage storage in storages)
            {
                if (storage.ContainsResource(resource.Id))
                    return storage;
            }
            
            throw new KeyNotFoundException($"No found storage for resource {resource.Id}");
        }
    }
}