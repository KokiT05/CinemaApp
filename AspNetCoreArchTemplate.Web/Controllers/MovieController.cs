using AspNetCoreArchTemplate.Data;
using AspNetCoreArchTemplate.Data.Models;
using AspNetCoreArchTemplate.Services.Core.Interfaces;
using AspNetCoreArchTemplate.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreArchTemplate.Web.Controllers
{
	using static ViewModels.ValidationMessages.Movie;
	public class MovieController : Controller
	{
		private readonly IMovieService movieService;

		public MovieController(IMovieService movieService)
		{
			this.movieService = movieService;
		}
		public async Task<IActionResult> Index()
		{
			IEnumerable<AllMoviesIndexViewModel> movies = await this.movieService.GetAllMoviesAsync();

			return View(movies);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(MovieFormInputModel movieFormInputModel)
		{
			if (!this.ModelState.IsValid)
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

				this.ModelState.AddModelError(string.Empty, ServiceMovieError);
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
					// TODO: Custom 404 page
					return this.RedirectToAction(nameof(Index));
                }

				return this.View(movieDetails);
            }
			catch (Exception e)
			{
                // TODO: Add JS bars to indicate such errors
                // TODO: Implement it with the ILogger
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
				// TODO: Add JS bars to indicate such errors
				// TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);

				return this.RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(MovieFormInputModel movieFormInputModel)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(movieFormInputModel);
			}

			try
			{
                bool editResult = await this.movieService.EditMovieAsync(movieFormInputModel);

				if (!editResult)
				{
					// TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Details), new { id = movieFormInputModel.Id });
            }
			catch (Exception e)
			{
                // TODO: Add JS bars to indicate such errors
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			MovieDetailsViewModel? movieDetails = await this.movieService.GetMovieDetailsByIdAsync(id);

			if (movieDetails == null)
			{
				return this.NotFound();
			}

			return this.View(movieDetails);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			await this.movieService.SoftDeleteAsync(id);
			return this.RedirectToAction(nameof(Index));
		}
	}
}
