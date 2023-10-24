using Game.Scripts.Common.Interfaces;
using Game.Scripts.GameplayLogic.ResourceManagement;

namespace Game.Scripts.GameplayLogic.JobManagement
{
    public class JobFactory
    {
        public Job CreateJob(JobCategory jobCategory, IGatherableResource resource)
        {
            Job job = new Job(jobCategory, (int)jobCategory, new JobData
            {
                Node = resource
            });

            return job;
        }
        
        public Job CreateJob(JobCategory jobCategory, IResourceProvider resourceProvider)
        {
            Job job = new Job(jobCategory, (int)jobCategory, new JobData
            {
                Provider = resourceProvider
            });

            return job;
        }
    }
}