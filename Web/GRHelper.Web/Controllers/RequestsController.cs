namespace GRHelper.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Common;
    using GRHelper.Data.Common;
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

        public IActionResult MyRequests(string id = GlobalConstants.ActiveRequestsRouteId)
        {
            string showType = id;
            if (showType != GlobalConstants.ActiveRequestsRouteId && showType != GlobalConstants.ArchivedRequestsRouteId)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var userId = this.User.Id();
            var requests = this.requestsService.AllByUserId<RequestListViewModel>(userId, showType);
            var toggleButtonText = showType == GlobalConstants.ActiveRequestsRouteId ? GlobalConstants.ArchivedRequestsRouteId : GlobalConstants.ActiveRequestsRouteId;
            this.ViewData["ShowType"] = toggleButtonText;
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

            var requests = this.requestsService.AllByReservationId<RequestListViewModel>(id);
            return this.View(requests);
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
                return this.RedirectToAction(nameof(this.Create), new { id=model.HotelServiceId });
            }

            DateTime? endDate = null;
            if (model.IsDaily)
            {
                endDate = reservationDates.To.Date;
            }

            await this.requestsService.CreateAsync(model, endDate);
            return this.RedirectToAction(nameof(this.MyRequests), new { id = GlobalConstants.ActiveRequestsRouteId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = this.User.Id();
            var userIsOwner = this.requestsService.UserIsOwner(id, userId);
            if (!userIsOwner)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var requestToEdit = await this.requestsService.GetById<EditRequestInputModel>(id);
            var serviceInfo = this.hotelServicesService.GetServiceForRequest(requestToEdit.HotelServiceId);
            requestToEdit = this.requestsService.GenerateEditModel(requestToEdit, serviceInfo);
            return this.View(requestToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRequestInputModel model)
        {
            var reservationDates = await this.reservationsService.GetByIdAsync<ReservationDatesModel>(model.ReservationId);
            var dateIsValid = HelperMethods.RequestDateIsValid(reservationDates, model.Date);

            if (!dateIsValid || !this.ModelState.IsValid)
            {
                if (!dateIsValid)
                {
                    this.ViewData["Message"] = "Date must be during the reservation.";
                }

                var serviceInfo = this.hotelServicesService.GetServiceForRequest(model.HotelServiceId);
                model = this.requestsService.GenerateEditModel(model, serviceInfo);
                return this.View(model);
            }

            DateTime? endDate = null;
            if (model.IsDaily)
            {
                endDate = reservationDates.To.Date;
            }

            await this.requestsService.EditAsync(model, endDate);
            return this.RedirectToAction(nameof(this.MyRequests));
        }

        public async Task<IActionResult> Cancel(int id)
        {
            await this.requestsService.UpdateStatus(RequestStatus.Cancelled.ToString(), id);
            return this.RedirectToAction(nameof(this.MyRequests));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.requestsService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.MyRequests));
        }
    }
}
