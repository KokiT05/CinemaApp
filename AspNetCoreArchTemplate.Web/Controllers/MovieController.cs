using AspNetCoreArchTemplate.Data;
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
	}
}
