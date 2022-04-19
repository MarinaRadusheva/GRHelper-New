namespace GRHelper.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using GRHelper.Data.Common;
    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.AspNetCore.Mvc;

    public class ReservationsController : AdministrationController
    {
        private readonly IReservationsService reservationsService;
        private readonly IVillasService villasService;

        public ReservationsController(IReservationsService resService, IVillasService villasService)
        {
            this.reservationsService = resService;
            this.villasService = villasService;
        }

        public IActionResult All()
        {
            var reservations = this.reservationsService.All<AllReservationsViewModel>(true);
            return this.View(reservations);
        }

        public IActionResult Archive()
        {
            var reservations = this.reservationsService.All<AllReservationsViewModel>(false);
            return this.View(reservations);
        }

        public IActionResult Create()
        {
            var model = new CreateReservationInputModel()
            { From = DateTime.Today,
              To = DateTime.Today.AddDays(1),
              AdultsCount = DataConstants.MinGuestCount,
              Villas = this.villasService.GetVillaNumbers(),
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Villas = this.villasService.GetVillaNumbers();
                return this.View(model);
            }

            await this.reservationsService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await this.reservationsService.GetByIdAsync<ReservationDetailsViewModel>(id);
            return this.View(reservation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await this.reservationsService.GetByIdAsync<EditReservationInputModel>(id);
            reservation.Villas = this.villasService.GetVillaNumbers();
            return this.View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Villas = this.villasService.GetVillaNumbers();
                return this.View(model);
            }

            await this.reservationsService.EditAsync(model);
            return this.RedirectToAction(nameof(this.Details), new { id = model.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.reservationsService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Search(int? number)
        {

            var model = this.reservationsService.GetBySearchTerms<AllReservationsViewModel>(number);

            return this.View(model);
        }
    }
}
