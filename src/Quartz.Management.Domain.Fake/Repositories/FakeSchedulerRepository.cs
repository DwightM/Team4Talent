using System;
using System.Collections.Generic;
using Quartz.Management.Domain.Model;
using Quartz.Management.Domain.Repositories;
using Quartz.Management.Shared;

namespace Quartz.Management.Domain.Fake.Repositories
{
    public class FakeSchedulerRepository
        : ISchedulerRepository
    {
        // Methods.
        public Scheduler ReadScheduler()
        {
            return new Scheduler(this)
                   {
                       Name = "QuartzScheduler",
                       InstanceId = "NON_CLUSTERED",
                       IsRemote = false,
                       TotalJobs = 2,
                       ExecutedJobs = 5,
                       RunningSince = new DateTime(2000, 11, 11, 23, 35, 00),
                       SchedulerType = "Quartz.Impl.StdScheduler",
                       Status = ESchedulerStatus.Started
                   };
        }


        public List<JobGroup> ReadJobGroups()
        {
            var _jobGroups = new List<JobGroup>
                             {
                                 new JobGroup(this, "MyOwnGroup"),
                                 new JobGroup(this, "Default")
                             };

            return _jobGroups;
        }


        public IList<Job> ReadJobs(JobGroup jobGroup)
        {
            var _jobs = new List<Job>();

            if (jobGroup.Name == "MyOwnGroup")
            {
                _jobs.Add(new Job(this, "MyJob", "MyDescription", typeof(int), false, false, jobGroup));
            }
            if (jobGroup.Name == "Default")
            {
                _jobs.Add(new Job(this, "MyJob01", "MyDescription", typeof(int), false, false, jobGroup));
                _jobs.Add(new Job(this, "MyJob02", "MyDescription", typeof(int), false, false, jobGroup));
            }

            return _jobs;
        }


        public IList<Trigger> ReadTriggers(Job job)
        {
            var _triggers = new List<Trigger>();

            if (job.JobGroup.Name == "MyOwnGroup")
            {
                if (job.Name == "MyJob")
                {
                    _triggers.Add(new Trigger("MyTrigger01", "Default", new DateTime(2012, 01, 19, 22, 45, 12), new DateTime(2012, 01, 25, 15, 00, 12), new DateTime(2012, 01, 21, 14, 05, 12), new DateTime(2012, 01, 21, 14, 10, 12), ETriggerActivityStatus.Active, job));
                }
            }
            if (job.JobGroup.Name == "Default")
            {
                if (job.Name == "MyJob01")
                {
                    _triggers.Add(new Trigger("", "Default", new DateTime(2012, 01, 19, 22, 46, 43), new DateTime(2012, 01, 19, 22, 46, 48), null, null, ETriggerActivityStatus.Complete, job));
                    _triggers.Add(new Trigger("MyTrigger03", "Default", new DateTime(2012, 01, 19, 22, 46, 43), null, null, new DateTime(2012, 01, 19, 23, 46, 43), ETriggerActivityStatus.Active, job));
                }
                if (job.Name == "MyJob02")
                {
                    _triggers.Add(new Trigger("MyTrigger04", "Default", new DateTime(2012, 01, 19, 22, 46, 43), new DateTime(2012, 01, 19, 22, 46, 48), new DateTime(2012, 01, 19, 22, 46, 43), new DateTime(2012, 01, 19, 22, 46, 48), ETriggerActivityStatus.Paused, job));
                }
            }

            return _triggers;
        }
    }
}