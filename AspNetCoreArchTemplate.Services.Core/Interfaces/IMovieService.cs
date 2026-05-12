using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreArchTemplate.Web.ViewModels.Movie;

namespace AspNetCoreArchTemplate.Services.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync();


    }
}
