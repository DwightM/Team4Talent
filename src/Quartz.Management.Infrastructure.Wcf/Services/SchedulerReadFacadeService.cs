using System;
//using System.ServiceModel.Activation;
using Quartz.Management.Application.Services.Contracts;
using Quartz.Management.Dto.Model;
using Quartz.Management.Infrastructure.Wcf.Services.Contracts;

namespace Quartz.Management.Infrastructure.Wcf.Services
{
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchedulerReadFacadeService
        : ISchedulerReadFacadeService
    {
        // Read-only Fields.
        private readonly ISchedulerReadService _SchedulerReadService;



        // Methods.
        public SchedulerReadFacadeService(ISchedulerReadService schedulerReadService)
        {
            if (schedulerReadService == null)
            {
                throw new ArgumentException("Dependency should not be null.", "schedulerReadService");
            }

            _SchedulerReadService = schedulerReadService;
        }


        public OverviewDataDto ReadOverviewData()
        {
            return _SchedulerReadService.ReadOverviewData();
        }


        public JobDetailDto ReadJobDetail(string name, string group)
        {
            return _SchedulerReadService.ReadJobDetail(name, group);
        }
    }
}