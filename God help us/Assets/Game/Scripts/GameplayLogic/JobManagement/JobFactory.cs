using Game.Scripts.GameplayLogic.ResourceLogic;

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
        
        public Job CreateJob(JobCategory jobCategory, Resource resource)
        {
            Job job = new Job(jobCategory, (int)jobCategory, new JobData
            {
                Resource = resource
            });

            return job;
        }
    }
}