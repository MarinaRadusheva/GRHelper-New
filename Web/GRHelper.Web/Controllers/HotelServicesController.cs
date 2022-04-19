namespace GRHelper.Web.Controllers
{
    using GRHelper.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class HotelServicesController : BaseController
    {
        private readonly IHotelServicesService hotelServices;

        public HotelServicesController(IHotelServicesService hotelServices)
        {
            this.hotelServices = hotelServices;
        }

        public IActionResult ShowHotelServices(string id)
        {
            var services = this.hotelServices.AllByCategory(id);
            this.ViewData["Category"] = id;
            return this.View(services);
        }
    }
}
