using System.Collections.Generic;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.AI.Systems.Movement;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.Registers;
using Game.Scripts.GameplayLogic.ResourceManagement;

namespace Game.Scripts.GameplayLogic.AI
{
    public class AIContext
    {
        private readonly BuildingRegistry _buildingRegistry;
        public Backpack Backpack { get; }
        public JobPlanner JobPlanner { get; }
        public IMovementSystem MovementSystem { get; }
        public ActorAnimator Animator { get; }
        public string ActorId { get; }

        public AIContext(string actorId,
            JobPlanner jobPlanner, 
            IMovementSystem movementSystem,
            ActorAnimator animator,
            Backpack backpack, 
            BuildingRegistry buildingRegistry)
        {
            ActorId = actorId;
            Backpack = backpack;
            JobPlanner = jobPlanner;
            Animator = animator;
            MovementSystem = movementSystem;
            _buildingRegistry = buildingRegistry;
        }

        public IResourceRequester GetResourceRequester(Resource resource)
        {
            List<Storage> storages = _buildingRegistry.GetStorages(resource.Type);

            foreach (Storage storage in storages)
            {
                if (storage.ContainsRegisterResource(resource))
                    return storage;
            }
            
            List<Building> buildings = _buildingRegistry.GetAllBuildings();

            foreach (Building building in buildings)
            {
                if (building.ContainsResource(resource))
                    return building;
            }
            
            throw new KeyNotFoundException($"No found storage for resource {resource.Id}");
        }

        public bool ThereIsAnyAvailableProductionBuilding() => 
            _buildingRegistry.CheckAnyAvailableProductionBuilding();

        public ProductionBuilding GetAvailableProductionBuilding() => 
            _buildingRegistry.GetAnyProductionBuilding();

        public ProductionBuilding GetRequestedBuilding() => 
            _buildingRegistry.GetRequesterBuilding(ActorId);
    }
}