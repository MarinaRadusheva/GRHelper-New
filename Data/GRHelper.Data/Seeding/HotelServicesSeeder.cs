namespace GRHelper.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using GRHelper.Data.Models;
    using Newtonsoft.Json;

    internal class HotelServicesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.HotelServices.Any())
            {
                return;
            }

            string servicesFromJson = File.ReadAllText(@"../../Data/GRHelper.Data/Seeding/SeedData/HotelServices.json");
            List<HotelService> hotelServices = JsonConvert.DeserializeObject<List<HotelServicesDto>>(servicesFromJson)
                .Select(x => new HotelService
                {
                    Name = x.Name,
                    SubCategoryId = dbContext.SubCategories.FirstOrDefault(c => c.Name == x.SubCategory).Id,
                    Price = x.Price,
                })
                .ToList();
            await dbContext.AddRangeAsync(hotelServices);
            await dbContext.SaveChangesAsync();
        }
    }
}
