using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaApp.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected bool IsUserAuthenticated()
        {
            if (this.User == null)
            {
                return false;
            }

            if (this.User.Identity == null)
            {
                return false;
            }

            return this.User.Identity!.IsAuthenticated;
        }

        protected string GetUserId()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
