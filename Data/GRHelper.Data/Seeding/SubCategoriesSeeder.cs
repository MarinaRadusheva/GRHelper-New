namespace GRHelper.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data.Models;
    using Newtonsoft.Json;

    internal class SubCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.SubCategories.Any())
            {
                return;
            }

            string categoriesFromJson = File.ReadAllText(@"../../Data/GRHelper.Data/Seeding/SeedData/SubCategories.json");
            List<SubCategory> subCategories = JsonConvert.DeserializeObject<List<SubCategoryDto>>(categoriesFromJson)
                .Select(x => new SubCategory
                {
                    Name = x.Name,
                    MainCategoryId = dbContext.MainCategories.FirstOrDefault(c => c.Name == x.MainCategory).Id,
                })
                .ToList();
            await dbContext.AddRangeAsync(subCategories);
            await dbContext.SaveChangesAsync();
        }
    }
}
