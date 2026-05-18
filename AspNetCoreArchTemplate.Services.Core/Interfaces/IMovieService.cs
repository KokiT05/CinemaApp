using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreArchTemplate.Data.Models;
using AspNetCoreArchTemplate.Web.ViewModels.Movie;

namespace AspNetCoreArchTemplate.Services.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync();

        Task AddMovieAsync(MovieFormInputModel movieFormInputModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id);

        Task<MovieFormInputModel?> GetEditableMovieByIdAsync(string? id);

        Task<bool> EditMovieAsync(MovieFormInputModel movieFormInputModel);

        Task SoftDeleteAsync(string id);

        Task HardDeleteAsync(string id);
    }
}
