using AspNetCoreArchTemplate.Data;
using AspNetCoreArchTemplate.Data.Models;
using AspNetCoreArchTemplate.Services.Core.Interfaces;
using AspNetCoreArchTemplate.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreArchTemplate.Web.Controllers
{
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
		public async Task<IActionResult> Create()
		{
			return this.View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(MovieFormViewModel movieFormViewModel)
		{
			if (!ModelState.IsValid)
			{
				return this.View(movieFormViewModel);
			}

			await this.movieService.AddAsync(movieFormViewModel);
			return this.RedirectToAction(nameof(Index));
        }

		[HttpGet]
		public async Task<IActionResult> Details(string id)
		{
			MovieDetailsViewModel? movieDetails = await this.movieService.GetByIdAsync(id);

			if (movieDetails == null)
			{
				return this.NotFound();
			}

			return this.View(movieDetails);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			MovieFormViewModel? movieForm = await this.movieService.GetForEditByIdAsync(id);

			if (movieForm == null)
			{
				return this.NotFound();
			}

			return this.View(movieForm);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, MovieFormViewModel movieFormViewModel)
		{
			if (!ModelState.IsValid)
			{
				return this.View(movieFormViewModel);
			}

			await this.movieService.EditAsync(id, movieFormViewModel);

			return this.RedirectToAction(nameof(Details), new { id });
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			MovieDetailsViewModel? movieDetails = await this.movieService.GetByIdAsync(id);

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
