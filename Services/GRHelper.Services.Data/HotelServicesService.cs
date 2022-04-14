namespace GRHelper.Services.Data
{
    using GRHelper.Data.Common.Repositories;
    using GRHelper.Data.Models;
    using GRHelper.Web.ViewModels.Guests.HotelServices;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HotelServicesService : IHotelServicesService
    {
        private readonly IDeletableEntityRepository<HotelService> hotelServices;
        private readonly IDeletableEntityRepository<SubCategory> subCategories;

        public HotelServicesService(
            IDeletableEntityRepository<HotelService> hotelServices,
            IDeletableEntityRepository<SubCategory> subCategories)
        {
            this.hotelServices = hotelServices;
            this.subCategories = subCategories;
        }

        public Dictionary<string, List<HotelServiceListViewModel>> AllByCategory(string category)
        {
            var subCategories = this.subCategories.AllAsNoTracking()
                .Include(s => s.MainCategory)
                .Where(s => s.MainCategory.Name == category)
                .Select(s => s.Name)
                .ToList();
            var services = this.hotelServices.AllAsNoTracking()
                .Include(s => s.SubCategory)
                .Where(s => subCategories.Any(n => n == s.SubCategory.Name))
                .Select(s => new
                {
                    Id = s.Id,
                    SubCategory = s.SubCategory.Name,
                    Name = s.Name,
                    Price = s.Price,
                })
                .ToList();
            var sortedServices = new Dictionary<string, List<HotelServiceListViewModel>>();
            foreach (var service in services)
            {
                if (!sortedServices.ContainsKey(service.SubCategory))
                {
                    sortedServices[service.SubCategory] = new List<HotelServiceListViewModel>();
                }

                sortedServices[service.SubCategory].Add(new HotelServiceListViewModel()
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price,
                });
            }

            return sortedServices;
        }
    }
}
