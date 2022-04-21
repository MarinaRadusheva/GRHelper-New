namespace GRHelper.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using GRHelper.Common;
    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Home()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.usersService.AddEmployeeAsync(model);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("ErrorMessage", ex.Message);
                return this.View(nameof(this.Home), model);
            }

            this.TempData["Message"] = "Succcessully added employee";
            return this.RedirectToAction(nameof(this.Home));
        }

        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                await this.usersService.DeleteUserAsync(email);
                this.TempData["Message"] = "User was deleted.";
            }
            catch (Exception ex)
            {
                this.TempData["Message"] = ex.Message;
            }

            return this.RedirectToAction(nameof(this.Home));
        }
    }
}
