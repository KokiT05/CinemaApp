using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!this.IsUserAuthenticated())
            {
                return this.RedirectToAction("Index", "Home");
            }

            string userId = this.GetUserId();

            IEnumerable<WatchlistViewModel> watchlist = 
                await this.watchlistService.GetUserWatchlistAsync(userId);

            return this.View(watchlist);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string movieId)
        {
            if (!this.IsUserAuthenticated())
            {
                return this.RedirectToAction("Index", "Home");
            }

            string userId = this.GetUserId();

            bool isMovieInWatchlist = await this.watchlistService.IsMovieInWatchlistAsync(userId, movieId);

            if (!isMovieInWatchlist)
            {
                await this.watchlistService.AddToWatchlistAsync(userId, movieId);
            }

            return this.RedirectToAction("Index", "Movie");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string movieId)
        {
            if (!this.IsUserAuthenticated())
            {
                return this.RedirectToAction("Index", "Home");
            }

            string userId = this.GetUserId();

            bool isMovieInWatchlist = await this.watchlistService.IsMovieInWatchlistAsync(userId, movieId);

            if (isMovieInWatchlist)
            {
                await this.watchlistService.RemoveFromWatchlistAsync(userId, movieId);
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
