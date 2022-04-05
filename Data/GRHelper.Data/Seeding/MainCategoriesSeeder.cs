namespace GRHelper.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data.Models;
    using Newtonsoft.Json;

    internal class MainCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.MainCategories.Any())
            {
                return;
            }

            string categoriesFromJson = File.ReadAllText(@"../../Data/GRHelper.Data/Seeding/SeedData/MainCategories.json");
            List<MainCategory> categories = JsonConvert.DeserializeObject<List<MainCategory>>(categoriesFromJson);
            await dbContext.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}
