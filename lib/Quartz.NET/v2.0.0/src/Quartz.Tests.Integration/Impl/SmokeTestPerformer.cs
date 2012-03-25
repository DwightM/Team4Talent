using System;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Job;
using Quartz.Spi;
using Quartz.Tests.Integration.Impl.AdoJobStore;
using Quartz.Util;

namespace Quartz.Tests.Integration.Impl
{
    public class SmokeTestPerformer
    {
       public void Test(IScheduler scheduler, bool clearJobs, bool scheduleJobs)
       {
           try
           {
               if (clearJobs)
               {
                   scheduler.Clear();
               }

               if (scheduleJobs)
               {
                   ICalendar cronCalendar = new CronCalendar("0/5 * * * * ?");
                   ICalendar holidayCalendar = new HolidayCalendar();

                   // QRTZNET-86
                   ITrigger t = scheduler.GetTrigger(new TriggerKey("NonExistingTrigger", "NonExistingGroup"));
                   Assert.IsNull(t);

                   AnnualCalendar cal = new AnnualCalendar();
                   scheduler.AddCalendar("annualCalendar", cal, false, true);

                   IOperableTrigger calendarsTrigger = new SimpleTriggerImpl("calendarsTrigger", "test", 20, TimeSpan.FromMilliseconds(5));
                   calendarsTrigger.CalendarName = "annualCalendar";

                   JobDetailImpl jd = new JobDetailImpl("testJob", "test", typeof(NoOpJob));
                   scheduler.ScheduleJob(jd, calendarsTrigger);

                   // QRTZNET-93
                   scheduler.AddCalendar("annualCalendar", cal, true, true);

                   scheduler.AddCalendar("baseCalendar", new BaseCalendar(), false, true);
                   scheduler.AddCalendar("cronCalendar", cronCalendar, false, true);
                   scheduler.AddCalendar("dailyCalendar", new DailyCalendar(DateTime.Now.Date, DateTime.Now.AddMinutes(1)), false, true);
                   scheduler.AddCalendar("holidayCalendar", holidayCalendar, false, true);
                   scheduler.AddCalendar("monthlyCalendar", new MonthlyCalendar(), false, true);
                   scheduler.AddCalendar("weeklyCalendar", new WeeklyCalendar(), false, true);

                   scheduler.AddCalendar("cronCalendar", cronCalendar, true, true);
                   scheduler.AddCalendar("holidayCalendar", holidayCalendar, true, true);

                   Assert.IsNotNull(scheduler.GetCalendar("annualCalendar"));

                   JobDetailImpl lonelyJob = new JobDetailImpl("lonelyJob", "lonelyGroup", typeof(SimpleRecoveryJob));
                   lonelyJob.Durable = true;
                   lonelyJob.RequestsRecovery = true;
                   scheduler.AddJob(lonelyJob, false);
                   scheduler.AddJob(lonelyJob, true);

                   string schedId = scheduler.SchedulerInstanceId;

                   int count = 1;

                   JobDetailImpl job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));

                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = true;
                   IOperableTrigger trigger = new SimpleTriggerImpl("trig_" + count, schedId, 20, TimeSpan.FromSeconds(5));
                   trigger.JobDataMap.Add("key", "value");
                   trigger.EndTimeUtc = DateTime.UtcNow.AddYears(10);

                   trigger.StartTimeUtc = DateTime.Now.AddMilliseconds(1000L);
                   scheduler.ScheduleJob(job, trigger);

                   // check that trigger was stored
                   ITrigger persisted = scheduler.GetTrigger(new TriggerKey("trig_" + count, schedId));
                   Assert.IsNotNull(persisted);
                   Assert.IsTrue(persisted is SimpleTriggerImpl);

                   count++;
                   job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));
                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = (true);
                   trigger = new SimpleTriggerImpl("trig_" + count, schedId, 20, TimeSpan.FromSeconds(5));

                   trigger.StartTimeUtc = (DateTime.Now.AddMilliseconds(2000L));
                   scheduler.ScheduleJob(job, trigger);

