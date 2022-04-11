using GRHelper.Web.ViewModels.Guests.Reservations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GRHelper.Web.ViewComponents
{
    public class ReservationsTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ReservationListViewModel> reservations)
        {
            return this.View(reservations);
        }
    }
}
