using GRHelper.Data.Models;
using GRHelper.Services.Data;
using GRHelper.Web.Infrastructure;
using GRHelper.Web.ViewModels.Guests.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRHelper.Web.Controllers
{
    [Authorize]
    public class ReservationsController : BaseController
    {
        private readonly IReservationsService reservationsService;

        public ReservationsController(IReservationsService reservationsService)
        {
            this.reservationsService = reservationsService;
        }

        public IActionResult UnlockedReservations()
        {
            var email = this.User.Email();
            var unlockedReservations = this.reservationsService.GetUnlocked<UnlockedReservationViewModel>(email);
            return this.View(unlockedReservations);
        }

        [HttpPost]
        public IActionResult Unlock(int Id, string Password)
        {
            var isUnlocked = this.reservationsService.Unlock(Id, Password);
            if (isUnlocked)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                return RedirectToAction(nameof(this.UnlockedReservations));
            }
        }
    }
}
