using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;

namespace MoviesServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository loginRepository;
        private readonly ILogger<LoginController> logger;

        public LoginController(ILoginRepository loginRepository, ILogger<LoginController> logger)
        {
            this.loginRepository = loginRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string userEmail, string password)
        {
            IActionResult response = Unauthorized();
            logger.LogDebug("Authenticating user {0}", userEmail);
            var user = await loginRepository.AuthenticateUser(userEmail, password);
            if (user == null)
            {
                logger.LogError("Invalid user: {0}", userEmail);
                return response;
            }
            string role = await loginRepository.GetRole(user);
            if (role == null)
            {
                logger.LogError("Error logging user {0}, invalid role", userEmail);
                return response;
            }

            logger.LogDebug("Generating token");
            var tokenStr = loginRepository.GenerateJSONWebToken(user.Name, user.Email, role);
            response = Ok(new { token = tokenStr });
            logger.LogDebug("Token generated succesfully");
            logger.LogInformation("User {0} is now connected", userEmail);
            Util.Constants.USER_ID = user.UserId;
            return response;
        }

        
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]Users model)
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
                var userId = await loginRepository.AddUser(model);
                if (userId > 0)
                {
                    logger.LogInformation("User {0} added succesfully", model.Email);
                    return Ok(userId);
                }
                else
                {
                    logger.LogError("Error adding the user {0}", model.Email);
                    message = "Error adding the user";
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error adding user: {0}", e.Message);
                message = "Error adding the user";
            }

            return BadRequest(message);
        }
    }
}