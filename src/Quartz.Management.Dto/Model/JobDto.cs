using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Quartz.Management.Shared;

namespace Quartz.Management.Dto.Model
{
    [DataContract]
    public class JobDto
    {
        // Properties.
        [DataMember]
        public string Name
        {
            get;
            set;
        }


        [DataMember]
        public string Description
        {
            get;
            set;
        }


        [DataMember]
        public string GroupName
        {
            get;
            set;
        }


        [DataMember]
        public string FullName
        {
            get;
            set;
        }


        [DataMember]
        public string JobType
        {
            get;
            set;
        }


        [DataMember]
        public bool IsDurable
        {
            get;
            set;
        }


        [DataMember]
        public bool IsVolatile
        {
            get;
            set;
        }


        [DataMember]
        public EJobActivityStatus ActivityStatus
        {
            get;
            set;
        }


        [DataMember]
        public bool CanStart
        {
            get;
            set;
        }


        [DataMember]
        public bool CanPause
        {
            get;
            set;
        }


        [DataMember]
        public bool CanExecuteNow
        {
            get;
            set;
        }


        [DataMember]
        public List<TriggerDto> Triggers
        {
            get;
            private set;
        }



        // Methods.
        public JobDto()
        {
            Triggers = new List<TriggerDto>();
        }
    }
}