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

        Task AddAsync(MovieFormViewModel movieFormViewModel);

        Task<MovieDetailsViewModel?> GetByIdAsync(string id);

        Task<MovieFormViewModel?> GetForEditByIdAsync(string id);

        Task EditAsync(string id, MovieFormViewModel movieFormViewModel);

        Task SoftDeleteAsync(string id);

        Task HardDeleteAsync(string id);
    }
}
