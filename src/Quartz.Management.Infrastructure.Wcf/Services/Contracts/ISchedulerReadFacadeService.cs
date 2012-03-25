using System.ServiceModel;
using Quartz.Management.Dto.Model;

namespace Quartz.Management.Infrastructure.Wcf.Services.Contracts
{
    [ServiceContract]
    public interface ISchedulerReadFacadeService
    {
        // Methods.
        [OperationContract]
        OverviewDataDto ReadOverviewData();


        [OperationContract]
        JobDetailDto ReadJobDetail(string name, string group);
    }
}