#region License

/* 
 * All content copyright Terracotta, Inc., unless otherwise indicated. All rights reserved. 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 * 
 */

#endregion

using System;
using System.IO;

using NUnit.Framework;

using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Job;
using Quartz.Simpl;
using Quartz.Spi;
using Quartz.Xml;

using Rhino.Mocks;

namespace Quartz.Tests.Unit.Xml
{
    /// <summary>
    /// Tests for <see cref="XMLSchedulingDataProcessor" />.
    /// </summary>
    /// <author>Marko Lahma (.NET)</author>
    [TestFixture]
    public class XMLSchedulingDataProcessorTest
    {
        private XMLSchedulingDataProcessor processor;
        private IScheduler mockScheduler;

        [SetUp]
        public void SetUp()
        {
            processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
            mockScheduler = MockRepository.GenerateMock<IScheduler>();
        }

        [Test]
        public void TestScheduling_RichConfiguration()
        {
            Stream s = ReadJobXmlFromEmbeddedResource("RichConfiguration_20.xml");
            processor.ProcessStream(s, null);
            Assert.IsFalse(processor.OverWriteExistingData);
            Assert.IsTrue(processor.IgnoreDuplicates);

            processor.ScheduleJobs(mockScheduler);

            mockScheduler.AssertWasCalled(x => x.ScheduleJob(Arg<ITrigger>.Is.NotNull), options => options.Repeat.Twice());
        }

        [Test]
        public void TestScheduling_MinimalConfiguration()
        {
            Stream s = ReadJobXmlFromEmbeddedResource("MinimalConfiguration_20.xml");
            processor.ProcessStream(s, null);
            Assert.IsFalse(processor.OverWriteExistingData);

            processor.ScheduleJobs(mockScheduler);
        }

        [Test]
        public void TestScheduling_QuartzNet250()
        {
            Stream s = ReadJobXmlFromEmbeddedResource("QRTZNET250.xml");
            processor.ProcessStream(s, null);
            processor.ScheduleJobs(mockScheduler);
            mockScheduler.AssertWasCalled(x => x.AddJob(Arg<IJobDetail>.Is.NotNull, Arg<bool>.Is.Anything), constraints => constraints.Repeat.Twice());
            mockScheduler.AssertWasCalled(x => x.ScheduleJob(Arg<ITrigger>.Is.NotNull), constraints => constraints.Repeat.Twice());
        }

        /// <summary>
        /// The default XMLSchedulingDataProcessor will setOverWriteExistingData(true), and we want to
        /// test programmatically overriding this value.
        /// </summary>
        /// <remarks>
        /// Note that XMLSchedulingDataProcessor#processFileAndScheduleJobs(Scheduler,boolean) will only
        /// read default "quartz_data.xml" in current working directory. So to test this, we must create
        /// this file. If this file already exist, it will be overwritten! 
        /// </remarks>
        [Test]
        public void TestOverwriteFlag()
        {
            // create temp file
            string tempFileName = XMLSchedulingDataProcessor.QuartzXmlFileName;
            using (TextWriter writer = new StreamWriter(tempFileName, false))
            {
                using (StreamReader reader = new StreamReader(ReadJobXmlFromEmbeddedResource("SimpleJobTrigger.xml")))
                {
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                    writer.Close();
                }
            }

            IScheduler scheduler = null;
            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // Let's setup a fixture job data that we know test is not going modify it.
                IJobDetail job = JobBuilder.Create<NoOpJob>()
                    .WithIdentity("job1").UsingJobData("foo", "dont_chg_me").Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("job1").WithSchedule(SimpleScheduleBuilder.RepeatHourlyForever()).Build();
                scheduler.ScheduleJob(job, trigger);

                XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
                try
                {
                    processor.ProcessFileAndScheduleJobs(scheduler, false);
                    Assert.Fail("OverWriteExisting flag didn't work. We should get Exception when overwrite is set to false.");
                }
                catch (ObjectAlreadyExistsException)
                {
                    // This is expected. Do nothing.
                }

                // We should still have what we start with.
                Assert.AreEqual(1, scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("DEFAULT")).Count);
                Assert.AreEqual(1, scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals("DEFAULT")).Count);

                job = scheduler.GetJobDetail(JobKey.Create("job1"));
                String fooValue = job.JobDataMap.GetString("foo");
                Assert.AreEqual("dont_chg_me", fooValue);
            }
            finally
            {
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }

                // shutdown scheduler
                if (scheduler != null)
                {
                    scheduler.Shutdown();
                }
            }
        }


        /** QTZ-187 */

        [Test]
        public void TesDirectivesNoOverwriteWithIgnoreDups()
        {
            // create temp file
            string tempFileName = XMLSchedulingDataProcessor.QuartzXmlFileName;
            using (TextWriter writer = new StreamWriter(tempFileName, false))
            {
                using (StreamReader reader = new StreamReader(ReadJobXmlFromEmbeddedResource("DirectivesNoOverwriteIgnoreDups.xml")))
                {
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                    writer.Close();
                }
            }

            IScheduler scheduler = null;
            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // Setup existing job with same names as in xml data.
                IJobDetail job = JobBuilder.Create<NoOpJob>()
                    .WithIdentity("job1")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("job1")
                    .WithSchedule(SimpleScheduleBuilder.RepeatHourlyForever()).Build();
                
                scheduler.ScheduleJob(job, trigger);

                job = JobBuilder.Create<NoOpJob>()
                    .WithIdentity("job2")
                    .Build();

                trigger = TriggerBuilder.Create().WithIdentity("job2").WithSchedule(SimpleScheduleBuilder.RepeatHourlyForever()).Build();
                scheduler.ScheduleJob(job, trigger);

                // Now load the xml data with directives: overwrite-existing-data=false, ignore-duplicates=true
                ITypeLoadHelper loadHelper = new SimpleTypeLoadHelper();
                loadHelper.Initialize();
                XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(loadHelper);
                processor.ProcessFileAndScheduleJobs(tempFileName, scheduler);
                Assert.AreEqual(2, scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("DEFAULT")).Count);
                Assert.AreEqual(2, scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals("DEFAULT")).Count);
            }
            finally
            {
                if (scheduler != null)
                {
                    scheduler.Shutdown();
                }
            }
        }

        private static Stream ReadJobXmlFromEmbeddedResource(string resourceName)
        {
            string fullName = "Quartz.Tests.Unit.Xml.TestData." + resourceName;
            return new StreamReader(typeof (XMLSchedulingDataProcessorTest).Assembly.GetManifestResourceStream(fullName)).BaseStream;
        }
    }
}