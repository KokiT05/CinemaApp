using CinemaApp.Data;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext context;

        public MovieController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<AllMoviesIndexViewModel> movies = await this.context.Movies
                                                        .Select(m => new AllMoviesIndexViewModel()
                                                        {
                                                            Id = m.Id.ToString(),
                                                            Title = m.Title,
                                                            Genre = m.Genre,
                                                            ReleaseDate = m.ReleaseDate.ToString("yyyy-MM-dd"),
                                                            Director = m.Director,
                                                            ImageUrl = m.ImageUrl,

                                                        }).ToListAsync();

            return View(movies);
        }
    }
}
