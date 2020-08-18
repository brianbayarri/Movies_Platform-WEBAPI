using Microsoft.AspNetCore.Mvc;
using MoviesServices.Controllers;
using Xunit;

namespace MoviesServices.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Get_WhenCalled_ReturnsOk()
        {
            var homeController = new HomeController();

            var result = homeController.Get() as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
        }
    }
}
