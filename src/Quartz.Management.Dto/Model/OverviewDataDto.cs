using System.Runtime.Serialization;

namespace Quartz.Management.Dto.Model
{
    [DataContract]
    public class OverviewDataDto
    {
        // Properties.
        [DataMember]
        public SchedulerDto Scheduler
        {
            get;
            set;
        }
    }
}