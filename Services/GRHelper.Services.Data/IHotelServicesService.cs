namespace GRHelper.Services.Data
{
    using System.Collections.Generic;
    using GRHelper.Services.Data.Models;
    using GRHelper.Web.ViewModels.Guests.HotelServices;

    public interface IHotelServicesService
    {
        Dictionary<string, List<HotelServiceListViewModel>> AllByCategory(string category);

        HotelServiceForRequestDto GetServiceForRequest(int serviceId);
    }
}
