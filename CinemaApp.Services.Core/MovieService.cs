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
    using static CinemaApp.Data.Common.EntityConstants.Movie;
    using static CinemaApp.GCommon.ApplicationConstants;
    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext cinemaAppDbContext;
        public MovieService(CinemaAppDbContext cinemaAppDbContext)
        {
            this.cinemaAppDbContext = cinemaAppDbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await this.cinemaAppDbContext.Movies
                                                                .Where(m => !m.IsDeleted)
                                                                .AsNoTracking()
                                                                .Select(m => new AllMoviesIndexViewModel
                                                                {
                                                                    Id = m.Id.ToString(),
                                                                    Title = m.Title,
                                                                    Genre = m.Genre,
                                                                    ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                                                                    Director = m.Director,
                                                                    ImageUrl = m.ImageUrl
                                                                }).ToListAsync();

            foreach (AllMoviesIndexViewModel movie in allMovies)
            {
                if (string.IsNullOrEmpty(movie.ImageUrl))
                {
                    movie.ImageUrl = $"/images/{NoImageUrl}";
                }
            }

            return allMovies;
        }

        public async Task AddMovieAsync(MovieFormInputModel movieFormInputModel)
        {
            Movie movie = new Movie()
            {
                Title = movieFormInputModel.Title,
                Genre = movieFormInputModel.Genre,
                Director = movieFormInputModel.Director,
                ReleaseDate = DateOnly.ParseExact(movieFormInputModel.ReleaseDate,
                                                    AppDateFormat,
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None),
                Duration = movieFormInputModel.Duration,
                Description = movieFormInputModel.Description,
                ImageUrl = movieFormInputModel.ImageUrl
            };

            await this.cinemaAppDbContext.Movies.AddAsync(movie);
            await this.cinemaAppDbContext.SaveChangesAsync();
        }

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id)
        {
            MovieDetailsViewModel? movieDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid movieId);

            if (isIdValidGuid)
            {
                movieDetails = await this.cinemaAppDbContext.Movies
                                                        .AsNoTracking()
                                                        .Where(m => m.Id == movieId)
                                                        .Select(m => new MovieDetailsViewModel()
                                                        {
                                                            Id = m.Id.ToString(),
                                                            Title = m.Title,
                                                            Genre = m.Genre,
                                                            Description = m.Description,
                                                            Director = m.Director,
                                                            Duration = m.Duration,
                                                            ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                                                            ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}"
                                                        }).SingleOrDefaultAsync();
            }

            return movieDetails;
        }

        public async Task<MovieFormInputModel?> GetEditableMovieByIdAsync(string? id)
        {
            MovieFormInputModel? movieEdit = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid movieId);

            if (isIdValidGuid)
            {
                movieEdit = await this.cinemaAppDbContext.Movies
                                                        .AsNoTracking()
                                                        .Where(m => m.Id == movieId)
                                                        .Select(m => new MovieFormInputModel()
                                                        {
                                                            Title = m.Title,
                                                            Genre = m.Genre,
                                                            Description = m.Description,
                                                            Director = m.Director,
                                                            Duration = m.Duration,
                                                            ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                                                            ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}"
                                                        }).SingleOrDefaultAsync();
            }

            return movieEdit;
        }

        public async Task<bool> EditMovieAsync(MovieFormInputModel movieFormInputModel)
        {
            Movie? editMovie = await this.cinemaAppDbContext.Movies
                                            .SingleOrDefaultAsync(m => m.Id.ToString() == movieFormInputModel.Id);

            if (editMovie == null)
            {
                return false;
            }

            DateOnly movieReleaseDate = DateOnly.ParseExact
                                            (movieFormInputModel.ReleaseDate, AppDateFormat, 
                                            CultureInfo.InvariantCulture, DateTimeStyles.None);

            editMovie.Title = movieFormInputModel.Title;
            editMovie.Genre = movieFormInputModel.Genre;
            editMovie.Director = movieFormInputModel.Director;
            editMovie.ReleaseDate = movieReleaseDate;
            editMovie.Duration = movieFormInputModel.Duration;
            editMovie.Description = movieFormInputModel.Description;
            editMovie.ImageUrl = movieFormInputModel.ImageUrl ?? $"~/images/{NoImageUrl}";

            await this.cinemaAppDbContext.SaveChangesAsync();

            return true;
        }

        public async Task SoftDeleteAsync(string id)
        {
            Movie? movie = await this.cinemaAppDbContext.Movies
                                    .FirstOrDefaultAsync(m => m.Id.ToString() == id && m.IsDeleted == false);

            if (movie != null)
            {
                movie.IsDeleted = true;
                await this.cinemaAppDbContext.SaveChangesAsync();
            }
        }

        public async Task HardDeleteAsync(string id)
        {
            Movie? movie = await this.cinemaAppDbContext.Movies
                                    .FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie != null)
            {
                this.cinemaAppDbContext.Remove(movie);
                await this.cinemaAppDbContext.SaveChangesAsync();
            }


        }
    }
}
