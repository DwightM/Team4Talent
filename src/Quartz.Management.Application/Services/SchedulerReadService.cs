using System;
using System.Linq;
using Quartz.Management.Application.Services.Contracts;
using Quartz.Management.Domain.Repositories;
using Quartz.Management.Dto.Model;

namespace Quartz.Management.Application.Services
{
    public class SchedulerReadService
        : ISchedulerReadService
    {
        private readonly ISchedulerRepository _SchedulerRepository;



        public SchedulerReadService(ISchedulerRepository schedulerRepository)
        {
            if (schedulerRepository == null)
            {
                throw new ArgumentException("Dependency should not be null.", "schedulerRepository");
            }

            _SchedulerRepository = schedulerRepository;
        }


        public OverviewDataDto ReadOverviewData()
        {
            var _scheduler = _SchedulerRepository.ReadScheduler();

            var _schedulerDto = new SchedulerDto
                                {
                                    SchedulerName = _scheduler.Name,
                                    RunningSince = _scheduler.RunningSince,
                                    TotalJobs = _scheduler.TotalJobs,
                                    ExecutedJobs = _scheduler.ExecutedJobs,
                                    InstanceId = _scheduler.InstanceId,
                                    IsRemote = _scheduler.IsRemote,
                                    SchedulerType = _scheduler.SchedulerType,
                                    Status = _scheduler.Status
                                };

            // Add all jobgroups.
            foreach (var _jobGroup in _scheduler.JobGroups)
            {
                var _jobGroupDto = new JobGroupDto
                                   {
                                       Name = _jobGroup.Name,
                                       ActivityStatus = _jobGroup.ActivityStatus,
                                       CanStart = _jobGroup.CanStart,
                                       CanPause = _jobGroup.CanPause
                                   };

                // Add all jobs.
                foreach (var _job in _jobGroup.Jobs)
                {
                    var _jobDto = new JobDto
                                  {
                                      Name = _job.Name,
                                      Description = _job.Description,
                                      GroupName = _job.JobGroup.Name,
                                      FullName = _job.FullName,
                                      IsDurable = _job.IsDurable,
                                      IsVolatile = _job.IsVolatile,
                                      JobType = _job.JobType.FullName,
                                      ActivityStatus = _job.ActivityStatus,
                                      CanStart = _job.CanStart,
                                      CanPause = _job.CanPause,
                                      CanExecuteNow = _job.CanExecuteNow
                                  };

                    // Add all triggers.
                    foreach (var _trigger in _job.Triggers)
                    {
                        var _triggerDto = new TriggerDto
                                          {
                                              Name = _trigger.Name,
                                              GroupName = _trigger.TriggerGroup.Name,
                                              StartDateTime = _trigger.StartDateTime,
                                              EndDateTime = _trigger.EndDateTime,
                                              PreviousFireDateTime = _trigger.PreviousFireDateTime,
                                              NextFireDateTime = _trigger.NextFireDateTime,
                                              ActivityStatus = _trigger.ActivityStatus,
                                              CanStart = _trigger.CanStart,
                                              CanPause = _trigger.CanPause
                                          };

                        _jobDto.Triggers.Add(_triggerDto);
                    }

                    _jobGroupDto.Jobs.Add(_jobDto);
                }

                _schedulerDto.JobsGroups.Add(_jobGroupDto);
            }

            var _overviewDataDto = new OverviewDataDto
                                   {
                                       Scheduler = _schedulerDto
                                   };

            return _overviewDataDto;
        }


        public JobDetailDto ReadJobDetail(string name, string group)
        {
            var _scheduler = _SchedulerRepository.ReadScheduler();

            var _schedulerDto = new SchedulerDto
                                {
                                    SchedulerName = _scheduler.Name,
                                    RunningSince = _scheduler.RunningSince,
                                    TotalJobs = _scheduler.TotalJobs,
                                    ExecutedJobs = _scheduler.ExecutedJobs,
                                    InstanceId = _scheduler.InstanceId,
                                    IsRemote = _scheduler.IsRemote,
                                    SchedulerType = _scheduler.SchedulerType,
                                    Status = _scheduler.Status
                                };

            // Find jobgroup.
            var _jobGroup = _scheduler.JobGroups
                                      .SingleOrDefault(x => x.Name == group);
            if (_jobGroup != null)
            {
                var _jobGroupDto = new JobGroupDto
                                   {
                                       Name = _jobGroup.Name,
                                       ActivityStatus = _jobGroup.ActivityStatus,
                                       CanStart = _jobGroup.CanStart,
                                       CanPause = _jobGroup.CanPause
                                   };

                // Find job.
                var _job = _jobGroup.Jobs
                                    .SingleOrDefault(x => x.Name == name);

                if (_job != null)
                {
                    var _jobDto = new JobDto
                                  {
                                      Name = _job.Name,
                                      Description = _job.Description,
                                      GroupName = _job.JobGroup.Name,
                                      FullName = _job.FullName,
                                      IsDurable = _job.IsDurable,
                                      IsVolatile = _job.IsVolatile,
                                      JobType = _job.JobType.FullName,
                                      ActivityStatus = _job.ActivityStatus,
                                      CanStart = _job.CanStart,
                                      CanPause = _job.CanPause,
                                      CanExecuteNow = _job.CanExecuteNow
                                  };

                    // Add all triggers.
                    foreach (var _trigger in _job.Triggers)
                    {
                        var _triggerDto = new TriggerDto
                        {
                            Name = _trigger.Name,
                            GroupName = _trigger.TriggerGroup.Name,
                            StartDateTime = _trigger.StartDateTime,
                            EndDateTime = _trigger.EndDateTime,
                            PreviousFireDateTime = _trigger.PreviousFireDateTime,
                            NextFireDateTime = _trigger.NextFireDateTime,
                            ActivityStatus = _trigger.ActivityStatus,
                            CanStart = _trigger.CanStart,
                            CanPause = _trigger.CanPause
                        };

                        _jobDto.Triggers.Add(_triggerDto);
                    }

                    _jobGroupDto.Jobs.Add(_jobDto);
                }

                _schedulerDto.JobsGroups.Add(_jobGroupDto);
            }

            var _jobDetailDto = new JobDetailDto
                                {
                                    Scheduler = _schedulerDto
                                };

            return _jobDetailDto;
        }
    }
}