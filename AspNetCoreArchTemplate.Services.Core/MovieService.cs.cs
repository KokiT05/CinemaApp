using AspNetCoreArchTemplate.Data;
using AspNetCoreArchTemplate.Services.Core.Interfaces;
using AspNetCoreArchTemplate.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreArchTemplate.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public MovieService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> movies = await this.applicationDbContext.Movies
                                                                .Where(m => !m.IsDeleted)
                                                                .AsNoTracking()
                                                                .Select(m => new AllMoviesIndexViewModel
                                                                {
                                                                    Id = m.Id.ToString(),
                                                                    Title = m.Title,
                                                                    Genre = m.Genre,
                                                                    ReleaseDate = m.ReleaseDate.ToString("yyyy-MM-dd"),
                                                                    Director = m.Director,
                                                                    ImageUrl = m.ImageUrl
                                                                }).ToListAsync();

            return movies;
        }
    }
}
