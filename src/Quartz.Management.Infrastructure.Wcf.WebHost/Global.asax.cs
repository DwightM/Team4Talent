using System;
using Ninject;
using Ninject.Extensions.Wcf;

namespace Quartz.Management.Infrastructure.Wcf.WebHost
{
    public class Global
        : NinjectWcfApplication
    {
        protected override IKernel CreateKernel()
        {
            return new StandardKernel(new FacadeServiceModule());
        }
    }
}