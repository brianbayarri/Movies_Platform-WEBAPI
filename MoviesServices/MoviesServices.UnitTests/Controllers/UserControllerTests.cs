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
    public class UserControllerTests
    {
        private readonly UserController userController;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<ILogger<UserController>> loggerMock;

        public UserControllerTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            loggerMock = new Mock<ILogger<UserController>>();
            userController = new UserController(userRepositoryMock.Object, loggerMock.Object);
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
                UserTypeId = 1
            };

            userRepositoryMock.Setup(ur => ur.AddUser(user)).ReturnsAsync(5);

            var response = await userController.Add(user) as OkObjectResult;

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

            userRepositoryMock.Setup(ur => ur.AddUser(user)).ReturnsAsync(Constants.FAILURE);

            var response = await userController.Add(user) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Error adding the user", response.Value);
        }

        [Fact]
        public async Task Delete_ValidUserId_ReturnsOk()
        {
            userRepositoryMock.Setup(ur => ur.DeleteUser(1)).ReturnsAsync(Constants.SUCCESS);

            var response = await userController.Delete(1) as OkResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Delete_UserDoesNotExists_ReturnsBadRequest()
        {
            userRepositoryMock.Setup(ur => ur.DeleteUser(1)).ReturnsAsync(Constants.FAILURE);

            var response = await userController.Delete(1) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task Delete_InvalidUserId_ReturnsBadRquestWithMessage()
        {
            var response = await userController.Delete(null) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Invalid user", response.Value);
        }

    }
}
