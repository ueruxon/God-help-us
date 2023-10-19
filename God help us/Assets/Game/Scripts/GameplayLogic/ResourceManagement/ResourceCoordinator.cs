﻿using System;
using System.Collections.Generic;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.Infrastructure.Factories;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.GameplayLogic.ResourceManagement
{
    public class ResourceCoordinator : ITickable
    {
        public event Action ResourceSpawned;
        
        private readonly IConfigProvider _configProvider;
        private readonly ResourceFactory _resourceFactory;
        private readonly JobController _jobController;
        private readonly JobFactory _jobFactory;
        
        private readonly List<ResourceNode> _allNodes;
        private readonly Dictionary<string, ResourceNode> _allNodeById;

        private readonly Queue<Resource> _resources;

        public ResourceCoordinator(IConfigProvider configProvider, 
            ResourceFactory resourceFactory, 
            JobController jobController,
            JobFactory jobFactory)
        {
            _configProvider = configProvider;
            _resourceFactory = resourceFactory;
            _jobController = jobController;
            _jobFactory = jobFactory;

            _allNodeById = new Dictionary<string, ResourceNode>();
            _allNodes = new List<ResourceNode>();
            _resources = new Queue<Resource>();
        }

        public void Init()
        {
            ResourceNodePointsConfig config = _configProvider.GetResourcesNodeConfigOnLevel();

            foreach (ResourcePointData data in config.ResourceNodePoints)
            {
                ResourceNode node = _resourceFactory.CreateResourceNode(data.Type, data.Position);
                node.WorkedOut += SpawnResource;
                node.Init();

                _allNodeById[node.Id] = node;
                _allNodes.Add(node);
            }
        }

        public Queue<Resource> GetAllResourcesOnLevel() => 
            _resources;
        
        public void Tick()
        {
            // for test
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _jobController.AddJob(
                    _jobFactory.CreateJob(JobCategory.Mining, _allNodes.Last()));
            }
            
            if (Input.GetKeyDown(KeyCode.U))
            {
                _jobController.AddJob(
                    _jobFactory.CreateJob(JobCategory.Mining, _allNodes[4]));
            }
        }

        public void SpawnResource(ResourceType resourceType, Vector3 position)
        {
            Resource resource = _resourceFactory.CreateResource(resourceType, position);
            _resources.Enqueue(resource);

            ResourceSpawned?.Invoke();
        }

        public void Cleanup()
        {
            foreach (ResourceNode node in _allNodes) 
                node.WorkedOut -= SpawnResource;
        }
    }
}