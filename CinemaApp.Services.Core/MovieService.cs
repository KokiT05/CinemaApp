using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    using static Data.Common.EntityConstants.Movie;
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

        public async Task AddAsync(MovieFormViewModel movieFormViewModel)
        {
            Movie movie = new Movie()
            {
                Title = movieFormViewModel.Title,
                Genre = movieFormViewModel.Genre,
                Director = movieFormViewModel.Director,
                Description = movieFormViewModel.Description,
                Duration = movieFormViewModel.Duration,
                ReleaseDate = DateOnly.ParseExact(movieFormViewModel.ReleaseDate, ReleaseDateFormat,
                                                    CultureInfo.InvariantCulture),
                ImageUrl = movieFormViewModel.ImageUrl,
            };

            await this._dbContext.Movies.AddAsync(movie);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
