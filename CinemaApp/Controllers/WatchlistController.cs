using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class WatchlistController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return this.View(new List<WatchlistViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            return this.View();
        }
    }
}
