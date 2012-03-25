using System;
using System.Collections.ObjectModel;
using System.Linq;
using Quartz.Management.Domain.Repositories;
using Quartz.Management.Shared;

namespace Quartz.Management.Domain.Model
{
    public class Job
    {
        // Read-only Fields.
        private readonly ISchedulerRepository _SchedulerRepository;



        // Fields.
        private EJobActivityStatus _ActivityStatus;



        // Properties.
        public string Name
        {
            get;
            private set;
        }


        public string Description
        {
            get;
            private set;
        }


        public string FullName
        {
            get
            {
                return String.Format("{0}.{1}", JobGroup.Name, Name);
            }
        }


        public Type JobType
        {
            get;
            private set;
        }


        public bool IsDurable
        {
            get;
            private set;
        }


        public bool IsVolatile
        {
            get;
            private set;
        }


        public JobGroup JobGroup
        {
            get;
            private set;
        }


        public EJobActivityStatus ActivityStatus
        {
            get
            {
                if (Triggers == null || Triggers.Count == 0)
                {
                    _ActivityStatus = EJobActivityStatus.Complete;
                }
                else if (Triggers.All(trigger => trigger.ActivityStatus == ETriggerActivityStatus.Complete))
                {
                    _ActivityStatus = EJobActivityStatus.Complete;
                }
                else if (Triggers.All(trigger => trigger.ActivityStatus == ETriggerActivityStatus.Active || trigger.ActivityStatus == ETriggerActivityStatus.Complete))
                {
                    _ActivityStatus = EJobActivityStatus.Active;
                }
                else if (Triggers.All(trigger => trigger.ActivityStatus == ETriggerActivityStatus.Paused || trigger.ActivityStatus == ETriggerActivityStatus.Complete))
                {
                    _ActivityStatus = EJobActivityStatus.Paused;
                }
                else
                {
                    _ActivityStatus = EJobActivityStatus.Mixed;
                }

                return _ActivityStatus;
            }
        }


        public bool CanStart
        {
            get
            {
                return ActivityStatus == EJobActivityStatus.Paused || ActivityStatus == EJobActivityStatus.Mixed;
            }
        }


        public bool CanPause
        {
            get
            {
                return ActivityStatus == EJobActivityStatus.Active || ActivityStatus == EJobActivityStatus.Mixed;
            }
        }


        public bool CanExecuteNow
        {
            get
            {
                return true;
            }
        }


        public ReadOnlyCollection<Trigger> Triggers
        {
            get
            {
                var _triggers = _SchedulerRepository.ReadTriggers(this);
                return new ReadOnlyCollection<Trigger>(_triggers);
            }
        }



        // Methods.
        public Job(ISchedulerRepository schedulerRepository, string name, string description, Type jobType, bool isDurable, bool isVolatile, JobGroup jobGroup)
        {
            _SchedulerRepository = schedulerRepository;

            Name = name;
            Description = description;
            JobType = jobType;
            IsDurable = isDurable;
            IsVolatile = isVolatile;
            JobGroup = jobGroup;
        }
    }
}