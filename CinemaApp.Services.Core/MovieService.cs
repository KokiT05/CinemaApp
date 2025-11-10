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
                                                ReleaseDate = m.ReleaseDate.ToString(ReleaseDateFormat),
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

		public async Task<MovieDetailsViewModel> GetByIdAsync(string id)
		{
			Movie? movie = await this._dbContext.Movies.AsNoTracking()
                                                .FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie == null)
            {
                return null;
            }

            MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel()
            {
                Id = movie.Id.ToString(),
                Title = movie.Title,
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate.ToString("yyyy-MM-dd"),
                Director = movie.Director,
                Description = movie.Description,
                Duration = movie.Duration,
                ImageUrl = movie.ImageUrl,
            };

            return movieDetailsViewModel;
        }

        public async Task<MovieFormViewModel?> GetForEditByIdAsync(string id)
        {
            MovieFormViewModel? movieFormViewModel = await this._dbContext.Movies
                                                    .Where(m => m.Id.ToString() == id)
                                                    .Select(m => new MovieFormViewModel()
                                                    {
                                                        Id = m.Id.ToString(),
                                                        Title = m.Title,
                                                        Genre = m.Genre,
                                                        Director = m.Director,
                                                        Description = m.Description,
                                                        Duration = m.Duration,
                                                        ReleaseDate = m.ReleaseDate
                                                                    .ToString(ReleaseDateFormat),
                                                        ImageUrl = m.ImageUrl,
                                                    }).FirstOrDefaultAsync();

            return movieFormViewModel;
        }

        // the method can return bool
        public async Task EditAsync(string id, MovieFormViewModel movieFormViewModel)
        {
            Movie? movie = await this._dbContext.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie == null)
            {
                return;
            }

            movie.Title = movieFormViewModel.Title;
            movie.Genre = movieFormViewModel.Genre;
            movie.Director = movieFormViewModel.Director;
            movie.Description = movieFormViewModel.Description;
            movie.Duration = movieFormViewModel.Duration;
            movie.ReleaseDate = DateOnly.ParseExact(movieFormViewModel.ReleaseDate, ReleaseDateFormat,
                                            CultureInfo.InvariantCulture);
            movie.ImageUrl = movieFormViewModel.ImageUrl;

            await this._dbContext.SaveChangesAsync();
        }

        // The method can return bool
        public async Task SoftDeleteAsync(string id)
        {
            Movie? movie = await this._dbContext.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie != null && movie.IsDeleted == false)
            {
                movie.IsDeleted = true;
                await this._dbContext.SaveChangesAsync();
            }
        }

        // The method can return bool
        public async Task HardDeleteAsync(string id)
        {
            Movie? movie = await this._dbContext.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie != null)
            {
                this._dbContext.Movies.Remove(movie);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
