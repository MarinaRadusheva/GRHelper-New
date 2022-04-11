namespace GRHelper.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using GRHelper.Common;
    using GRHelper.Data.Models;
    using GRHelper.Services.Data;
    using GRHelper.Web.Infrastructure;
    using GRHelper.Web.ViewModels;
    using GRHelper.Web.ViewModels.Guests.Reservations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IReservationsService reservationService;

        public HomeController(SignInManager<ApplicationUser> signInManager, IReservationsService reservationService)
        {
            this.signInManager = signInManager;
            this.reservationService = reservationService;
        }

        public IActionResult Index()
        {
            var user = this.signInManager.IsSignedIn(this.User);
            var userisAdmin = user ? this.User.IsInRole(GlobalConstants.AdministratorRoleName) : false;
            if (user && userisAdmin)
            {
                return View("~/Areas/Administration/Views/Administration/Index.cshtml");
            }
            else
            {
                //model with userissignedin, active reservations, unlocked reservations, last request
                var email = this.User.Email();
                var unlockedReservations = this.reservationService.GetUnlockedCount(email);
                return this.View(unlockedReservations);
            }
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
