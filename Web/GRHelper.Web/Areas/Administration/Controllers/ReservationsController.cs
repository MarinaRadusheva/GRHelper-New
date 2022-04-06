namespace GRHelper.Web.Areas.Administration.Controllers
{
    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ReservationsController : AdministrationController
    {
        private readonly IReservationsService reservationsService;

        public ReservationsController(IReservationsService resService)
        {
            this.reservationsService = resService;
        }

        public IActionResult All()
        {
            int count = this.reservationsService.GetCount();
            return this.View(count);
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
    }
}
