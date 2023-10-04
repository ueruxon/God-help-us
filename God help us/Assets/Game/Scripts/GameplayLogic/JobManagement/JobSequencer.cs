using Game.Scripts.Common.DataStructures.PriorityQueue;
using Game.Scripts.GameplayLogic.Services.ActorRegistry;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.JobManagement
{
    public class JobSequencer
    {
        private readonly IActorRegistry _actorRegistry;
        
        private readonly PriorityQueue<Job> _jobSequence;

        public JobSequencer(IActorRegistry actorRegistry)
        {
            _actorRegistry = actorRegistry;
            _jobSequence = new PriorityQueue<Job>();
        }

        public void AddJob(Job job)
        {
            _jobSequence.Enqueue(job);
        }

        public bool HasAnyJob()
        {
            return _jobSequence.Count > 0;
        }

        public Job GetPriorityJob()
        {
            Job job = _jobSequence.Dequeue();
            job.ChangeStatus(Job.Status.InWork);
            
            // add to list jobs in work

            return job;
        }

        public void CompleteJob(Job job)
        {
            
        }

        public void CanceledJob(Job job)
        {
            _jobSequence.Enqueue(job);
        }
    }
}