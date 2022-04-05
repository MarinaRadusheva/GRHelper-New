namespace GRHelper.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Common;
    using GRHelper.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            const string adminEmail = "master@gr.com";
            const string adminPass = "admin123";

            var adminExists = dbContext.Users.Any(u => u.Email == adminEmail);
            if (adminExists)
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var admin = new ApplicationUser
            {
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow,
                AccessFailedCount = 0,
                TwoFactorEnabled = false,
                IsDeleted = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
            };
            await userManager.CreateAsync(admin, adminPass);
            await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
            await dbContext.SaveChangesAsync();
        }
    }
}
