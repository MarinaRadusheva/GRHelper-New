namespace GRHelper.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using GRHelper.Data.Models;
    using Newtonsoft.Json;

    internal class VillasSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Villas.Any())
            {
                return;
            }

            string villasFromJson = File.ReadAllText(@"../../Data/GRHelper.Data/Seeding/SeedData/Villas.json");
            List<Villa> villas = JsonConvert.DeserializeObject<List<Villa>>(villasFromJson);
            await dbContext.AddRangeAsync(villas);
            await dbContext.SaveChangesAsync();
        }
    }
}
