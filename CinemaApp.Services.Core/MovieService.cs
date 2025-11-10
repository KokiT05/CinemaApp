using CinemaApp.Data;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            List<AllMoviesIndexViewModel> movies = await this._dbContext.Movies
                                            .Where(m => m.IsDeleted == false)
                                            .AsNoTracking()
                                            .Select(m => new AllMoviesIndexViewModel()
                                            {
                                                Id = m.Id.ToString(),
                                                Title = m.Title,
                                                Genre = m.Genre,
                                                ReleaseDate = m.ReleaseDate.ToString("yyyy-MM-dd"),
                                                Director = m.Director,
                                                ImageUrl = m.ImageUrl,

                                            }).ToListAsync();

            return movies;
        }
    }
}
