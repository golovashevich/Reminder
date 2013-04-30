using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Web.Controllers;

namespace Web.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Index("Test message") as ViewResult;

			// Assert
			Assert.AreEqual("Test message", result.ViewBag.ResultMessage);
		}
	}
}
