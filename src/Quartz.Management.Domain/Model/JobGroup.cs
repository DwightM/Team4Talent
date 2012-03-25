using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Quartz.Management.Domain.Repositories;
using Quartz.Management.Shared;

namespace Quartz.Management.Domain.Model
{
    public class JobGroup
    {
        // Read-only Fields.
        private readonly ISchedulerRepository _SchedulerRepository;



        // Fields.
        private EJobGroupActivityStatus _ActivityStatus;



        // Properties.
        public string Name
        {
            get;
            private set;
        }


        public EJobGroupActivityStatus ActivityStatus
        {
            get
            {
                if (Jobs == null || Jobs.Count == 0)
                {
                    _ActivityStatus = EJobGroupActivityStatus.Complete;
                }
                else if (Jobs.All(job => job.ActivityStatus == EJobActivityStatus.Complete))
                {
                    _ActivityStatus = EJobGroupActivityStatus.Complete;
                }
                else if (Jobs.All(job => job.ActivityStatus == EJobActivityStatus.Active || job.ActivityStatus == EJobActivityStatus.Complete))
                {
                    _ActivityStatus = EJobGroupActivityStatus.Active;
                }
                else if (Jobs.All(job => job.ActivityStatus == EJobActivityStatus.Paused || job.ActivityStatus == EJobActivityStatus.Complete))
                {
                    _ActivityStatus = EJobGroupActivityStatus.Paused;
                }
                else
                {
                    _ActivityStatus = EJobGroupActivityStatus.Mixed;
                }

                return _ActivityStatus;
            }
        }


        public bool CanStart
        {
            get
            {
                return ActivityStatus == EJobGroupActivityStatus.Paused || ActivityStatus == EJobGroupActivityStatus.Mixed;
            }
        }


        public bool CanPause
        {
            get
            {
                return ActivityStatus == EJobGroupActivityStatus.Active || ActivityStatus == EJobGroupActivityStatus.Mixed;
            }
        }


        public ReadOnlyCollection<Job> Jobs
        {
            get
            {
                var _jobs = _SchedulerRepository.ReadJobs(this);
                return new ReadOnlyCollection<Job>(_jobs);
            }
        }



        // Methods.
        public JobGroup(ISchedulerRepository schedulerRepository, string name)
        {
            _SchedulerRepository = schedulerRepository;

            Name = name;
        }
    }
}