using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz.Management.Presentation.Mvc.Controllers;

namespace Quartz.Management.Presentation.Mvc.Test.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        // Methods.
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }


        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}