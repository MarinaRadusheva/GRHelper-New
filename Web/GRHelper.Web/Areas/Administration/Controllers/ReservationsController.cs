namespace GRHelper.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.AspNetCore.Mvc;

    public class ReservationsController : AdministrationController
    {
        private readonly IReservationsService reservationsService;

        public ReservationsController(IReservationsService resService)
        {
            this.reservationsService = resService;
        }

        public IActionResult All()
        {
            var allReservations = this.reservationsService.All<AllReservationsViewModel>();
            return this.View(allReservations);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
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
            return this.View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
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
    }
}
