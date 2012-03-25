using System;
using System.Runtime.Serialization;
using Quartz.Management.Shared;

namespace Quartz.Management.Dto.Model
{
    [DataContract]
    public class TriggerDto
    {
        // Properties.
        [DataMember]
        public string Name
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
        public DateTime StartDateTime
        {
            get;
            set;
        }


        [DataMember]
        public DateTime? EndDateTime
        {
            get;
            set;
        }


        [DataMember]
        public DateTime? PreviousFireDateTime
        {
            get;
            set;
        }


        [DataMember]
        public DateTime? NextFireDateTime
        {
            get;
            set;
        }


        [DataMember]
        public ETriggerActivityStatus ActivityStatus
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
    }
}