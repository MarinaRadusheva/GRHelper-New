namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System;
    using System.Collections.Generic;

    using GRHelper.Common;

    public class AllReservationsViewModel
    {
        public IEnumerable<ReservationListViewModel> Reservations { get; set; }

        public int ReservationsCount { get; set; }

        public int ReservationsPerPage { get; set; } = GlobalConstants.ItemsPerPage;

        public int PagesCount => (int)Math.Ceiling((double)this.ReservationsCount / this.ReservationsPerPage);

        public int PageNumber { get; set; }

        public bool HasPrevious => this.PageNumber > 1;

        public bool HasNext => this.PageNumber < this.PagesCount;

        public int PrevPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;
    }
}
