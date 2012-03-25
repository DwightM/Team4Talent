using System;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Job;

namespace Quartz.ConsoleSpike
{
    class Program
    {
        static void Main()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            var job = JobBuilder.Create()
                                .OfType(typeof(NoOpJob))
                                .WithDescription("My first Quartz.NET job")
                                .WithIdentity("A no operation job", "Default")
                                .Build();

            var trigger = TriggerBuilder.Create()
                                        .StartNow()
                                        .WithDescription("Fires every 5 minutes")
                                        .WithIdentity("A trigger that fires every 5 minutes", "Default")
                                        .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(5))
                                        .Build();

            scheduler.ScheduleJob(job, trigger);

            var jobGroupNames = scheduler.GetJobGroupNames();
            foreach (var jobGroupName in jobGroupNames)
            {
                var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(jobGroupName));

                foreach (var jobKey in jobKeys)
                {
                    var jobDetail = scheduler.GetJobDetail(jobKey);

                    var jobTriggers = scheduler.GetTriggersOfJob(jobKey);
                    
                    Console.WriteLine("Job with name '{0}' in group '{1}' having description '{2}' with {3} trigger(s).", jobDetail.Key.Name, jobDetail.Key.Group, jobDetail.Description, jobTriggers.Count);
                }
            }

            Console.WriteLine("\r\nPress any key to quit...");
            Console.ReadKey(true);

            scheduler.Shutdown();
        }
    }
}