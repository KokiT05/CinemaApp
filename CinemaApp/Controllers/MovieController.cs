using CinemaApp.Data;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    using static CinemaApp.Web.ViewModels.ValidationMessages.Movie;
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [AllowAnonymous]
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
        public async Task<IActionResult> Create(MovieFormInputModel movieFormInputModel)
        {
            if(this.ModelState.IsValid == false)
            {
                return this.View(movieFormInputModel);
            }

            try
            {
                await this.movieService.AddMovieAsync(movieFormInputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(movieFormInputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                MovieDetailsViewModel? movieDetails = await this.movieService.GetMovieDetailsByIdAsync(id);

                if (movieDetails == null)
                {
                    //TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(movieDetails);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                //TODO: Add JS bars to indicate such errors 
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                MovieFormInputModel? editableMovie = await this.movieService.GetEditableMovieByIdAsync(id);

                if (editableMovie == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editableMovie);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                //TODO: Add JS bars to indicate such errors 
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index)); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieFormInputModel movieInputModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(movieInputModel);
            }

            try
            {
                bool editSuccess = await this.movieService.EditMovieAsync(movieInputModel);

                if (editSuccess == false)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Details), new { id = movieInputModel.Id });
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                //TODO: Add JS bars to indicate such errors 
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            MovieDetailsViewModel movieDetailsViewModel = await this.movieService.GetMovieDetailsByIdAsync(id);

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
