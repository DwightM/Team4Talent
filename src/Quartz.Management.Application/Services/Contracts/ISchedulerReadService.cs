using Quartz.Management.Dto.Model;

namespace Quartz.Management.Application.Services.Contracts
{
    public interface ISchedulerReadService
    {
        // Methods.
        OverviewDataDto ReadOverviewData();


        JobDetailDto ReadJobDetail(string name, string group);
    }
}