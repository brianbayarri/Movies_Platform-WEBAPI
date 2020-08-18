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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]Users model)
        {
            string message;
            logger.LogDebug("Validating user");
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid user");
                message = "Invalid user";
                return BadRequest(message);
            }

            logger.LogDebug("User {0} is valid", model.Email);
            try
            {
                logger.LogDebug("Adding user");
                var userId = await userRepository.AddUser(model);
                if (userId > 0)
                {
                    logger.LogInformation("User {0} added succesfully", model.Email);
                    return Ok(userId);
                }

                logger.LogError("Error adding the user {0}", model.Email);
                message = "Error adding the user";
            }
            catch (Exception e)
            {
                logger.LogError("Error adding user: {0}", e.Message);
                message = "Error adding the user";
            }

            return BadRequest(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? userID)
        {
            string message;
            if (userID == null)
            {
                message = "Invalid user";
                return BadRequest(message);
            }

            try
            {
                logger.LogDebug("Deleting user {0}", userID);
                int result = await userRepository.DeleteUser(userID);
                if (result == Constants.SUCCESS)
                {
                    logger.LogInformation("User with ID {0} deleted succesfully", userID);
                    return Ok();
                }
                message = "Error deleting user";
            }
            catch (Exception e)
            {
                logger.LogError("Error deleting user: {0}", e.Message);
                message = "Error deleting user";
            }

            return BadRequest(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody]Users model, int userID)
        {
            string message;
            logger.LogDebug("Validating user");
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid user");
                message = "Invalid user";
                return BadRequest(message);
            }

            logger.LogDebug("User {0} is valid", model.Email);
            try
            {
                logger.LogDebug("Updating user");
                model.UserId = userID;
                int result = await userRepository.UpdateUser(model);
                if (result == Constants.SUCCESS)
                {
                    logger.LogInformation("User {0} updated succesfully", model.Email);
                    return Ok();
                }
                logger.LogError("Error updating the category {0}", model.Email);
                return NotFound("Error updating the category");
            }
            catch (Exception e)
            {
                logger.LogError("Error updating user: {0}", e.Message);
                message = "Error updating the user";
            }

            return BadRequest(message);
        }
    }
}