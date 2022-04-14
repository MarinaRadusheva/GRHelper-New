using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRHelper.Web.ViewModels.Guests.HotelServices
{
    public class HotelServiceListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
}
