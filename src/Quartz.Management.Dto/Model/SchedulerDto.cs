using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Quartz.Management.Shared;

namespace Quartz.Management.Dto.Model
{
    [DataContract]
    public class SchedulerDto
    {
        // Properties.
        [DataMember]
        public string SchedulerName
        {
            get;
            set;
        }


        [DataMember]
        public DateTime? RunningSince
        {
            get;
            set;
        }


        [DataMember]
        public int TotalJobs
        {
            get;
            set;
        }


        [DataMember]
        public int ExecutedJobs
        {
            get;
            set;
        }


        [DataMember]
        public string InstanceId
        {
            get;
            set;
        }


        [DataMember]
        public bool IsRemote
        {
            get;
            set;
        }


        [DataMember]
        public string SchedulerType
        {
            get;
            set;
        }


        [DataMember]
        public List<JobGroupDto> JobsGroups
        {
            get;
            private set;
        }


        [DataMember]
        public ESchedulerStatus Status
        {
            get;
            set;
        }



        // Methods.
        public SchedulerDto()
        {
            JobsGroups = new List<JobGroupDto>();
        }
    }
}