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
            var model = this.requestsService.All<RequestListViewModel>();
            var allmodel = new AllRequestsViewModel()
            {
                Requests = model,
                From = DateTime.MinValue,
                To = DateTime.MaxValue,
            };
            return this.View(allmodel);
        }

        public IActionResult AllSearch(DateTime from, DateTime to)
        {
            var model = this.requestsService.All<RequestListViewModel>(from, to);
            var allmodel = new AllRequestsViewModel()
            {
                Requests = model,
                From = from,
                To = to,
            };
            return this.View("All", allmodel);
        }

        public IActionResult Pending()
        {
            var model = this.requestsService.GetPending<RequestListViewModel>();
            return this.View(model);
        }
    }
}
