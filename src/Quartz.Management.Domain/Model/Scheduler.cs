using System;
using System.Collections.ObjectModel;
using Quartz.Management.Domain.Repositories;
using Quartz.Management.Shared;

namespace Quartz.Management.Domain.Model
{
    public class Scheduler
    {
        // Read-only Fields.
        private readonly ISchedulerRepository _SchedulerRepository;



        // Properties.
        public string Name
        {
            get;
            set;
        }


        public string InstanceId
        {
            get;
            set;
        }


        public bool IsRemote
        {
            get;
            set;
        }


        public int TotalJobs
        {
            get;
            set;
        }


        public int ExecutedJobs
        {
            get;
            set;
        }


        public DateTime? RunningSince
        {
            get;
            set;
        }


        public string SchedulerType
        {
            get;
            set;
        }


        public ESchedulerStatus Status
        {
            get;
            set;
        }


        public ReadOnlyCollection<JobGroup> JobGroups
        {
            get
            {
                var _jobGroups = _SchedulerRepository.ReadJobGroups();
                return new ReadOnlyCollection<JobGroup>(_jobGroups);
            }
        }


        // Methods.
        public Scheduler(ISchedulerRepository schedulerRepository)
        {
            _SchedulerRepository = schedulerRepository;
        }
    }
}