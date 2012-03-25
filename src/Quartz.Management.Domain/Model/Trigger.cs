using System;
using Quartz.Management.Shared;

namespace Quartz.Management.Domain.Model
{
    public class Trigger
    {
        // Properties.
        public string Name
        {
            get;
            private set;
        }


        public DateTime StartDateTime
        {
            get;
            private set;
        }


        public DateTime? EndDateTime
        {
            get;
            private set;
        }


        public DateTime? PreviousFireDateTime
        {
            get;
            private set;
        }


        public DateTime? NextFireDateTime
        {
            get;
            private set;
        }


        public Job Job
        {
            get;
            private set;
        }


        public TriggerGroup TriggerGroup
        {
            get;
            private set;
        }


        public ETriggerActivityStatus ActivityStatus
        {
            get;
            private set;
        }


        public bool CanStart
        {
            get
            {
                return ActivityStatus == ETriggerActivityStatus.Paused;
            }
        }


        public bool CanPause
        {
            get
            {
                return ActivityStatus == ETriggerActivityStatus.Active;
            }
        }
        


        // Methods.
        public Trigger(string name, string group, DateTime startDateTime, DateTime? endDateTime, DateTime? previousFireDateTime, DateTime? nextFireDateTime, ETriggerActivityStatus activityStatus, Job job)
        {
            Name = name;
            TriggerGroup = new TriggerGroup(group);
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            PreviousFireDateTime = previousFireDateTime;
            NextFireDateTime = nextFireDateTime;
            ActivityStatus = activityStatus;
            Job = job;
        }
    }
}