using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    using static CinemaApp.GCommon.ApplicationConstants;
    public class WatchlistService : IWatchlistService
    {
        private readonly CinemaAppDbContext cinemaAppDbContext;

        public WatchlistService(CinemaAppDbContext cinemaAppDbContext)
        {
            this.cinemaAppDbContext = cinemaAppDbContext;
        }

        public async Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId)
        {


            List<WatchlistViewModel> watchlistViewModels = await this.cinemaAppDbContext.UserMovies
                                                            .AsNoTracking()
                                                            .Where(um => um.UserId == userId)
                                                            .Select(um => new WatchlistViewModel()
                                                            {
                                                                MovieId = um.MovieId.ToString(),
                                                                Title = um.Movie.Title,
                                                                Genre = um.Movie.Genre,
                                                                ReleaseDate = um.Movie.ReleaseDate.ToString(AppDateFormat),
                                                                ImageUrl = um.Movie.ImageUrl ?? $"/images/{NoImageUrl}",
                                                            }).ToListAsync();

            return watchlistViewModels;
        }

        public async Task<bool> IsMovieInWatchlistAsync(string userId, string movieId)
        {
            bool isMovieInWatchlist = await this.cinemaAppDbContext.UserMovies
                                            .AnyAsync(um => um.UserId == userId && um.MovieId.ToString() == movieId);
            
            return isMovieInWatchlist;
        }

        public async Task AddToWatchlistAsync(string userId, string movieId)
        {
            UserMovie userMovie = new UserMovie()
            {
                UserId = userId,
                MovieId = Guid.Parse(movieId)
            };

            await this.cinemaAppDbContext.UserMovies.AddAsync(userMovie);
            await this.cinemaAppDbContext.SaveChangesAsync();
        }

        public async Task RemoveFromWatchlistAsync(string userId, string movieId)
        {
            UserMovie? userMovie = await this.cinemaAppDbContext.UserMovies.FirstOrDefaultAsync
                                            (um => um.UserId == userId && um.MovieId.ToString() == movieId);

            if (userMovie != null)
            {
                this.cinemaAppDbContext.Remove(userMovie);
                await this.cinemaAppDbContext.SaveChangesAsync();
            }
        }
    }
}
