namespace GRHelper.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data.Common;
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

        public IActionResult AllToday()
        {
            var model = new AllRequestsViewModel()
            {
                DatePicker = "today",
                From = DateTime.UtcNow.Date,
                To = DateTime.UtcNow.Date,
                Statuses = HelperMethods.LoadStatuses(),
            };
            var requests = this.requestsService.All<RequestListViewModel>(model.DatePicker, model.From, model.To, model.Statuses, null);
            model.Requests = requests;
            return this.View("All", model);
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

        public async Task<IActionResult> ChangeStatus(int id, string btnText)
        {
            var status = HelperMethods.GetEnumTypes<RequestStatus>().FirstOrDefault(s => HelperMethods.GetAttribute<DisplayAttribute>(s).Name == btnText);
            await this.requestsService.UpdateStatus(status.ToString(), id);
            return this.RedirectToAction(nameof(this.AllToday));
        }

        public async Task<IActionResult> Cancel(int id)
        {
            await this.requestsService.UpdateStatus(RequestStatus.Cancelled.ToString(), id);
            return this.RedirectToAction(nameof(this.AllToday));
        }
    }
}
