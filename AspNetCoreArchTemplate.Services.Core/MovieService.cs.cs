using AspNetCoreArchTemplate.Data;
using AspNetCoreArchTemplate.Data.Models;
using AspNetCoreArchTemplate.Services.Core.Interfaces;
using AspNetCoreArchTemplate.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreArchTemplate.Services.Core
{
    using static AspNetCoreArchTemplate.Data.Common.EntityConstants.Movie;
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public MovieService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> movies = await this.applicationDbContext.Movies
                                                                .Where(m => !m.IsDeleted)
                                                                .AsNoTracking()
                                                                .Select(m => new AllMoviesIndexViewModel
                                                                {
                                                                    Id = m.Id.ToString(),
                                                                    Title = m.Title,
                                                                    Genre = m.Genre,
                                                                    ReleaseDate = m.ReleaseDate.ToString("yyyy-MM-dd"),
                                                                    Director = m.Director,
                                                                    ImageUrl = m.ImageUrl
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
                ReleaseDate = DateOnly.ParseExact(movieFormViewModel.ReleaseDate,
                                                    ReleaseDateFormat,
                                                    CultureInfo.InvariantCulture),
                Duration = movieFormViewModel.Duration,
                Description = movieFormViewModel.Description,
                ImageUrl = movieFormViewModel.ImageUrl
            };

            await this.applicationDbContext.AddAsync(movie);
            await this.applicationDbContext.SaveChangesAsync();
        }

        public async Task<MovieDetailsViewModel?> GetByIdAsync(string id)
        {
            Movie? movie = await this.applicationDbContext.Movies
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(m => m.Id.ToString() == id &&
                                                                            m.IsDeleted == false);

            if (movie == null)
            {
                return null;
            }

            MovieDetailsViewModel movieDetails = new MovieDetailsViewModel()
            {
                Id = id,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                ReleaseDate = movie.ReleaseDate.ToString(ReleaseDateFormat),
                Duration = movie.Duration,
                Description = movie.Description,
                ImageUrl = movie.ImageUrl
            };

            return movieDetails;
        }

        public async Task<MovieFormViewModel?> GetForEditByIdAsync(string id)
        {
            MovieFormViewModel? movieFormViewModel = await this.applicationDbContext.Movies
                                                        .Where(m => m.Id.ToString() == id && m.IsDeleted == false)
                                                        .Select(m => new MovieFormViewModel()
                                                        {
                                                            Id = id,
                                                            Title = m.Title,
                                                            Genre = m.Genre,
                                                            Director = m.Director,
                                                            ReleaseDate = m.ReleaseDate.ToString(ReleaseDateFormat),
                                                            Duration = m.Duration,
                                                            Description = m.Description,
                                                            ImageUrl = m.ImageUrl

                                                        }).FirstOrDefaultAsync();

            return movieFormViewModel;
        }

        public async Task EditAsync(string id, MovieFormViewModel movieFormViewModel)
        {
            Movie? movie = await this.applicationDbContext.Movies
                                                .FirstOrDefaultAsync(m => m.Id.ToString() == id && m.IsDeleted == false);

            if (movie == null)
            {
                return;
            }

            movie.Title = movieFormViewModel.Title;
            movie.Genre = movieFormViewModel.Genre;
            movie.Director = movieFormViewModel.Director;
            movie.ReleaseDate = DateOnly.ParseExact
                                (movieFormViewModel.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture);
            movie.Duration = movieFormViewModel.Duration;
            movie.Description = movieFormViewModel.Description;
            movie.ImageUrl = movieFormViewModel.ImageUrl;

            await this.applicationDbContext.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string id)
        {
            Movie? movie = await this.applicationDbContext.Movies
                                    .FirstOrDefaultAsync(m => m.Id.ToString() == id && m.IsDeleted == false);

            if (movie != null)
            {
                movie.IsDeleted = true;
                await this.applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task HardDeleteAsync(string id)
        {
            Movie? movie = await this.applicationDbContext.Movies
                                    .FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (movie != null)
            {
                this.applicationDbContext.Remove(movie);
                await this.applicationDbContext.SaveChangesAsync();
            }


        }
    }
}
