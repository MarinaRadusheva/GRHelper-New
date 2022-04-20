namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System.Collections.Generic;

    public class SearchResultsViewModel
    {
        public IEnumerable<ReservationListViewModel> Reservations { get; set; }

        public int ReservationsCount { get; set; }
    }
}
