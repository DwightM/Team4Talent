using NUnit.Framework;
using Quartz.Management.Domain.Model;
using Quartz.Management.Shared;

namespace Quartz.Management.Domain.Test.Model
{
    [TestFixture]
    public class JobTest
    {
        [Test]
        public void ShouldCreateInstance()
        {
            // Arrange.

            // Act.
            var _sut = new Job(null, null, null, null, false, false, null);

            // Assert.
            Assert.IsNotNull(_sut);
        }
    }
}