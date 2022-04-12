namespace GRHelper.Web.ViewComponents
{
    using System.Collections.Generic;

    using GRHelper.Web.ViewModels.Guests.Reservations;
    using Microsoft.AspNetCore.Mvc;

    public class ReservationsTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ReservationListViewModel> reservations)
        {
            return this.View(reservations);
        }
    }
}
