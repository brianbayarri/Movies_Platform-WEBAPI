using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;
using System.Security.Claims;

namespace MoviesServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin,Viewer")]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                logger.LogDebug("Getting categories");
                var categories = await categoryRepository.GetCategories();
                if (categories == null)
                {
                    logger.LogError("Error getting categories");
                    return NotFound();
                }
                logger.LogDebug("Categories got succesfully");
                return Ok(categories);
            }
            catch (Exception e)
            {
                logger.LogError("Error getting categories: " + e.Message);
                return BadRequest("Error getting categories");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]Category model)
        {
            string message;
            logger.LogDebug("Validating category");
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid category");
                message = "Invalid category";
                return BadRequest(message);
            }

            logger.LogDebug("Category {0} is valid", model.Name);
            try
            {
                logger.LogDebug("Adding category");
                
                var categoryId = await categoryRepository.AddCategory(model);
                if (categoryId > 0)
                {
                    logger.LogInformation("Category {0} added succesfully", model.Name);
                    return Ok(categoryId);
                }

                logger.LogError("Error adding the category {0}", model.Name);
                message = "Error adding the category";
            }
            catch (Exception e)
            {
                logger.LogError("Error adding category: {0}", e.Message);
                message = "Error adding the category";
            }

            return BadRequest(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? categoryID)
        {
            string message;
            if (categoryID == null)
            {
                message = "Invalid category";
                return BadRequest(message);
            }

            try
            {
                logger.LogDebug("Deleting category {0}", categoryID);
                int result = await categoryRepository.DeleteCategory(categoryID);
                if (result == Constants.SUCCESS)
                {
                    logger.LogInformation("Category with ID {0} deleted succesfully", categoryID);
                    return Ok();
                }
                else if (result == Constants.CATEGORY_HAS_ASSOCIATED_MOVIES)
                    message = "The category has associated movies";
                else
                    message = "Error deleting category";
            }
            catch (Exception e)
            {
                logger.LogError("Error deleting category: {0}", e.Message);
                message = "Error deleting category";
            }

            return BadRequest(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody]Category model, int categoryID)
        {
            string message;
            logger.LogDebug("Validating category");
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid category");
                message = "Invalid category";
                return BadRequest(message);
            }

            logger.LogDebug("Category {0} is valid", model.Name);
            try
            {
                logger.LogDebug("Updating category");
                model.CategoryId = categoryID;
                int result = await categoryRepository.UpdateCategory(model);
                if (result == Constants.SUCCESS)
                {
                    logger.LogInformation("Category {0} updated succesfully", model.Name);
                    return Ok();
                }
                logger.LogError("Error updating the category {0}", model.Name);
                return NotFound("Error updating the category");
            }
            catch (Exception e)
            {
                logger.LogError("Error updating category: {0}", e.Message);
                message = "Error updating the category";
            }

            return BadRequest(message);
        }
    }
}