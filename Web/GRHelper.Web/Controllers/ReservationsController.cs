using GRHelper.Data.Models;
using GRHelper.Services.Data;
using GRHelper.Web.Infrastructure;
using GRHelper.Web.ViewModels.Guests.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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

        public IActionResult MyReservations()
        {
            var userId = this.User.Id();
            var userEmail = this.User.Email();
            var allReservations = this.reservationsService.AllByGuestId<ReservationListViewModel>(userId);
            var unlocked = this.reservationsService.GetUnlockedCount(userEmail);
            var reservationsModel = new AllReservationsViewModel()
            {
                FutureReservations = allReservations.Where(r => r.From > DateTime.UtcNow).ToList(),
                PresentReservations = allReservations.Where(r => r.From <= DateTime.UtcNow && r.To >= DateTime.UtcNow).ToList(),
                PastReservations = allReservations.Where(r => r.From < DateTime.UtcNow && r.To < DateTime.UtcNow).ToList(),
                UnlockedExist = unlocked != 0,
            };

            return this.View(reservationsModel);

        }

        public IActionResult UnlockedReservations(bool success = true)
        {
            var userEmail = this.User.Email();
            var unlockedReservations = this.reservationsService.GetUnlocked<ReservationListViewModel>(userEmail);
            this.ViewData["Successful"] = success;
            return this.View(unlockedReservations);
        }

        [HttpPost]
        public IActionResult Unlock(int Id, string Password)
        {
            var userId = this.User.Id();
            var isUnlocked = this.reservationsService.Unlock(Id, Password, userId);
            if (isUnlocked)
            {
                return this.RedirectToAction(nameof(this.MyReservations));
            }
            else
            {
                return this.RedirectToAction(nameof(this.UnlockedReservations), new { success = false });
            }
        }
    }
}
