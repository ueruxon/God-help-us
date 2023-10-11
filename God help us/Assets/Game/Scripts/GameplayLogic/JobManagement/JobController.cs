using System.Collections.Concurrent;
using Game.Scripts.Common.DataStructures.PriorityQueue;
using Game.Scripts.GameplayLogic.Actors;
using Game.Scripts.GameplayLogic.Services;

namespace Game.Scripts.GameplayLogic.JobManagement
{
    public class JobController
    {
        private readonly ActorRegistry _actorRegistry;
        
        private readonly PriorityQueue<Job> _requestedJobs;
        private readonly ConcurrentQueue<AssigmentJob> _assignmentJobs;

        public JobController(ActorRegistry actorRegistry)
        {
            _actorRegistry = actorRegistry;
            _requestedJobs = new PriorityQueue<Job>();
            _assignmentJobs = new ConcurrentQueue<AssigmentJob>();
        }

        public void AddJob(Job job)
        {
            _requestedJobs.Enqueue(job);
        }

        public bool HasAnyJob() => 
            _requestedJobs.Count > 0;

        public Job GetPriorityJob(string workerId)
        {
            Job job = _requestedJobs.Dequeue();
            job.ChangeStatus(Job.Status.InWork);

            AssigmentJob assigmentJob = new AssigmentJob(_actorRegistry.GetActor(workerId), job);
            _assignmentJobs.Enqueue(assigmentJob);

            return job;
        }

        public void CompleteJob(Job job)
        {
            
        }

        public void CanceledJob(Job job)
        {
            _requestedJobs.Enqueue(job);
        }
    }

    public class AssigmentJob
    {
        public Actor Worker { get; }
        public Job Job { get; }

        public AssigmentJob(Actor worker, Job job)
        {
            Worker = worker;
            Job = job;
        }
    }
}