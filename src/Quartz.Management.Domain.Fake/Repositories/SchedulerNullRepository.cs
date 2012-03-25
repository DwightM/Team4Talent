using System;
using System.Collections.Generic;
using Quartz.Management.Domain.Model;
using Quartz.Management.Domain.Repositories;

namespace Quartz.Management.Domain.Fake.Repositories
{
    public class SchedulerNullRepository
        : ISchedulerRepository
    {
        public List<JobGroup> ReadAllJobInformation()
        {
            return new List<JobGroup>();
        }
    }
}