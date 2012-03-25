using System.Collections.Generic;
using System.Runtime.Serialization;
using Quartz.Management.Shared;

namespace Quartz.Management.Dto.Model
{
    [DataContract]
    public class JobGroupDto
    {
        // Properties.
        [DataMember]
        public string Name
        {
            get;
            set;
        }


        [DataMember]
        public EJobGroupActivityStatus ActivityStatus
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
        public List<JobDto> Jobs
        {
            get;
            private set;
        }



        // Methods.
        public JobGroupDto()
        {
            Jobs = new List<JobDto>();
        }
    }
}