using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Moq;
using MoviesServices.Controllers;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;
using MoviesServices.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MoviesServices.UnitTests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly CategoryController categoryController;
        private readonly Mock<ICategoryRepository> categoryRepositoryMock;
        private readonly Mock<ILogger<CategoryController>> loggerMock;
        private List<CategoryViewModel> listOfCategories;

        public CategoryControllerTests()
        {
            categoryRepositoryMock = new Mock<ICategoryRepository>();
            loggerMock = new Mock<ILogger<CategoryController>>();
            categoryController = new CategoryController(categoryRepositoryMock.Object, loggerMock.Object);
            LoadData();
        }

        [Fact]
        public async Task Get_ValidListOfCategories_ReturnsOk()
        {
            categoryRepositoryMock.Setup(cr => cr.GetCategories()).ReturnsAsync(listOfCategories);

            var response = await categoryController.Get() as OkObjectResult;
            var categories = response.Value as IEnumerable<CategoryViewModel>;

            Assert.Equal(200, response.StatusCode);
            var listItems = Assert.IsType<List<CategoryViewModel>>(categories);
            Assert.Equal(2, listItems.Count);
        }

        [Fact]
        public async Task Get_InvalidListOfCategories_ReturnsNotFound()
        {
            List <CategoryViewModel> invalidListOfCategories = null;
            categoryRepositoryMock.Setup(cr => cr.GetCategories()).ReturnsAsync(invalidListOfCategories);

            var response = await categoryController.Get() as NotFoundResult;

            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task Add_ValidCategory_ReturnsOk()
        {
            Category category = new Category
            {
                CategoryId = 5,
                Name = "Comedy",
                UserId = 1
            };

            categoryRepositoryMock.Setup(cr => cr.AddCategory(category)).ReturnsAsync(5);

            var response = await categoryController.Add(category) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Add_ErrorAddingCategory_ReturnsBadRequestWithMessage()
        {
            Category category = new Category
            {
                CategoryId = 5,
                Name = "Comedy",
                UserId = 1
            };

            categoryRepositoryMock.Setup(cr => cr.AddCategory(category)).ReturnsAsync(Constants.FAILURE);

            var response = await categoryController.Add(category) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Error adding the category", response.Value);
        }

        [Fact]
        public async Task Delete_ValidCategoryID_ReturnsOk()
        {
            categoryRepositoryMock.Setup(cr => cr.DeleteCategory(1)).ReturnsAsync(Constants.SUCCESS);

            var response = await categoryController.Delete(1) as OkResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Delete_CategoryWithAssociatedMovies_ReturnsBadRequestWithMessage()
        {
            categoryRepositoryMock.Setup(cr => cr.DeleteCategory(1)).ReturnsAsync(Constants.CATEGORY_HAS_ASSOCIATED_MOVIES);

            var response = await categoryController.Delete(1) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("The category has associated movies", response.Value);
        }

        [Fact]
        public async Task Delete_CategoryDoesNotExists_ReturnsBadRequestWithMessage()
        {
            categoryRepositoryMock.Setup(cr => cr.DeleteCategory(1)).ReturnsAsync(Constants.FAILURE);

            var response = await categoryController.Delete(1) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Error deleting category", response.Value);
        }

        [Fact]
        public async Task Delete_InvalidCategoryId_ReturnsBadRquestWithMessage()
        {
            var response = await categoryController.Delete(null) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Invalid category", response.Value);
        }

        private void LoadData()
        {
            listOfCategories = new List<CategoryViewModel>()
            {
                new CategoryViewModel() { CategoryId = 1, Name = "Horror" },
                new CategoryViewModel() { CategoryId = 2, Name = "Action" }
            };
        }
    }
}
