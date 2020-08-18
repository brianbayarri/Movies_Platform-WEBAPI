using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.ViewModel;
using MoviesServices.Util;

namespace MoviesServices.Repositoy
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MovieService_DBContext db;

        public CategoryRepository(MovieService_DBContext db)
        {
            this.db = db;
        }

        public async Task<List<CategoryViewModel>> GetCategories()
        {
            return await db.Category.Select(x => new CategoryViewModel { CategoryId = x.CategoryId, Name = x.Name }).ToListAsync();
        }


        public async Task<int> AddCategory(Category category)
        {
            if (db.Category.Any(t => t.Name == category.Name))
                return Constants.FAILURE;

            category.UserId = Constants.USER_ID;
            await db.Category.AddAsync(category);
            var result = await db.SaveChangesAsync();
            if (result != Constants.FAILURE)
                return category.CategoryId;
            return result;
        }

        public async Task<int> DeleteCategory(int? categoryID)
        {
            //Validation if the category has associated movies
            if (db.Movie.Any(t => t.CategoryId == categoryID))
                return Constants.CATEGORY_HAS_ASSOCIATED_MOVIES;

            var category = await db.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryID);
            if (category == null)
                return Constants.FAILURE;
            db.Category.Remove(category);
            return await db.SaveChangesAsync();
        }

        public async Task<int> UpdateCategory(Category category)
        {
            if (db.Category.Any(t => t.Name == category.Name))
                return Constants.FAILURE;

            category.UserId = Constants.USER_ID;
            db.Category.Update(category);
            return await db.SaveChangesAsync();
        }
    }
}
