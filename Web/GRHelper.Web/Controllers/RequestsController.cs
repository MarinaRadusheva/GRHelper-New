namespace GRHelper.Web.Controllers
{
    using GRHelper.Services.Data;
    using GRHelper.Web.Infrastructure;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.AspNetCore.Mvc;

    public class RequestsController : BaseController
    {
        private readonly IRequestsService requestsService;
        private readonly IReservationsService reservationsService;

        public RequestsController(IRequestsService requestsService, IReservationsService reservationsService)
        {
            this.requestsService = requestsService;
            this.reservationsService = reservationsService;
        }

        public IActionResult MyRequests()
        {
            var userId = this.User.Id();
            var requests = this.requestsService.AllByUserId<RequestListViewModel>(userId);
            return this.View(requests);
        }

        public IActionResult AllByReservation(int id)
        {
            var userId = this.User.Id();
            bool userIsOwner = this.reservationsService.UserIsOwner(id, userId);
            if (!userIsOwner)
            {
                return this.NotFound();
            }

            return this.View();
        }
    }
}
