namespace GRHelper.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelpe.Web.ViewModels.Guests.Requests;
    using GRHelper.Services.Data;
    using GRHelper.Web.Infrastructure;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestInputModel model)
        {
            var reservationDates = await this.reservationsService.GetByIdAsync<ReservationDatesModel>(model.ReservationId);
            var dateIsValid = HelperMethods.RequestDateIsValid(reservationDates, model.Date);

            if (!dateIsValid || !this.ModelState.IsValid)
            {
                return this.View(model.HotelServiceId);
            }

            DateTime? endDate = null;
            if (model.IsDaily)
            {
                endDate = reservationDates.To.Date;
            }

            await this.requestsService.CreateAsync(model, endDate);
            return this.RedirectToAction(nameof(this.MyRequests));
        }
    }
}