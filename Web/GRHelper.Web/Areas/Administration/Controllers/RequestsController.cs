namespace GRHelper.Web.Areas.Administration.Controllers
{
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
            var model = this.requestsService.All<RequestListViewModel>();
            return this.View(model);
        }

        public IActionResult Pending()
        {
            var model = this.requestsService.GetPending<RequestListViewModel>();
            return this.View(model);
        }
    }
}
