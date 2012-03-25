using Quartz.Management.Application.Services;
using Quartz.Management.Application.Services.Contracts;
using Quartz.Management.Domain.Fake.Repositories;
using Quartz.Management.Domain.Repositories;

namespace Quartz.Management.Infrastructure.Wcf.WebHost
{
    public class FacadeServiceModule
        : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ISchedulerReadService>()
                .To<SchedulerReadService>();

            Bind<ISchedulerRepository>()
                .To<FakeSchedulerRepository>();
        }
    }
}