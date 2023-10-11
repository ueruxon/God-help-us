using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.Buildings;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.ResourceLogic;
using Game.Scripts.Infrastructure.Factories;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.GameplayLogic.Services
{
    public class ResourceCoordinator : ITickable
    {
        private readonly IConfigProvider _configProvider;
        private readonly ResourceFactory _resourceFactory;
        private readonly JobController _jobController;
        private readonly JobFactory _jobFactory;
        private readonly BuildingRegistry _buildingRegistry;

        private readonly Dictionary<ResourceType, List<ResourceNode>> _allNodesByType = new ();
        private readonly Stack<Resource> _resourcesOnLevel;

        public ResourceCoordinator(IConfigProvider configProvider, 
            ResourceFactory resourceFactory, 
            JobController jobController,
            JobFactory jobFactory,
            BuildingRegistry buildingRegistry)
        {
            _configProvider = configProvider;
            _resourceFactory = resourceFactory;
            _jobController = jobController;
            _jobFactory = jobFactory;
            _buildingRegistry = buildingRegistry;

            foreach (ResourceType value in Enum.GetValues(typeof(ResourceType)))
                _allNodesByType[value] = new List<ResourceNode>();

            _resourcesOnLevel = new Stack<Resource>();
        }

        public void Init()
        {
            ResourceNodePointsConfig config = _configProvider.GetResourcesNodeConfigOnLevel();

            foreach (ResourcePointData data in config.ResourceNodePoints)
            {
                ResourceNode node = _resourceFactory.CreateResourceNode(data.Type, data.Position);
                node.Destroyed += SpawnResource;
                node.Init();

                _allNodesByType[data.Type].Add(node);
            }
        }

        public void Tick()
        {
            // for test
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _jobController.AddJob(
                    _jobFactory.CreateJob(JobCategory.Mining, _allNodesByType[ResourceType.Wood].Last()));
            }
            
            if (Input.GetKeyDown(KeyCode.U))
            {
                _jobController.AddJob(
                    _jobFactory.CreateJob(JobCategory.Mining, _allNodesByType[ResourceType.Wood][4]));
            }
        }

        private void SpawnResource(ResourceType resourceType, Vector3 position)
        {
            Resource resource = _resourceFactory.CreateResource(resourceType, position);
            
            if (RegisterResourceInStorage(resource) == false) 
                _resourcesOnLevel.Push(resource);
        }

        private bool RegisterResourceInStorage(Resource resource)
        {
            List<Storage> storages = _buildingRegistry.GetStorages(resource.Type);
            
            foreach (Storage storage in storages)
            {
                if (storage.IsFull() == false)
                {
                    if (storage.RegisterResource(resource.Id))
                    {
                        _jobController.AddJob(_jobFactory.CreateJob(JobCategory.Collect, resource));
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}