using MoviesServices.Models;
using MoviesServices.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesServices.Services
{
    public interface ICategoryRepository
    {
        Task<List<CategoryViewModel>> GetCategories();
        Task<int> AddCategory(Category category);
        Task<int> DeleteCategory(int? categoryID);
        Task <int> UpdateCategory(Category category);
    }
}
