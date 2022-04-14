namespace GRHelper.Services.Data
{
    using GRHelper.Web.ViewModels.Guests.HotelServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHotelServicesService
    {
        Dictionary<string, List<HotelServiceListViewModel>> AllByCategory(string category);
    }
}
