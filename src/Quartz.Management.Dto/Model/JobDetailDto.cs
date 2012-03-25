using System.Runtime.Serialization;

namespace Quartz.Management.Dto.Model
{
    [DataContract]
    public class JobDetailDto
    {
        [DataMember]
        public SchedulerDto Scheduler
        {
            get;
            set;
        }


    }
}