namespace GRHelper.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;

    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Administration.Requests;
    using Microsoft.AspNetCore.Mvc;

    public class RequestsController : AdministrationController
    {
        private readonly IRequestsService requestsService;

        public RequestsController(IRequestsService requestsService)
        {
            this.requestsService = requestsService;
        }

        public IActionResult All()
        {
            var requests = this.requestsService.All<RequestListViewModel>();
            var model = new AllRequestsViewModel()
            {
                Requests = requests,
                From = DateTime.UtcNow.Date,
                To = DateTime.UtcNow.Date,
                Statuses = HelperMethods.LoadStatuses(),
            };
            return this.View(model);
        }

        public IActionResult AllSearch(string datePicker, DateTime from, DateTime to, List<StatusForRequestSearchModel> statuses, int? reservationNumber)
        {
            var requests = this.requestsService.All<RequestListViewModel>(datePicker, from, to, statuses, reservationNumber);
            var model = new AllRequestsViewModel()
            {
                Requests = requests,
                DatePicker = datePicker,
                Statuses = statuses,
                From = from.Date == DateTime.MinValue ? DateTime.UtcNow.Date : from,
                To = to.Date == DateTime.MinValue ? DateTime.UtcNow.Date : to,
                ReservationNumber = reservationNumber,
            };
            return this.View("All", model);
        }

        public IActionResult Pending()
        {
            var model = this.requestsService.GetPending<RequestListViewModel>();
            return this.View(model);
        }
    }
}
