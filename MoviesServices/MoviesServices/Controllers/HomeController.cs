using Microsoft.AspNetCore.Mvc;

namespace MoviesServices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome!");
        }
    }
}