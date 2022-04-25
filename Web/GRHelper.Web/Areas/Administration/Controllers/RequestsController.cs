namespace GRHelper.Web.Areas.Administration.Controllers
{
    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Administration.Requests;
    using Microsoft.AspNetCore.Mvc;
    using System;

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
            };
            return this.View(model);
        }

        public IActionResult AllSearch(string datePicker, DateTime from, DateTime to)
        {
            var requests = this.requestsService.All<RequestListViewModel>(datePicker, from, to);
            var model = new AllRequestsViewModel()
            {
                Requests = requests,
                DatePicker = datePicker,
                From = from.Date,
                To = to.Date,
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
