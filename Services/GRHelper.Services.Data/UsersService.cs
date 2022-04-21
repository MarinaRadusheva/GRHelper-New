namespace GRHelper.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Common;
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> users;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(IDeletableEntityRepository<ApplicationUser> users, UserManager<ApplicationUser> userManager)
        {
            this.users = users;
            this.userManager = userManager;
        }

        public async Task AddEmployeeAsync(AddEmployeeInputModel model)
        {
            var email = model.Email;
            var password = model.Password;
            var emailExists = this.users.AllAsNoTrackingWithDeleted().Any(u => u.Email == email);
            if (emailExists)
            {
                throw new ArgumentException("Email already exists.");
            }

            var employee = new ApplicationUser
            {
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow,
                AccessFailedCount = 0,
                TwoFactorEnabled = false,
                IsDeleted = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
            };
            await this.userManager.CreateAsync(employee, password);
            await this.userManager.AddToRoleAsync(employee, GlobalConstants.EmployeeRoleName);
            await this.users.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string email)
        {
            var user = this.users.AllWithDeleted().FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new ArgumentException("User with this email does not exist");
            }

            bool isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (isAdmin)
            {
                throw new ArgumentException("User cannot be deleted");
            }

            if (user.IsDeleted)
            {
                throw new ArgumentException("User already deleted.");
            }

            this.users.Delete(user);
            await this.users.SaveChangesAsync();
        }
    }
}