                   count++;
                   job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryStatefulJob));
                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = (true);
                   trigger = new SimpleTriggerImpl("trig_" + count, schedId, 20, TimeSpan.FromSeconds(3));

                   trigger.StartTimeUtc = (DateTime.Now.AddMilliseconds(1000L));
                   scheduler.ScheduleJob(job, trigger);

                   count++;
                   job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));
                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = (true);
                   trigger = new SimpleTriggerImpl("trig_" + count, schedId, 20, TimeSpan.FromSeconds(4));

                   trigger.StartTimeUtc = (DateTime.Now.AddMilliseconds(1000L));
                   scheduler.ScheduleJob(job, trigger);

                   count++;
                   job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));
                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = (true);
                   trigger = new SimpleTriggerImpl("trig_" + count, schedId, 20, TimeSpan.FromMilliseconds(4500));
                   scheduler.ScheduleJob(job, trigger);

                   count++;
                   job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));
                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = (true);
                   IOperableTrigger ct = new CronTriggerImpl("cron_trig_" + count, schedId, "0/10 * * * * ?");
                   ct.JobDataMap.Add("key", "value");
                   ct.StartTimeUtc = DateTime.Now.AddMilliseconds(1000);

                   scheduler.ScheduleJob(job, ct);

                   count++;
                   job = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));
                   // ask scheduler to re-Execute this job if it was in progress when
                   // the scheduler went down...
                   job.RequestsRecovery = (true);
                   NthIncludedDayTrigger nt = new NthIncludedDayTrigger("nth_trig_" + count, schedId);
                   nt.StartTimeUtc = DateTime.Now.Date.AddMilliseconds(1000);
                   nt.N = 1;

                   scheduler.ScheduleJob(job, nt);

                   job.RequestsRecovery = (true);
                   CalendarIntervalTriggerImpl intervalTrigger = new CalendarIntervalTriggerImpl(
                       "calint_trig_" + count,
                       schedId,
                       DateTime.UtcNow.AddMilliseconds(300),
                       DateTime.UtcNow.AddMinutes(1),
                       IntervalUnit.Second,
                       8);
                   intervalTrigger.JobKey = job.Key;

                   scheduler.ScheduleJob(intervalTrigger);

                   // bulk operations
                   IDictionary<IJobDetail, IList<ITrigger>> info = new Dictionary<IJobDetail, IList<ITrigger>>();
                   IJobDetail detail = new JobDetailImpl("job_" + count, schedId, typeof(SimpleRecoveryJob));
                   ITrigger simple = new SimpleTriggerImpl("trig_" + count, schedId, 20, TimeSpan.FromMilliseconds(4500));
                   List<ITrigger> triggers = new List<ITrigger>();
                   triggers.Add(simple);
                   info[detail] = triggers;

                   scheduler.ScheduleJobs(info, true);

                   Assert.IsTrue(scheduler.CheckExists(detail.Key));
                   Assert.IsTrue(scheduler.CheckExists(simple.Key));

                   // QRTZNET-243
                   scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains("a").DeepClone());
                   scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEndsWith("a").DeepClone());
                   scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupStartsWith("a").DeepClone());
                   scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("a").DeepClone());

                   scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupContains("a").DeepClone());
                   scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEndsWith("a").DeepClone());
                   scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupStartsWith("a").DeepClone());
                   scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals("a").DeepClone());

                   scheduler.Start();

                   Thread.Sleep(TimeSpan.FromSeconds(3));

                   scheduler.PauseAll();

                   scheduler.ResumeAll();

                   scheduler.PauseJob(new JobKey("job_1", schedId));

                   scheduler.ResumeJob(new JobKey("job_1", schedId));

                   scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(schedId));

                   Thread.Sleep(TimeSpan.FromSeconds(1));

                   scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(schedId));

                   scheduler.PauseTrigger(new TriggerKey("trig_2", schedId));
                   scheduler.ResumeTrigger(new TriggerKey("trig_2", schedId));

                   scheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(schedId));

                   Assert.AreEqual(1, scheduler.GetPausedTriggerGroups().Count);

                   Thread.Sleep(TimeSpan.FromSeconds(3));
                   scheduler.ResumeTriggers(GroupMatcher<TriggerKey>.GroupEquals(schedId));
                   
                   Assert.IsNotNull(scheduler.GetTrigger(new TriggerKey("trig_2", schedId)));
                   Assert.IsNotNull(scheduler.GetJobDetail(new JobKey("job_1", schedId)));
                   Assert.IsNotNull(scheduler.GetMetaData());
                   Assert.IsNotNull(scheduler.GetCalendar("weeklyCalendar"));

                   Thread.Sleep(TimeSpan.FromSeconds(20));

                   scheduler.Standby();

                   CollectionAssert.IsNotEmpty(scheduler.GetCalendarNames());
                   CollectionAssert.IsNotEmpty(scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(schedId)));

                   CollectionAssert.IsNotEmpty(scheduler.GetTriggersOfJob(new JobKey("job_2", schedId)));
                   Assert.IsNotNull(scheduler.GetJobDetail(new JobKey("job_2", schedId)));

                   scheduler.DeleteCalendar("cronCalendar");
                   scheduler.DeleteCalendar("holidayCalendar");
                   scheduler.DeleteJob(new JobKey("lonelyJob", "lonelyGroup"));
                   scheduler.DeleteJob(job.Key);

                   scheduler.GetJobGroupNames();
                   scheduler.GetCalendarNames();
                   scheduler.GetTriggerGroupNames();
               }
           }
           finally
           {
               scheduler.Shutdown(false);
           }

       }
    }
}