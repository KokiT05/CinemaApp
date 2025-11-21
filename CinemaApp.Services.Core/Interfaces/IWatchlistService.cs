using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId);

        Task<bool> IsMovieInWatchlistAsync(string userId, string movieId);// movieId can be Guid

        Task AddToWatchlistAsync(string userId, string movieId);

        Task RemoveFromWatchlistAsync(string userId, string movieId);
    }
}
