namespace GRHelper.Web.Controllers
{
    using System.Diagnostics;

    using GRHelper.Common;
    using GRHelper.Data.Models;
    using GRHelper.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var user = this.signInManager.IsSignedIn(this.User);
            var userisAdmin = user ? this.User.IsInRole(GlobalConstants.AdministratorRoleName) : false;
            if (user && userisAdmin)
            {
                return View("~/Areas/Administration/Views/Administration/Index.cshtml");
            }
            //model with userissignedin, active reservations, unlocked reservations, last request
            else { return this.View(); }
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
