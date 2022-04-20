namespace GRHelper.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
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

        public IActionResult All(int id = 1)
        {
            var allReservationsCount = this.reservationsService.GetCount(true);
            var reservations = this.reservationsService.All<ReservationListViewModel>(true, id);
            var model = new AllReservationsViewModel()
            {
                Reservations = reservations,
                PageNumber = id,
                ReservationsCount = allReservationsCount,
            };
            return this.View(model);
        }

        public IActionResult Archive(int id = 1)
        {
            var allReservationsCount = this.reservationsService.GetCount(false);
            var reservations = this.reservationsService.All<ReservationListViewModel>(false, id);
            var model = new AllReservationsViewModel()
            {
                Reservations = reservations,
                PageNumber = id,
                ReservationsCount = allReservationsCount,
            };
            return this.View(model);
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

            var results = this.reservationsService.GetBySearchTerms<ReservationListViewModel>(number);
            var model = new SearchResultsViewModel()
            {
                Reservations = results,
                ReservationsCount = results.Count(),
            };
            return this.View(model);
        }
    }
}
