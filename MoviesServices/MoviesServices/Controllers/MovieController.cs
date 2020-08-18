using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;

namespace MoviesServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;
        private readonly ILogger<MovieController> logger;

        public MovieController(IMovieRepository movieRepository, ILogger<MovieController> logger)
        {
            this.movieRepository = movieRepository;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin,Viewer")]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                logger.LogDebug("Getting movies");
                var movies = await movieRepository.GetMovies();
                if (movies == null)
                    return NotFound();
                logger.LogDebug("Movies got succesfully");
                return Ok(movies);
            }
            catch (Exception e)
            {
                logger.LogError("Error getting movies: " + e.Message);
                return BadRequest("Error getting movies");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]Movie model)
        {
            string message;
            logger.LogDebug("Validating movie");
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid movie");
                message = "Invalid movie";
                return BadRequest(message);
            }

            try
            {
                logger.LogDebug("Movie {0} is valid", model.Name);
                var movieId = await movieRepository.AddMovie(model);
                if (movieId > 0)
                {
                    logger.LogInformation("Movie {0} added succesfully", model.Name);
                    return Ok(movieId);
                }
                else
                {
                    logger.LogError("Error adding the movie {0}", model.Name);
                    message = "Error adding the movie";
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error adding movie: {0}", e.Message);
                message = "Error adding the movie";
            }

            return BadRequest(message);
    }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? movieID)
        {
            if (movieID == null)
                return BadRequest("Invalid movie");

            try
            {
                logger.LogDebug("Deleting movie {0}", movieID);
                int result = await movieRepository.DeleteMovie(movieID);
                if (result == Constants.SUCCESS)
                {
                    logger.LogInformation("Movie with ID {0} deleted succesfully", movieID);
                    return Ok();
                }
                logger.LogError("Error deleting movie");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.LogError("Error deleting movie: {0}", e.Message);
                return BadRequest("Error deleting movie");
            }            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody]Movie model, int movieID)
        {
            string message;
            logger.LogDebug("Validating movie");
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid movie");
                message = "Invalid movie";
                return BadRequest(message);
            }

            logger.LogDebug("Movie {0} is valid", model.Name);
            try
            {
                logger.LogDebug("Updating movie");
                model.MovieId = movieID;
                int result = await movieRepository.UpdateMovie(model);
                if (result == Constants.SUCCESS)
                {
                    logger.LogInformation("Movie {0} updated succesfully", model.Name);
                    return Ok();
                }
                logger.LogError("Error updating the movie {0}", model.Name);
                message = "Error updateing the movie";
            }
            catch (Exception e) 
            {
                logger.LogError("Error updating movie: {0}", e.Message);
                message = "Error updateing the movie";
            }

            return BadRequest(message);
        }
    }
}