using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly IWatchlistService watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<WatchlistViewModel> watchlistViewModel = new List<WatchlistViewModel>();
            return View(watchlistViewModel);
        }

        public IActionResult Add()
        {
            return this.RedirectToAction(nameof(Index));
        }

        public IActionResult Remove()
        {
            return this.RedirectToAction(nameof(Index));
        }
    }
}
