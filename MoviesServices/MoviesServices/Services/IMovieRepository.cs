using MoviesServices.Models;
using MoviesServices.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesServices.Services
{
    public interface IMovieRepository
    {
        Task<List<MovieViewModel>> GetMovies();
        Task<int> AddMovie(Movie movie);
        Task<int> DeleteMovie(int? movieID);
        Task<int> UpdateMovie(Movie movie);
    }
}
