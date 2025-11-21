using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
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

        public WatchlistService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId)
        {
            List<WatchlistViewModel> userWatchlist = new List<WatchlistViewModel>();

            bool isValidId = Guid.TryParse(userId, out Guid id);

            if (isValidId)
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
    }
}
