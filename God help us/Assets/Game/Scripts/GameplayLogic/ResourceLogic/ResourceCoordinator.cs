using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.ResourcesData;
using Game.Scripts.GameplayLogic.JobManagement;
using Game.Scripts.GameplayLogic.JobManagement.Jobs;
using Game.Scripts.Infrastructure.Factories;
using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.GameplayLogic.ResourceLogic
{
    public class ResourceCoordinator : ITickable
    {
        private readonly IConfigProvider _configProvider;
        private readonly ResourceFactory _resourceFactory;
        private readonly JobSequencer _jobSequencer;

        private readonly Dictionary<ResourceType, List<ResourceNode>> _allNodesByType = new ();

        public ResourceCoordinator(IConfigProvider configProvider, ResourceFactory resourceFactory, JobSequencer jobSequencer)
        {
            _configProvider = configProvider;
            _resourceFactory = resourceFactory;
            _jobSequencer = jobSequencer;

            foreach (ResourceType value in Enum.GetValues(typeof(ResourceType)))
                _allNodesByType[value] = new List<ResourceNode>();
        }

        public void Init()
        {
            ResourceNodePointsConfig config = _configProvider.GetResourcesNodeConfigOnLevel();

            foreach (ResourcePointData data in config.ResourceNodePoints)
            {
                ResourceNode node = _resourceFactory.CreateResourceNode(data.Type, data.Position);
                node.Destroyed += OnNodeWasDestroyed;
                node.Init();

                _allNodesByType[data.Type].Add(node);
            }
        }

        public void Tick()
        {
            // for test
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _jobSequencer.AddJob(new MiningJob(_allNodesByType[ResourceType.Wood].Last(), 2));
            }
            
            if (Input.GetKeyDown(KeyCode.U))
            {
                _jobSequencer.AddJob(new MiningJob(_allNodesByType[ResourceType.Wood][4], 2));
            }
        }

        private void OnNodeWasDestroyed()
        {
            Debug.Log("Ресурс был уничтожен");
        }
    }
}