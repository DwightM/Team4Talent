using System.Collections.Generic;
using Quartz.Management.Domain.Model;

namespace Quartz.Management.Domain.Repositories
{
    public interface ISchedulerRepository
    {
        // Methods.
        Scheduler ReadScheduler();


        List<JobGroup> ReadJobGroups();


        IList<Job> ReadJobs(JobGroup jobGroup);


        IList<Trigger> ReadTriggers(Job job);
    }
}