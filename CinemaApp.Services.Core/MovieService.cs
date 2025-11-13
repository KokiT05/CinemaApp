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
    using static GCommon.ApplicationConstants;
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext dbContext;

        public MovieService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            List<AllMoviesIndexViewModel> movies = await this.dbContext.Movies
                                            .Where(m => m.IsDeleted == false)
                                            .AsNoTracking()
                                            .Select(m => new AllMoviesIndexViewModel()
                                            {
                                                Id = m.Id.ToString(),
                                                Title = m.Title,
                                                Genre = m.Genre,
                                                ReleaseDate = m.ReleaseDate.ToString(ApplicationDateFormat),
                                                Director = m.Director,
                                                ImageUrl = m.ImageUrl,

                                            }).ToListAsync();

            foreach (AllMoviesIndexViewModel movie in movies)
            {
                if (String.IsNullOrEmpty(movie.ImageUrl))
                {
                    movie.ImageUrl = $"~/images/{NoImageUrl}";
                }
            }

            return movies;
        }

        public async Task AddMovieAsync(MovieFormInputModel movieInputModel)
        {
            Movie movie = new Movie()
            {
                Title = movieInputModel.Title,
                Genre = movieInputModel.Genre,
                Director = movieInputModel.Director,
                Description = movieInputModel.Description,
                Duration = movieInputModel.Duration,
                ReleaseDate = DateOnly.ParseExact(movieInputModel.ReleaseDate, ApplicationDateFormat,
                                                    CultureInfo.InvariantCulture, DateTimeStyles.None),
                ImageUrl = movieInputModel.ImageUrl,
            };

            await this.dbContext.Movies.AddAsync(movie);
            await this.dbContext.SaveChangesAsync();
        }

		public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id)
		{
            MovieDetailsViewModel? movieDetailsViewModel = null;

            bool isValidGuid = Guid.TryParse(id, out Guid movieId);
            if (isValidGuid)
            {
                movieDetailsViewModel = await this.dbContext.Movies
                                            .AsNoTracking()
                                            .Where(m => m.Id == movieId) 
                                            .Select(m => new MovieDetailsViewModel()
                                            {
                                                Id = m.Id.ToString(),
                                                Title = m.Title,
                                                Genre = m.Genre,
                                                Director = m.Director,
                                                Description = m.Description,
                                                Duration = m.Duration,
                                                ReleaseDate = m.ReleaseDate.ToString(ApplicationDateFormat),
                                                ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}",
                                            }).SingleOrDefaultAsync();
            }

            return movieDetailsViewModel;
        }

        public async Task<MovieFormInputModel?> GetEditableMovieByIdAsync(string? id)
        {
            MovieFormInputModel? editableMovie = null;

            bool isValidGuid = Guid.TryParse(id, out Guid movieId);
            if (isValidGuid)
            {
                editableMovie = await this.dbContext.Movies
                                            .AsNoTracking()
                                            .Where(m => m.Id == movieId)
                                            .Select(m => new MovieFormInputModel()
                                            {
                                                Title = m.Title,
                                                Genre = m.Genre,
                                                Director = m.Director,
                                                Description = m.Description,
                                                Duration = m.Duration,
                                                ReleaseDate = m.ReleaseDate.ToString(ApplicationDateFormat),
                                                ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}",
                                            }).SingleOrDefaultAsync();
            }

            return editableMovie;
        }

        public async Task<bool> EditMovieAsync(MovieFormInputModel movieInputModel)
        {
            Movie? movie = await this.dbContext.Movies
                .SingleOrDefaultAsync(m => m.Id.ToString() == movieInputModel.Id);

            if (movie == null)
            {
                return false;
            }

            DateOnly movieReleaseDate = DateOnly.ParseExact
                                    (movieInputModel.ReleaseDate, ApplicationDateFormat,
                                    CultureInfo.InvariantCulture, DateTimeStyles.None);

            movie.Title = movieInputModel.Title;    
            movie.Genre = movieInputModel.Genre;
            movie.Director = movieInputModel.Director;
            movie.Description = movieInputModel.Description;
            movie.Duration = movieInputModel.Duration;
            movie.ReleaseDate = movieReleaseDate;
            movie.ImageUrl = movieInputModel.ImageUrl ?? $"~/images/{NoImageUrl}";

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        // The method can return bool
        public async Task SoftDeleteAsync(string id)
        {
            Movie? movie = await this.dbContext.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie != null && movie.IsDeleted == false)
            {
                movie.IsDeleted = true;
                await this.dbContext.SaveChangesAsync();
            }
        }

        // The method can return bool
        public async Task HardDeleteAsync(string id)
        {
            Movie? movie = await this.dbContext.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie != null)
            {
                this.dbContext.Movies.Remove(movie);
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
