using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRHelper.Web.ViewModels.Guests.Reservations
{
    public class ReservationForRequestModel : ReservationListViewModel
    {
        public string VillaNumber { get; set; }

        public override string ToString()
        {
            return "No: " + this.Number + " - " + this.VillaNumber;
        }
    }
}
