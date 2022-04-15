namespace GRHelper.Web.Controllers
{
    using GRHelper.Services.Data;
    using GRHelper.Web.Infrastructure;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class RequestsController : BaseController
    {
        private readonly IRequestsService requestsService;
        private readonly IReservationsService reservationsService;
        private readonly IHotelServicesService hotelServicesService;

        public RequestsController(
            IRequestsService requestsService,
            IReservationsService reservationsService,
            IHotelServicesService hotelServicesService)
        {
            this.requestsService = requestsService;
            this.reservationsService = reservationsService;
            this.hotelServicesService = hotelServicesService;
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

        public IActionResult Create(int id)
        {
            var userId = this.User.Id();
            var reservations = this.reservationsService.AvailableByGuestId(userId).ToList();
            var serviceInfo = this.hotelServicesService.GetServiceForRequest(id);
            var model = this.requestsService.GenerateRequestModel(serviceInfo, reservations);
            return this.View(model);
        }
    }
}