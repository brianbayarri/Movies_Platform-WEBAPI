using Microsoft.AspNetCore.Mvc;
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
    public class MovieControllerTests
    {
        private readonly MovieController movieController;
        private readonly Mock<IMovieRepository> movieRepositoryMock;
        private readonly Mock<ILogger<MovieController>> loggerMock;
        private List<MovieViewModel> listOfMovies;

        public MovieControllerTests()
        {
            movieRepositoryMock = new Mock<IMovieRepository>();
            loggerMock = new Mock<ILogger<MovieController>>();
            movieController = new MovieController(movieRepositoryMock.Object, loggerMock.Object);
            LoadData();
        }

        [Fact]
        public async Task GetMovies()
        {
            movieRepositoryMock.Setup(mr => mr.GetMovies()).ReturnsAsync(listOfMovies);

            var response = await movieController.Get() as OkObjectResult;
            var movies = response.Value as IEnumerable<MovieViewModel>;

            Assert.Equal(200, response.StatusCode);
            var listItems = Assert.IsType<List<MovieViewModel>>(movies);
            Assert.Equal(2, listItems.Count);
        }

        [Fact]
        public async Task Get_InvalidListOfMovies_ReturnsNotFound()
        {
            List<MovieViewModel> invalidListOfMovies = null;
            movieRepositoryMock.Setup(mr => mr.GetMovies()).ReturnsAsync(invalidListOfMovies);

            var response = await movieController.Get() as NotFoundResult;

            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task Add_ValidMovie_ReturnsOk()
        {
            Movie movie = new Movie
            {
                MovieId = 5,
                Name = "Movie3",
                Description = "Description of film 3",
                Duration = 120,
                CategoryId = 1
            };

            movieRepositoryMock.Setup(mr => mr.AddMovie(movie)).ReturnsAsync(5);

            var response = await movieController.Add(movie) as OkObjectResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Add_ErrorAddingMovie_ReturnsBadRequestWithMessage()
        {
            Movie movie = new Movie
            {
                MovieId = 5,
                Name = "Movie3",
                Description = "Description of film 3",
                Duration = 120,
                CategoryId = 1
            };

            movieRepositoryMock.Setup(mr => mr.AddMovie(movie)).ReturnsAsync(Constants.FAILURE);

            var response = await movieController.Add(movie) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Error adding the movie", response.Value);
        }

        [Fact]
        public async Task Delete_ValidMovieId_ReturnsOk()
        {
            movieRepositoryMock.Setup(mr => mr.DeleteMovie(1)).ReturnsAsync(Constants.SUCCESS);

            var response = await movieController.Delete(1) as OkResult;

            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Delete_MovieDoesNotExists_ReturnsBadRequest()
        {
            movieRepositoryMock.Setup(mr => mr.DeleteMovie(1)).ReturnsAsync(Constants.FAILURE);

            var response = await movieController.Delete(1) as NotFoundResult;

            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task Delete_InvalidMovieId_ReturnsBadRquestWithMessage()
        {
            var response = await movieController.Delete(null) as BadRequestObjectResult;

            Assert.Equal(400, response.StatusCode);
            Assert.Equal("Invalid movie", response.Value);
        }
        private void LoadData()
        {
            listOfMovies = new List<MovieViewModel>()
            {
                new MovieViewModel() { Id = 1, name = "Movie1", description = "Description 1", categoryName = "Horror"  },
                new MovieViewModel() { Id = 2, name = "Movie2", description = "Description 2", categoryName = "Comedy"  }
            };
        }
    }
}
