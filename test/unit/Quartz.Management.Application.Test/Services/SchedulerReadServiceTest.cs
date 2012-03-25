using System;
using Moq;
using NUnit.Framework;
using Quartz.Management.Application.Services;
using Quartz.Management.Domain.Repositories;

namespace Quartz.Management.Application.Test.Services
{
    [TestFixture]
    public class SchedulerReadServiceTest
    {
        [Test]
        public void ShouldCreateInstance()
        {
            // Arrange.
            var schedulerRepositoryMock = new Mock<ISchedulerRepository>();

            var schedulerRepository = schedulerRepositoryMock.Object;

            // Act.
            var sut = new SchedulerReadService(schedulerRepository);

            // Assert.
            Assert.IsNotNull(sut);
        }


        [Test]
        public void ShouldNotCreateInstanceWhenSchedulerRepositoryIsNull()
        {
            // Arrange.
            var isArgumentExceptionThrown = false;
            ISchedulerRepository schedulerRepository = null;

            // Act.
            try
            {
                new SchedulerReadService(schedulerRepository);
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName == "schedulerRepository")
                {
                    isArgumentExceptionThrown = true;
                }
            }

            // Assert.
            Assert.IsTrue(isArgumentExceptionThrown);
        }


        //[Test]
        //public void ShouldInvokeReadAllJobInformationOnSchedulerRepositoryWhenReadAllJobGroups()
        //{
        //    // Arrange.
        //    var schedulerRepositoryMock = new Mock<ISchedulerRepository>();
        //    schedulerRepositoryMock.Setup(sr => sr.ReadAllJobInformation());
            
        //    var schedulerRepository = schedulerRepositoryMock.Object;
        //    var sut = new SchedulerReadService(schedulerRepository);

        //    // Act.
        //    sut.ReadAllJobGroups();

        //    // Assert.
        //    schedulerRepositoryMock.Verify(sr => sr.ReadAllJobInformation(), Times.Once());
        //}


        //[Test]
        //public void ShouldReturnEmptyListWhenReadAllJobGroupsHasNoData()
        //{
        //    // Arrange.
        //    var schedulerRepositoryMock = new Mock<ISchedulerRepository>();
        //    schedulerRepositoryMock.Setup(sr => sr.ReadAllJobInformation())
        //                           .Returns(() => null);

        //    var schedulerRepository = schedulerRepositoryMock.Object;
        //    var sut = new SchedulerReadService(schedulerRepository);

        //    // Act.
        //    var jobGroups = sut.ReadAllJobGroups();

        //    // Assert.
        //    Assert.IsNotNull(jobGroups);
        //}
    }
}