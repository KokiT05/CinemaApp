using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    using static GCommon.ApplicationConstants;
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public WatchlistService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId)
        {
            List<WatchlistViewModel> userWatchlist = new List<WatchlistViewModel>();

            bool isUserExist = await this.userManager.Users.AnyAsync(u => u.Id == userId);

            if (isUserExist)
            {
                userWatchlist = await this.dbContext.UserMovies
                                            .Include(um => um.Movie)
                                            .AsNoTracking()
                                            .Where(um => um.UserId == userId)
                                            .Select(um => new WatchlistViewModel()
                                            {
                                                MovieId = um.MovieId.ToString(),
                                                Title = um.Movie.Title,
                                                Genre = um.Movie.Genre,
                                                ReleaseDate = um.Movie.ReleaseDate
                                                                .ToString(ApplicationDateFormat),
                                                ImageUrl = um.Movie.ImageUrl,
                                            }).ToListAsync();
            }

            return userWatchlist;
        }

        public async Task<bool> IsMovieInWatchlistAsync(string userId, string movieId)
        {
            bool isMovieInWatchlist = await this.dbContext.UserMovies
                                        .AnyAsync(um => um.UserId == userId && 
                                                    um.MovieId.ToString() == movieId);

            return isMovieInWatchlist;
        }

        // This metod can return bool
        public async Task AddToWatchlistAsync(string userId, string movieId)
        {
            bool isUserExist = await this.userManager.Users.AnyAsync(u => u.Id == userId);
            bool isMovieExist = await this.dbContext.Movies.AnyAsync(m => m.Id.ToString() == movieId);

            if (isUserExist && isMovieExist)
            {

                UserMovie userMovie = new UserMovie()
                {
                    UserId = userId,
                    MovieId = Guid.Parse(movieId)
                };

                await this.dbContext.UserMovies.AddAsync(userMovie);
                await this.dbContext.SaveChangesAsync();
            }
        }

        // This metod can return bool
        public async Task RemoveFromWatchlistAsync(string userId, string movieId)
        {
            bool isUserExist = await this.userManager.Users.AnyAsync(u => u.Id == userId);
            bool isMovieExist = await this.dbContext.Movies.AnyAsync(m => m.Id.ToString() == movieId);

            if (isUserExist && isMovieExist)
            {

                UserMovie userMovie = await this.dbContext.UserMovies.FindAsync(userId, movieId);

                this.dbContext.UserMovies.Remove(userMovie);
            }

            // or
            //UserMovie userMovie = await this.dbContext.UserMovies.FindAsync(userId, movieId);

            //if (userMovie != null)
            //{
            //    this.dbContext.UserMovies.Remove(userMovie);
            //}
        }
    }
}
