using CinemaApp.Data;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> movies = await this.movieService.GetAllMoviesAsync();

            return this.View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieFormViewModel movieFormViewModel)
        {
            if(this.ModelState.IsValid == false)
            {
                return this.View(movieFormViewModel);
            }

            await this.movieService.AddAsync(movieFormViewModel);
            return this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            MovieDetailsViewModel movieDetailsViewModel = await this.movieService.GetByIdAsync(id);

            if (movieDetailsViewModel == null)
            {
                
                return NotFound();
            }

            return this.View(movieDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            MovieFormViewModel? movieFormViewModel = await this.movieService.GetForEditByIdAsync(id);

            if (movieFormViewModel == null)
            {
                return NotFound();
            }

            return this.View(movieFormViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, MovieFormViewModel movieFormViewModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(movieFormViewModel);
            }

            await this.movieService.EditAsync(id, movieFormViewModel);

            return this.RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            MovieDetailsViewModel movieDetailsViewModel = await this.movieService.GetByIdAsync(id);

            if (movieDetailsViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(movieDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComfirmed(string id)
        {
            await this.movieService.SoftDeleteAsync(id);
            return this.RedirectToAction(nameof(Index));
        }
    }
}
