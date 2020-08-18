using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MoviesServices.Controllers;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;
using System.Threading.Tasks;
using Xunit;

namespace MoviesServices.UnitTests.Controllers
{
    public class LoginControllerTests
    {
        private readonly LoginController loginController;
        private readonly Mock<ILoginRepository> loginRepositoryMock;
        private readonly Mock<ILogger<LoginController>> loggerMock;

        public LoginControllerTests()
        {
            loginRepositoryMock = new Mock<ILoginRepository>();
            loggerMock = new Mock<ILogger<LoginController>>();
            loginController = new LoginController(loginRepositoryMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsOk()
        {
            Users user = new Users
            {
                UserId = 5,
                Name = "Brian",
                Surname = "Bayarri",
                Email = "bb@gmail.com",
                Password = "12345",
                UserTypeId = 1
            };

            loginRepositoryMock.Setup(lr => lr.AuthenticateUser("email", "password")).ReturnsAsync(user);
            loginRepositoryMock.Setup(lr => lr.GetRole(user)).ReturnsAsync("role");
            loginRepositoryMock.Setup(lr => lr.GenerateJSONWebToken("name", "email", "role")).Returns("token");

            var response = await loginController.Login("email","password") as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsUnauthorized()
        {
            Users user = null;
            loginRepositoryMock.Setup(lr => lr.AuthenticateUser("email", "password")).ReturnsAsync(user);

            var response = await loginController.Login("email", "password") as UnauthorizedResult;

            Assert.Equal(401, response.StatusCode);
        }

        [Fact]
        public async Task Login_ErrorGettingRole_ReturnsOk()
        {
            string role = null;
            Users user = new Users
            {
                UserId = 5,
                Name = "Brian",
                Surname = "Bayarri",
                Email = "bb@gmail.com",
                Password = "12345",
                UserTypeId = 1
            };

            loginRepositoryMock.Setup(lr => lr.AuthenticateUser("email", "password")).ReturnsAsync(user);
            loginRepositoryMock.Setup(lr => lr.GetRole(user)).ReturnsAsync(role);

            var response = await loginController.Login("email", "password") as UnauthorizedResult;

            Assert.Equal(401, response.StatusCode);
        }

        [Fact]
        public async Task Add_ValidUser_ReturnsOk()
        {
            Users user = new Users
            {
                UserId = 5,
                Name = "Brian",
                Surname = "Bayarri",
                Email = "bb@gmail.com",
                Password = "12345",
            };

            loginRepositoryMock.Setup(lr => lr.AddUser(user)).ReturnsAsync(5);

            var response = await loginController.Create(user) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Add_ErrorAddingUser_ReturnsBadRequestWithMessage()
        {
            Users user = new Users
            {
                UserId = 5,
                Name = "Brian",
                Surname = "Bayarri",
                Email = "bb@gmail.com",
                Password = "12345",
                UserTypeId = 1
            };

            loginRepositoryMock.Setup(lr => lr.AddUser(user)).ReturnsAsync(Constants.FAILURE);

            var response = await loginController.Create(user) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Error adding the user", response.Value);
        }

    }
}
