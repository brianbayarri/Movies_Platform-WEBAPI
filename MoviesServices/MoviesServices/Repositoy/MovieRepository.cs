using Microsoft.EntityFrameworkCore;
using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesServices.Util;

namespace MoviesServices.Repositoy
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieService_DBContext db;

        public MovieRepository(MovieService_DBContext db)
        {
            this.db = db;
        }

        public async Task<List<MovieViewModel>> GetMovies()
        {
            return await (from m in db.Movie
                            join c in db.Category on m.CategoryId equals c.CategoryId
                            select new MovieViewModel
                            {
                                Id = m.MovieId,
                                name = m.Name,
                                description = m.Description,
                                categoryName = c.Name
                            }).ToListAsync();
        }
        public async Task<int> AddMovie(Movie movie)
        {
            await db.Movie.AddAsync(movie);
            await db.SaveChangesAsync();
            return movie.MovieId;
        }

        public async Task<int> DeleteMovie(int? movieID)
        {
            var movie = await db.Movie.FirstOrDefaultAsync(x => x.MovieId == movieID);
            if (movie == null)
                return Constants.FAILURE;

            db.Movie.Remove(movie);
            return await db.SaveChangesAsync();
        }

        public async Task<int> UpdateMovie(Movie movie)
        {
            if (!db.Movie.Any(t => t.MovieId == movie.MovieId))
                return Constants.FAILURE;

            db.Movie.Update(movie);
            return await db.SaveChangesAsync();
        }
    }
}
