namespace GRHelper.Web.Areas.Administration.Controllers
{
    using GRHelper.Common;
    using GRHelper.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
